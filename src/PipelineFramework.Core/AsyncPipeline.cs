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
    internal class AsyncPipeline<T> : PipelineBase<IAsyncPipelineComponent<T>>, IAsyncPipeline<T>
    {
        #region ctor
        /// <inheritdoc />
        internal AsyncPipeline(
            IPipelineComponentResolver resolver,
            IEnumerable<string> componentNames,
            IDictionary<string, IDictionary<string, string>> settings)
        : base(resolver, componentNames, settings)
        { }

        /// <inheritdoc />
        internal AsyncPipeline(
            IPipelineComponentResolver resolver,
            IEnumerable<Type> componentTypes,
            IDictionary<string, IDictionary<string, string>> settings)
            : base(resolver, componentTypes, settings)
        { }
        #endregion

        /// <summary>
        /// Executes linear work flow asynchronously.
        /// </summary>
        /// <param name="payload">Type of payload passed through the pipeline during execution.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/> using to cancel execution.</param>
        /// <returns><see cref="Task{T}"/></returns>

        public async Task<T> ExecuteAsync(T payload, CancellationToken cancellationToken = default)
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
