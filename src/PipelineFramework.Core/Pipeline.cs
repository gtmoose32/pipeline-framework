using PipelineFramework.Abstractions;
using PipelineFramework.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace PipelineFramework
{
    /// <summary>
    /// Pipeline implementation that provides execution of a linear workflow.
    /// </summary>
    /// <typeparam name="T">Type of payload that travels down the execution pipeline.</typeparam>
    internal class Pipeline<T> : PipelineBase<IPipelineComponent<T>>, IPipeline<T>
    {
        private readonly IPipelineComponentExecutionStatusReceiver _componentExecutionStatusReceiver;

        #region ctor
        public Pipeline(
            IPipelineComponentResolver resolver,
            IEnumerable<string> componentNames,
            IDictionary<string, IDictionary<string, string>> settings,
            IPipelineComponentExecutionStatusReceiver componentExecutionStatusReceiver)
        : base(resolver, componentNames, settings)
        {
            _componentExecutionStatusReceiver = componentExecutionStatusReceiver;
        }

        /// <inheritdoc />
        public Pipeline(
            IPipelineComponentResolver resolver,
            IEnumerable<Type> componentTypes,
            IDictionary<string, IDictionary<string, string>> settings,
            IPipelineComponentExecutionStatusReceiver componentExecutionStatusReceiver)
            : base(resolver, componentTypes, settings)
        {
            _componentExecutionStatusReceiver = componentExecutionStatusReceiver;
        }
        #endregion

        /// <inheritdoc />
        public T Execute(T payload) => Execute(payload, default);

        /// <inheritdoc />
        public T Execute(T payload, CancellationToken cancellationToken)
        {
            IPipelineComponent current = null;
            try
            {
                foreach (var component in Components)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    current = component;
                    var currentPayload =  ExecuteComponent(component, payload, cancellationToken);
                    if (currentPayload == null) break;

                    payload = currentPayload;
                }

                return payload;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new PipelineExecutionException(current, exception);
            }
        }

        private T ExecuteComponent(IPipelineComponent<T> component, T payload, CancellationToken cancellationToken)
        {
            if (_componentExecutionStatusReceiver == null)
                return component.Execute(payload, cancellationToken);

            _componentExecutionStatusReceiver.ReceiveExecutionStarting(new PipelineComponentExecutionStartingInfo(component.Name));
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                var result = component.Execute(payload, cancellationToken);
                stopwatch.Stop();
                _componentExecutionStatusReceiver.ReceiveExecutionCompleted(new PipelineComponentExecutionCompletedInfo(component.Name, stopwatch.Elapsed));
                return result;
            }
            catch (Exception e)
            {
                stopwatch.Stop();
                _componentExecutionStatusReceiver.ReceiveExecutionCompleted(new PipelineComponentExecutionCompletedInfo(component.Name, stopwatch.Elapsed, e));
                throw;
            }
        }
    }
}