using PipelineFramework.Abstractions;
using System;

namespace PipelineFramework.Autofac
{
    public class PipelineComponentResolver : IPipelineComponentResolver
    {
        public T GetInstance<T>(string name) where T : IPipelineComponent
        {
            throw new NotImplementedException();
        }
    }
}
