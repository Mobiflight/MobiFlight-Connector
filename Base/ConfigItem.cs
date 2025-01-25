﻿using MobiFlight.Modifier;

namespace MobiFlight.Base
{
    public interface IConfigItem
    {
        string GUID { get; set; }
        bool Active { get; set; }
        string Type { get; }
        string Name { get; set; }
        string ModuleSerial { get; set; }
        PreconditionList Preconditions { get; set; }
        ModifierList Modifiers { get; set; }
        ConfigRefList ConfigRefs { get; set; }
        string RawValue { get; set; }
        string Value { get; set; }
        IDeviceConfig Device { get; }
    }

    public abstract class ConfigItem : IConfigItem
    {
        public string GUID { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; }
        public string Type { get { return GetConfigItemType(); } }
        public string ModuleSerial { get; set; }
        public PreconditionList Preconditions { get; set; }
        public ModifierList Modifiers { get; set; }
        public ConfigRefList ConfigRefs { get; set; }
        public string RawValue { get; set; }
        public string Value { get; set; }
        
        public IDeviceConfig Device { get { return GetDeviceConfig(); } }

        protected abstract IDeviceConfig GetDeviceConfig();

        protected virtual string GetConfigItemType()
        {
            return this.GetType().ToString();
        }
    }
}
