using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Quixo3D.UI
{
    /// <summary>
    /// A configuration element that contains information about an AI Engine type.
    /// </summary>
    public class EngineConfigurationElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the unique key of a plugin type.
        /// </summary>
        [ConfigurationProperty("key")]
        public string Key
        {
            get { return (string)this["key"]; }
            set { this["key"] = value; }
        }

        /// <summary>
        /// Gets or sets the type name value of an AI plugin type.
        /// </summary>
        [ConfigurationProperty("value")]
        public string Value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }

    }
}
