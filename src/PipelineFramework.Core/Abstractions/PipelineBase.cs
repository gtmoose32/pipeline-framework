using System;
using System.Collections.Generic;
using System.Linq;

namespace PipelineFramework.Abstractions
{
    /// <summary>
    /// Abstract pipeline implementation providing base pipeline functionality.
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    public abstract class PipelineBase<TComponent> : IDisposable
        where TComponent : class, IPipelineComponent
    {
        #region ctor
        /// <summary>
        /// Abstract pipeline ctor.
        /// </summary>
        /// <param name="resolver">Provides <see cref="IPipelineComponent"/> dependency resolution.</param>
        /// <param name="componentNames">Names of the pipeline components that are contained within this pipeline. 
        /// Order of names denotes order of component execution in the pipeline.</param>
        /// <param name="settings">Configuration settings for the pipeline and all of it's components.</param>
        protected PipelineBase(
            IPipelineComponentResolver resolver,
            IEnumerable<string> componentNames,
            IDictionary<string, IDictionary<string, string>> settings)
        {
            if (resolver == null) throw new ArgumentNullException(nameof(resolver));
            if (componentNames == null) throw new ArgumentNullException(nameof(componentNames));

            Components = componentNames.Select(name =>
            {
                IDictionary<string, string> componentSettings = null;
                if (settings != null && settings.ContainsKey(name))
                    componentSettings = settings[name];

                var component = resolver.GetInstance<TComponent>(name);
                component.Initialize(name, componentSettings);
                return component;
            });
        }

        /// <summary>
        /// Abstract pipeline ctor.
        /// </summary>
        /// <param name="resolver">Provides <see cref="IPipelineComponent"/> dependency resolution.</param>
        /// <param name="componentTypes">List of pipeline components types that are contained within this pipeline. 
        /// Order of Types denotes order of component execution in the pipeline.</param>
        /// <param name="settings">Configuration settings for the pipeline and all of it's components.</param>
        protected PipelineBase(
            IPipelineComponentResolver resolver,
            IEnumerable<Type> componentTypes,
            IDictionary<string, IDictionary<string, string>> settings)
            : this(resolver, componentTypes.Select(t => t.Name), settings)
        { }
        #endregion

        /// <summary>
        /// List of components contained with this pipeline instance.
        /// </summary>
        protected IEnumerable<TComponent> Components { get; }

        /// <summary>
        /// Disposes of the pipeline.  This method will call dispose on any <see cref="IPipelineComponent"/> that implements the <see cref="IDisposable"/> interface.
        /// </summary>
        public void Dispose()
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            foreach (var component in Components.OfType<IDisposable>())
            {
                component.Dispose();
            }
        }
    }
}
