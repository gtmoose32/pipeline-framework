using PipelineFramework.Abstractions;
using PipelineFramework.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PipelineFramework
{
    /// <summary>
    /// Asynchronous pipeline implementation that provides execution of a linear workflow.
    /// </summary>
    /// <typeparam name="T">Type of payload that travels down the execution pipeline.</typeparam>
    public class AsyncPipeline<T> : PipelineBase<IAsyncPipelineComponent<T>>, IAsyncPipeline<T>
    {
        #region ctor
        /// <inheritdoc />
        public AsyncPipeline(
            IPipelineComponentResolver resolver,
            IEnumerable<string> componentNames,
            IDictionary<string, IDictionary<string, string>> settings = null)
        : base(resolver, componentNames, settings)
        { }

        /// <inheritdoc />
        public AsyncPipeline(
            IPipelineComponentResolver resolver,
            IEnumerable<Type> componentTypes,
            IDictionary<string, IDictionary<string, string>> settings = null)
            : base(resolver, componentTypes, settings)
        { }
        #endregion

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
                    var currentPayload = await component.ExecuteAsync(payload, cancellationToken);
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
    }
}
