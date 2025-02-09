﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace MobiFlight.Base
{
    public class Project
    {
        public string Name { get; set; }
        public string FilePath { get; set; }

        public List<IConfigFile> ConfigFiles { get; set; } = new List<IConfigFile>();

        public void OpenFile()
        {
            if (IsJson(FilePath))
            {
                var json = File.ReadAllText(FilePath);
                var project = JsonConvert.DeserializeObject<Project>(json);
                Name = project.Name;
                FilePath = project.FilePath;
                ConfigFiles = project.ConfigFiles;

                foreach (var configFile in ConfigFiles)
                {
                    if (!configFile.EmbedContent)
                    {
                        configFile.OpenFile();
                    }
                }
            }
            else if (IsXml(FilePath))
            {
                // Create a dummy project for old XML files
                var deprecatedConfigFile = ConfigFileFactory.CreateConfigFile(FilePath);
                deprecatedConfigFile.OpenFile();
                var configFile = new ConfigFile { FileName = FilePath, EmbedContent = true, ReferenceOnly = false, ConfigItems = deprecatedConfigFile.ConfigItems };
                Name = "MobiFlight Project";
                FilePath = FilePath;
                ConfigFiles.Add(configFile);
            }
            else
            {
                throw new InvalidDataException("Unsupported file format.");
            }
        }

        public void SaveFile()
        {
            foreach (var configFile in ConfigFiles)
            {
                if (!configFile.EmbedContent && !configFile.ReferenceOnly)
                {
                    configFile.SaveFile();
                }
            }

            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        private static bool IsJson(string filePath)
        {
            var firstChar = File.ReadAllText(filePath).TrimStart()[0];
            return firstChar == '{' || firstChar == '[';
        }

        private static bool IsXml(string filePath)
        {
            var firstFewChars = File.ReadAllText(filePath).TrimStart().Substring(0, 5);
            return firstFewChars.StartsWith("<?xml");
        }
    }
}