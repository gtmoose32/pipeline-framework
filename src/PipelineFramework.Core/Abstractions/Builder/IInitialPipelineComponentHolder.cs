using System;

namespace PipelineFramework.Abstractions.Builder
{
    /// <summary>
    /// Defines a pipeline component holder for a pipeline builder.
    /// </summary>
    /// <typeparam name="TPipeline">Type of pipeline to build.</typeparam>
    /// <typeparam name="TComponentBase">Base type of pipeline components used to build the pipeline.</typeparam>
    /// <typeparam name="TPayload">Type of the payload the pipeline will use during execution.</typeparam>
    public interface IInitialPipelineComponentHolder<out TPipeline, TComponentBase, TPayload>
        where TPipeline : IPipeline
        where TComponentBase : IPipelineComponent 
    {
        /// <summary>
        /// Adds a pipeline component to the pipeline component holder by type.
        /// </summary>
        /// <typeparam name="TComponent">The type to add to the pipeline component holder.</typeparam>
        /// <returns>Additional pipeline component holder.</returns>
        IAdditionalPipelineComponentHolder<TPipeline, TComponentBase, TPayload> WithComponent<TComponent>() where TComponent : TComponentBase;
        
        /// <summary>
        /// Adds a pipeline component to the pipeline component holder by type.
        /// </summary>
        /// <returns>Additional pipeline component holder.</returns>
        IAdditionalPipelineComponentHolder<TPipeline, TComponentBase, TPayload> WithComponent(Type componentType);
    }
}