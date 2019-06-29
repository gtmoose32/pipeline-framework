using PipelineFramework.Abstractions;
using PipelineFramework.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading;

namespace PipelineFramework
{
    /// <summary>
    /// Pipeline implementation that provides execution of a linear workflow.
    /// </summary>
    /// <typeparam name="T">Type of payload that travels down the execution pipeline.</typeparam>
    public class Pipeline<T> : PipelineBase<IPipelineComponent<T>>, IPipeline<T>
    {
        #region ctor
        /// <inheritdoc />
        public Pipeline(
            IPipelineComponentResolver resolver,
            IEnumerable<string> componentNames,
            IDictionary<string, IDictionary<string, string>> settings)
        : base(resolver, componentNames, settings)
        { }

        /// <inheritdoc />
        public Pipeline(
            IPipelineComponentResolver resolver,
            IEnumerable<Type> componentTypes,
            IDictionary<string, IDictionary<string, string>> settings)
            : base(resolver, componentTypes, settings)
        { }
        #endregion

        /// <inheritdoc />
        public T Execute(T payload, CancellationToken cancellationToken = default)
        {
            IPipelineComponent current = null;
            try
            {
                foreach (var component in Components)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    current = component;
                    var currentPayload = component.Execute(payload, cancellationToken);
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
    }
}