﻿using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace MobiFlight.Base
{
    public class ConfigFile : IConfigFile
    {
        public string FileName { get; set; }
        public bool ReferenceOnly { get; set; } = false;
        public bool EmbedContent { get; set; } = true;
        public List<IConfigItem> ConfigItems { get; set; } = new List<IConfigItem>();

        public ConfigFile() { }
        public ConfigFile(string FileName)
        {
            this.FileName = FileName;
        }

        public void OpenFile()
        {
            if (EmbedContent)
            {
                // Content is embedded, no need to load from file
                return;
            }

            var json = File.ReadAllText(FileName);
            var configFile = JsonConvert.DeserializeObject<ConfigFile>(json);
            FileName = configFile.FileName;
            ReferenceOnly = configFile.ReferenceOnly;
            EmbedContent = configFile.EmbedContent;
            ConfigItems = configFile.ConfigItems;
        }

        public void SaveFile()
        {
            if (EmbedContent || ReferenceOnly)
            {
                // Content is embedded or read-only, no need to save to file
                return;
            }

            var json = JsonConvert.SerializeObject(this, Formatting.Indented);
            File.WriteAllText(FileName, json);
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}