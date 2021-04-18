using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Quixo3D.UI
{
    /// <summary>
    /// A configuration class used to hold configuration elements for AI engine plugins.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface")]
    public class EngineConfigurationElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new EngineConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EngineConfigurationElement)element).Key;
        }

        /// <summary>
        /// Gets an <see cref="EngineConfigurationElement"/> object with the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        new public EngineConfigurationElement this[string key]
        {
            get { return (EngineConfigurationElement)BaseGet(key); }
        }

    }
}
