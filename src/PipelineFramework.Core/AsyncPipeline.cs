using PipelineFramework.Abstractions;
using PipelineFramework.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
#pragma warning disable 4014

namespace PipelineFramework
{
    /// <summary>
    /// Asynchronous pipeline implementation that provides execution of a linear workflow.
    /// </summary>
    /// <typeparam name="T">Type of payload that travels down the execution pipeline.</typeparam>
    internal class AsyncPipeline<T> : PipelineBase<IAsyncPipelineComponent<T>>, IAsyncPipeline<T>
    {
        private readonly IAsyncPipelineComponentExecutionStatusReceiver _componentExecutionStatusReceiver;

        #region ctor
        /// <inheritdoc />
        public AsyncPipeline(
            IPipelineComponentResolver resolver,
            IEnumerable<string> componentNames,
            IDictionary<string, IDictionary<string, string>> settings,
            IAsyncPipelineComponentExecutionStatusReceiver componentExecutionStatusReceiver)
        : base(resolver, componentNames, settings)
        {
            _componentExecutionStatusReceiver = componentExecutionStatusReceiver;
        }

        /// <inheritdoc />
        public AsyncPipeline(
            IPipelineComponentResolver resolver,
            IEnumerable<Type> componentTypes,
            IDictionary<string, IDictionary<string, string>> settings,
            IAsyncPipelineComponentExecutionStatusReceiver componentExecutionStatusReceiver)
            : base(resolver, componentTypes, settings)
        {
            _componentExecutionStatusReceiver = componentExecutionStatusReceiver;
        }
        #endregion


        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public Task<T> ExecuteAsync(T payload) => ExecuteAsync(payload, default);

        /// <inheritdoc />
        public async Task<T> ExecuteAsync(T payload, CancellationToken cancellationToken)
        {
            IPipelineComponent current = null;
            try
            {
                foreach (var component in Components)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    current = component;
                    var currentPayload = await ExecuteComponentAsync(component, payload, cancellationToken).ConfigureAwait(false);
                    if (currentPayload == null) break;

                    payload = currentPayload;
                }

                return payload;
            }
            catch(OperationCanceledException)
            {
                throw;
            }
            catch (Exception exception)
            {
                throw new PipelineExecutionException(current, exception);
            }
        }

        private const string NullTaskExceptionMessage = "AsyncPipelineComponent named '{0}' returned a null task.";

        private async Task<T> ExecuteComponentAsync(IAsyncPipelineComponent<T> component, T payload, CancellationToken cancellationToken)
        {
            if (_componentExecutionStatusReceiver == null)
            {
                var task = component.ExecuteAsync(payload, cancellationToken) 
                           ?? throw new InvalidOperationException(string.Format(NullTaskExceptionMessage, component.Name));
                return await task.ConfigureAwait(false);
            }

            var executionStartingInfo = new PipelineComponentExecutionStartingInfo(component.Name, payload);
            await _componentExecutionStatusReceiver.ReceiveExecutionStartingAsync(
                    executionStartingInfo)
                .ConfigureAwait(false);

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                var task = component.ExecuteAsync(payload, cancellationToken) 
                           ?? throw new InvalidOperationException(string.Format(NullTaskExceptionMessage, component.Name));
                var result = await task.ConfigureAwait(false);

                stopwatch.Stop();
                await _componentExecutionStatusReceiver.ReceiveExecutionCompletedAsync(
                        new PipelineComponentExecutionCompletedInfo(executionStartingInfo, stopwatch.Elapsed))
                    .ConfigureAwait(false);

                return result;
            }
            catch (Exception e)
            {
                stopwatch.Stop();
                await _componentExecutionStatusReceiver.ReceiveExecutionCompletedAsync(
                        new PipelineComponentExecutionCompletedInfo(executionStartingInfo, stopwatch.Elapsed, e))
                    .ConfigureAwait(false);

                throw;
            }
        }
    }
}
