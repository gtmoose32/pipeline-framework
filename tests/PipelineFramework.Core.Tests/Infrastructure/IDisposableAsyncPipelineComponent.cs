using PipelineFramework.Abstractions;
using System;

namespace PipelineFramework.Core.Tests.Infrastructure
{
    public interface IDisposableAsyncPipelineComponent : IAsyncPipelineComponent<TestPayload>, IDisposable
    {

    }
}