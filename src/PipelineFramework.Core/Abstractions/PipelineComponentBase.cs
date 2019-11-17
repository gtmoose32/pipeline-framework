using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PipelineFramework.Abstractions
{
    /// <summary>
    /// Abstract base class for pipeline components.
    /// </summary>
    /// <typeparam name="T">Type of payload to be used by component.</typeparam>
    public abstract class PipelineComponentBase<T> : PipelineComponentBase, IPipelineComponent<T>
    {
        public abstract T Execute(T payload, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Abstract base class for all pipeline components, both async and sync.
    /// </summary>
    public abstract class PipelineComponentBase : IPipelineComponent
    {
        protected PipelineComponentBase()
        {
            Settings = new Settings(this);
        }

        public string Name { get; private set; }

        public virtual void Initialize(string name, IDictionary<string, string> settings)
        {
            Name = name;

            if (settings == null)
                return;

            Settings.AddRange(settings.Select(kvp => kvp));
        }

        /// <summary>
        /// Gets the pipeline component settings.
        /// </summary>
        protected IDictionary<string, string> Settings { get; }
    }
}
