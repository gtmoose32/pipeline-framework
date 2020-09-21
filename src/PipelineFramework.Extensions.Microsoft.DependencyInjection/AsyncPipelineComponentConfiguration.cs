using PipelineFramework.Abstractions;
using System;

namespace PipelineFramework.Extensions.Microsoft.DependencyInjection
{
    /// <inheritdoc />
    public class AsyncPipelineComponentConfiguration<TPayload> : PipelineComponentConfigurationBase
    {
        internal AsyncPipelineComponentConfiguration() { }

        /// <summary>
        /// Adds an <see cref="IAsyncPipelineComponent{T}"/> to the <see cref="IAsyncPipeline{T}"/> configuration.
        /// </summary>
        /// <param name="componentFactory">Factory used to provide new instances of the specified type param.  Only one custom component factory per type is supported and last one added wins.</param>
        /// <typeparam name="TComponent">Type that implements <see cref="IAsyncPipelineComponent{T}"/></typeparam>
        /// <returns><see cref="AsyncPipelineComponentConfiguration{TPayload}"/></returns>
        public AsyncPipelineComponentConfiguration<TPayload> WithComponent<TComponent>(Func<IServiceProvider, TComponent> componentFactory = null) 
            where TComponent : class, IAsyncPipelineComponent<TPayload>
        {
            AddComponent(componentFactory);
            return this;
        }
    }
}