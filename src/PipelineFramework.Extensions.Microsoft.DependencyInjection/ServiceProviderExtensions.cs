using Microsoft.Extensions.DependencyInjection;
using PipelineFramework.Abstractions;
using System;
using System.Linq;

namespace PipelineFramework.Extensions.Microsoft.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        public static IAsyncPipeline<TPayload> GetAsyncPipeline<TPayload>(this IServiceProvider provider,
            string pipelineName,
            StringComparison comparisonType)
        {
            if (provider == null) return null;
            if (pipelineName == null) throw new ArgumentNullException(nameof(pipelineName));

            return provider.GetServices<IAsyncPipeline<TPayload>>()
                .FirstOrDefault(p => p.Name.Equals(pipelineName, comparisonType));
        }

        public static IAsyncPipeline<TPayload> GetAsyncPipeline<TPayload>(this IServiceProvider provider,
            string pipelineName)
            => provider.GetAsyncPipeline<TPayload>(pipelineName, StringComparison.CurrentCulture);

        public static IPipeline<TPayload> GetPipeline<TPayload>(this IServiceProvider provider,
            string pipelineName,
            StringComparison comparisonType)
        {
            if (provider == null) return null;
            if (pipelineName == null) throw new ArgumentNullException(nameof(pipelineName));

            return provider.GetServices<IPipeline<TPayload>>()
                .FirstOrDefault(p => p.Name.Equals(pipelineName, comparisonType));
        }

        public static IPipeline<TPayload> GetPipeline<TPayload>(this IServiceProvider provider,
            string pipelineName)
            => provider.GetPipeline<TPayload>(pipelineName, StringComparison.CurrentCulture);
    }
}