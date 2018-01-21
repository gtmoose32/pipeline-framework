using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PipelineFramework.Abstractions
{
    /// <summary>
    /// Abstract base class for pipeline components.
    /// </summary>
    /// <typeparam name="T">Type of payload to be used by component.</typeparam>
    public abstract class PipelineComponentBase<T> : IPipelineComponent<T>
    {
        /// <summary>
        /// 
        /// </summary>
        protected PipelineComponentBase()
        {
            Settings = new Settings(this);
        }

        /// <inheritdoc />
        public abstract T Execute(T payload, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the pipeline component settings.
        /// </summary>
        protected Settings Settings { get; }

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
