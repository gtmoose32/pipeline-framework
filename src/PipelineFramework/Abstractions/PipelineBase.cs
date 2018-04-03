using System;
using System.Collections.Generic;
using System.Linq;

namespace PipelineFramework.Abstractions
{
    /// <summary>
    /// Abstract pipeline implementation providing base pipeline functionality.
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    public abstract class PipelineBase<TComponent>
        where TComponent : IPipelineComponent
    {
        #region ctor
        /// <summary>
        /// Abstract pipeline ctor.
        /// </summary>
        /// <param name="resolver">Provides <see cref="IPipelineComponent"/> dependency resolution.</param>
        /// <param name="componentNames">Names of the pipeline components that are containened within this pipeline. 
        /// Order of names denotes order of component execution in the pipeline.</param>
        /// <param name="settings">Configuration settings for the pipeline and all of it's components.</param>
        protected PipelineBase(
            IPipelineComponentResolver resolver,
            IEnumerable<string> componentNames,
            IDictionary<string, IDictionary<string, string>> settings)
        {
            var list = new List<TComponent>();
            foreach (var name in componentNames)
            {
                IDictionary<string, string> componentSettings = null;
                if (settings != null && settings.ContainsKey(name))
                    componentSettings = settings[name];

                var component = resolver.GetInstance<TComponent>(name);
                component.Initialize(name, componentSettings);
                list.Add(component);
            }

            Components = list;
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
    }
}
