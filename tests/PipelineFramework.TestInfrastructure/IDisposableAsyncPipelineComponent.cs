using PipelineFramework.Abstractions;
using System;

namespace PipelineFramework.TestInfrastructure
{
    public interface IDisposableAsyncPipelineComponent : IAsyncPipelineComponent<TestPayload>, IDisposable
    {

    }
}