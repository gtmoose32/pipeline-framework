using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PipelineFramework.Abstractions
{
    /// <summary>
    /// Abstract base class for asynchronous pipeline components.
    /// </summary>
    /// <typeparam name="T">Type of payload to be used by component.</typeparam>
    public abstract class AsyncPipelineComponentBase<T> : IAsyncPipelineComponent<T>
    {
        /// <summary>
        /// 
        /// </summary>
        protected AsyncPipelineComponentBase()
        {
            Settings = new Settings(this);
        }

        /// <inheritdoc />
        public abstract Task<T> ExecuteAsync(T payload, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the pipeline component settings.
        /// </summary>
        protected IDictionary<string, string> Settings { get; }

        /// <inheritdoc />
        public string Name { get; private set; }

        /// <inheritdoc />
        public virtual void Initialize(string name, IDictionary<string, string> settings)
        {
            Name = name;

            if (settings == null)
                return;

            Settings.AddRange(settings.Select(kvp => kvp));
        }
    }
}
