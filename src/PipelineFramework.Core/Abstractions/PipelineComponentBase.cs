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
        /// <inheritdoc />
        public abstract T Execute(T payload, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Abstract base class for all pipeline components, both async and sync.
    /// </summary>
    public abstract class PipelineComponentBase : IPipelineComponent
    {
        /// <summary>
        /// Initializes a new pipeline component instance.
        /// </summary>
        protected PipelineComponentBase()
        {
            Settings = new Settings(this);
        }

        /// <inheritdoc />
        public string Name => GetType().Name;

        /// <inheritdoc />
        public virtual void Initialize(IDictionary<string, string> settings)
        {
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
