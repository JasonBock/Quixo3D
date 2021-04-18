using System;
using System.Configuration;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Quixo3D.Engine;
using System.IO;

namespace Quixo3D.UI
{
    /// <summary>
    /// A collection class that holds the types of AI engines available in the game.
    /// </summary>
    public class PluginCollection : ObservableCollection<Type>
    {
        /// <summary>
        /// Creates a new instance of the class.
        /// </summary>
        public PluginCollection()
        {
           
                string currentPath = Assembly.GetExecutingAssembly().Location;
                FileInfo assemblyFile = new FileInfo(currentPath);
                DirectoryInfo assemblyDirectory = assemblyFile.Directory;

                //load plugins from the executing location
                LoadTypesFromFolder(assemblyDirectory);
            
            
        }

        void LoadTypesFromFolder(DirectoryInfo folder)
        {
            string filter = "*.dll";
            FileInfo[] files = folder.GetFiles(filter);

            foreach(FileInfo file in files)
            {
                Assembly assembly = Assembly.LoadFile(file.FullName);
                try
                {
                    Type[] types = assembly.GetTypes();
                    foreach(Type type in types)
                    {
                        if(type != typeof(IEngine) && typeof(IEngine).IsAssignableFrom(type))
                        {
                            base.Add(type);
                        }
                    }
                }
                catch(ReflectionTypeLoadException reflectionTypeLoadException)
                {
                    System.Diagnostics.Debug.WriteLine(reflectionTypeLoadException.Message + ".  Error loading types from assembly '" + assembly.FullName + "'");
                }
            }

            //Load plugins from any subfolders named "Plugins"
            DirectoryInfo[] directories = folder.GetDirectories(ConfigSettings.Instance.PluginFolderName);
            foreach(DirectoryInfo directory in directories)
            {
                LoadTypesFromFolder(directory);
            }

        }
    }
}
