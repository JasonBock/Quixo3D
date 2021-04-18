using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Quixo3D.UI {

    /// <summary>
    /// Contains configuration data for the Quixo3D.UI application.
    /// </summary>
    public class ConfigSettings : ConfigurationSection {

        private const string ConfigSectionName = "quixo3dUiConfig";
        private const string CellSpacingKey = "cellSpacing";
        private const string EmptyCellOpacityKey = "emptyCellOpacity";
        private const string NonEmptyCellOpacityKey = "nonEmptyCellOpacity";
        private const string EnginesKey = "engines";
        private const string PluginFolderNameKey = "pluginFolderName";

        private static ConfigSettings instance;

        /// <summary>
        /// Gets the single instance of the <see cref="ConfigSettings"/> class.
        /// </summary>
        public static ConfigSettings Instance {
            get {
                if(instance == null) {
                    instance = (ConfigSettings)ConfigurationManager.GetSection(ConfigSectionName);
                }
                return instance;
            }
        }

        /// <summary>
        /// Gets or sets the spacing value used for the distance between Quixo game pieces.
        /// </summary>
        [ConfigurationProperty(CellSpacingKey, IsRequired=true)]
        public double CellSpacing
        {
            get { return (double)this[CellSpacingKey]; }
            set { this[CellSpacingKey] = value; }
        }

        /// <summary>
        /// Gets or sets the name of the folder that contains AI plugins.
        /// </summary>
        [ConfigurationProperty(PluginFolderNameKey, IsRequired = true)]
        public string PluginFolderName
        {
            get { return (string)this[PluginFolderNameKey]; }
            set { this[PluginFolderNameKey] = value; }
        }

        /// <summary>
        /// Gets the collection of elements containing AI engine plugin information.
        /// </summary>
        [ConfigurationProperty(EnginesKey, IsRequired=true)]
        public EngineConfigurationElementCollection Engines
        {
            get { return (EngineConfigurationElementCollection)this[EnginesKey]; }
        }

    }
}
