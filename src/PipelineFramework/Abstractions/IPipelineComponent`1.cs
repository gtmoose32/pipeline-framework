﻿using System.Threading;

namespace PipelineFramework.Abstractions
{
    /// <summary>
    /// Defines the behavior of a pipeline component.
    /// </summary>
    /// <typeparam name="T">The object that participates in the execution of the pipeline component.</typeparam>
    public interface IPipelineComponent<T> : IPipelineComponent
    {
        /// <summary>
        /// Executes this pipeline component.
        /// </summary>
        /// <param name="payload">Payload object this instance uses for execution.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Payload object returned after this instance execution complete.</returns>
        T Execute(T payload, CancellationToken cancellationToken);
    }
}
