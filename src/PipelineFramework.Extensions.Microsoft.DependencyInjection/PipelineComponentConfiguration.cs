using PipelineFramework.Abstractions;
using System;

namespace PipelineFramework.Extensions.Microsoft.DependencyInjection
{
    /// <inheritdoc />
    public class PipelineComponentConfiguration<TPayload> : PipelineComponentConfigurationBase
    {
        internal PipelineComponentConfiguration() { }

        /// <summary>
        /// Adds an <see cref="IPipelineComponent{T}"/> to the <see cref="IPipeline{T}"/> configuration.
        /// </summary>
        /// <param name="componentFactory">Factory used to provide new instances of the specified type param.  Only one custom component factory per type is supported and last one added wins.</param>
        /// <typeparam name="TComponent">Type that implements <see cref="IPipelineComponent{T}"/></typeparam>
        /// <returns><see cref="PipelineComponentConfiguration{TPayload}"/></returns>
        public PipelineComponentConfiguration<TPayload> WithComponent<TComponent>(Func<IServiceProvider, TComponent> componentFactory = null) 
            where TComponent : class, IPipelineComponent<TPayload>
        {
            AddComponent(componentFactory);
            return this;
        }
    }
}