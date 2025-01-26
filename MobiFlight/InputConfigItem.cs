﻿using MobiFlight.Base;
using MobiFlight.Config;
using MobiFlight.InputConfig;
using System.Collections.Generic;
using System;
using System.Xml;
using System.Xml.Serialization;

namespace MobiFlight
{
    public class InputConfigItem : ConfigItem, IXmlSerializable, ICloneable, IConfigRefConfigItem
    {
        // we initialize a cultureInfo object 
        // which is used for serialization
        // independently from current cultureInfo
        // @see: https://forge.simple-solutions.de/issues/275
        private System.Globalization.CultureInfo serializationCulture = new System.Globalization.CultureInfo("de");   

        public const String TYPE_NOTSET = "-";
        public const String TYPE_BUTTON = MobiFlightButton.TYPE;
        public const String TYPE_ENCODER = MobiFlightEncoder.TYPE;
        public const String TYPE_INPUT_SHIFT_REGISTER = MobiFlightInputShiftRegister.TYPE;
        public const String TYPE_INPUT_MULTIPLEXER = MobiFlightInputMultiplexer.TYPE;
        public const String TYPE_ANALOG = MobiFlightAnalogInput.TYPE;
        // only for backward compatibility during loading
        public const String TYPE_ANALOG_OLD = "Analog";

        public ButtonInputConfig button { get; set; }
        public EncoderInputConfig encoder { get; set; }
        public InputShiftRegisterConfig inputShiftRegister { get; set; }
        public InputMultiplexerConfig inputMultiplexer { get; set; }
        public AnalogInputConfig analog { get; set; }

        public string DeviceType { get; set; }
        public string DeviceName { get; set; }

        public InputConfigItem()
        {
            Preconditions = new PreconditionList();
            DeviceType = TYPE_NOTSET;

            ConfigRefs = new ConfigRefList();
        }

        public List<InputAction> GetInputActionsByType(System.Type type)
        {
            List<InputAction> result = new List<InputAction>();
            if (button != null)
            {
                result.AddRange(button.GetInputActionsByType(type));
            }

            if (encoder != null)
            {
                result.AddRange(encoder.GetInputActionsByType(type));
            }

            if (analog != null)
            {
                result.AddRange(analog.GetInputActionsByType(type));
            }

            if (inputShiftRegister != null)
            {
                result.AddRange(inputShiftRegister.GetInputActionsByType(type));
            }

            if (inputMultiplexer != null) {
                result.AddRange(inputMultiplexer.GetInputActionsByType(type));
            }
            return result;
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return (null);
        }

        public virtual void ReadXml(XmlReader reader)
        {
            ModuleSerial = reader["serial"];
            // This name is only present with input devices
            // and it is in the wrong place.
            DeviceName = reader["name"];
            if (reader["type"] != null && reader["type"] != "")
            {
                DeviceType = reader["type"];
                if (DeviceType == TYPE_ANALOG_OLD) DeviceType = TYPE_ANALOG;
            }

            reader.Read(); // this should be the button or encoder

            if (reader.LocalName == "button")
            {
                button = new ButtonInputConfig();
                button.ReadXml(reader);
            }

            if (reader.LocalName == "encoder")
            {
                encoder = new EncoderInputConfig();
                encoder.ReadXml(reader);
            }

            if (reader.LocalName == "inputShiftRegister")
            {
                inputShiftRegister = new InputShiftRegisterConfig();
                inputShiftRegister.ReadXml(reader);
            }

            if (reader.LocalName == "inputMultiplexer")
            {
                inputMultiplexer = new InputMultiplexerConfig();
                inputMultiplexer.ReadXml(reader);
            }

            if (reader.LocalName == "analog")
            {
                analog = new AnalogInputConfig();
                analog.ReadXml(reader);
            }

            // this is fallback, because type was not set in the past
            if (DeviceType == TYPE_NOTSET)
            {
                if (button != null)
                    DeviceType = TYPE_BUTTON;
                if (encoder != null)
                    DeviceType = TYPE_ENCODER;
            }

            /*
            if (reader.LocalName != "preconditions")            
                reader.Read(); // this should be the preconditions tag
            if (reader.LocalName != "preconditions")
                reader.Read(); // this should be the preconditions tag
            */
            if (reader.LocalName == "preconditions")
            {
                bool atPosition = false;
                // read precondition settings if present
                if (reader.ReadToDescendant("precondition"))
                {
                    // load a list
                    do
                    {
                        Precondition tmp = new Precondition();
                        tmp.ReadXml(reader);
                        Preconditions.Add(tmp);
                    } while (reader.LocalName == "precondition");
                }
                if (reader.NodeType != XmlNodeType.EndElement) 
                    reader.Read(); // this should be the corresponding "end" node

                if (reader.NodeType == XmlNodeType.EndElement)
                    reader.Read(); // move on to the next node
            }

            if (reader.LocalName == "configrefs")
            {
                bool atPosition = false;
                // read precondition settings if present
                if (reader.ReadToDescendant("configref"))
                {
                    // load a list
                    do
                    {
                        ConfigRef tmp = new ConfigRef();
                        tmp.ReadXml(reader);
                        ConfigRefs.Add(tmp);
                    } while (reader.LocalName == "configref");
                }

                reader.Read(); // advance to the next
            }
        }

        public virtual void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("serial", this.ModuleSerial);
            writer.WriteAttributeString("name", this.DeviceName);
            writer.WriteAttributeString("type", this.DeviceType);

            if (this.DeviceType == TYPE_BUTTON && button != null)
            {
                writer.WriteStartElement("button");
                button.WriteXml(writer);
                writer.WriteEndElement();
            }

            if (this.DeviceType == TYPE_ENCODER && encoder != null)
            {
                writer.WriteStartElement("encoder");
                encoder.WriteXml(writer);
                writer.WriteEndElement();
            }

            if (this.DeviceType == TYPE_INPUT_SHIFT_REGISTER && inputShiftRegister != null)
            {
                writer.WriteStartElement("inputShiftRegister");
                inputShiftRegister.WriteXml(writer);
                writer.WriteEndElement();
            }

            if (this.DeviceType == TYPE_INPUT_MULTIPLEXER && inputMultiplexer != null)
            {
                writer.WriteStartElement("inputMultiplexer");
                inputMultiplexer.WriteXml(writer);
                writer.WriteEndElement();
            }

            if (this.DeviceType == TYPE_ANALOG && analog != null)
            {
                writer.WriteStartElement("analog");
                analog.WriteXml(writer);
                writer.WriteEndElement();
            }

            writer.WriteStartElement("preconditions");
            foreach (Precondition p in Preconditions)
            {
                p.WriteXml(writer);
            }
            writer.WriteEndElement();

            writer.WriteStartElement("configrefs");
            foreach (ConfigRef p in ConfigRefs)
            {
                p.WriteXml(writer);
            }
            writer.WriteEndElement();
        }

        public object Clone()
        {
            InputConfigItem clone = new InputConfigItem();
            clone.ModuleSerial = ModuleSerial;
            clone.Name = DeviceName;
            clone.DeviceType = DeviceType;

            if (button != null)
                clone.button = (ButtonInputConfig)this.button.Clone();

            if (encoder != null)
                clone.encoder = (EncoderInputConfig)this.encoder.Clone();

            if (inputShiftRegister != null)
                clone.inputShiftRegister = (InputShiftRegisterConfig)this.inputShiftRegister.Clone();

            if (inputMultiplexer != null)
                clone.inputMultiplexer = (InputMultiplexerConfig)this.inputMultiplexer.Clone();

            if (analog != null)
                clone.analog = (AnalogInputConfig)this.analog.Clone();

            foreach (Precondition p in Preconditions)
            {
                clone.Preconditions.Add(p.Clone() as Precondition);
            }

            foreach (ConfigRef configRef in ConfigRefs)
            {
                clone.ConfigRefs.Add(configRef.Clone() as ConfigRef);
            }

            return clone;
        }

        internal void execute(
            CacheCollection cacheCollection,
            InputEventArgs e,
            List<ConfigRefValue> configRefs)
        {
            switch (DeviceType)
            {
                case TYPE_BUTTON:
                    if (button != null)                
                        button.execute(cacheCollection, e, configRefs);
                    break;
                case TYPE_ENCODER:
                    if (encoder != null)
                        encoder.execute(cacheCollection, e, configRefs);
                    break;

                case TYPE_INPUT_SHIFT_REGISTER:
                    if (inputShiftRegister != null)
                        inputShiftRegister.execute(cacheCollection, e, configRefs);
                    break;

                case TYPE_INPUT_MULTIPLEXER:
                    if (inputMultiplexer != null)
                        inputMultiplexer.execute(cacheCollection, e, configRefs);
                    break;

                case TYPE_ANALOG:
                    if (analog != null)
                        analog.execute(cacheCollection, e, configRefs);
                    break;
            }            
        }

        public Dictionary<String, int> GetStatistics()
        {
            Dictionary<String, int> result = new Dictionary<string, int>();

            if (DeviceType == TYPE_BUTTON)
            {
                // explicit test is needed 
                // in some older version we didn't save the node correctly
                if (button != null)
                    result = button?.GetStatistics();

            } else if (DeviceType == TYPE_ENCODER)
            {
                // explicit test is needed 
                // in some older version we didn't save the node correctly
                if (encoder != null)
                    result = encoder.GetStatistics();
            }
            else if (DeviceType == TYPE_ANALOG)
            {
                // explicit test is needed 
                // in some older version we didn't save the node correctly
                if (analog != null)
                    result = analog.GetStatistics();
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            bool areSame = (obj != null && obj is InputConfigItem) &&
                            ModuleSerial == (obj as InputConfigItem).ModuleSerial &&
                            DeviceName == (obj as InputConfigItem).DeviceName &&
                            DeviceType == (obj as InputConfigItem).DeviceType;

            if (areSame) {
                areSame = areSame && ((button == null && (obj as InputConfigItem).button == null) || (button != null && button.Equals((obj as InputConfigItem).button)));
                areSame = areSame && ((encoder == null && (obj as InputConfigItem).encoder == null) || (encoder != null && encoder.Equals((obj as InputConfigItem).encoder)));
                areSame = areSame && ((analog == null && (obj as InputConfigItem).analog == null) || (analog != null && analog.Equals((obj as InputConfigItem).analog)));
                areSame = areSame && ((inputShiftRegister == null && (obj as InputConfigItem).inputShiftRegister == null) || (inputShiftRegister != null && inputShiftRegister.Equals((obj as InputConfigItem).inputShiftRegister)));
                areSame = areSame && ((inputMultiplexer == null && (obj as InputConfigItem).inputMultiplexer == null) || (inputMultiplexer != null && inputMultiplexer.Equals((obj as InputConfigItem).inputMultiplexer)));

                areSame = areSame && 
                            Preconditions.Equals((obj as InputConfigItem).Preconditions) &&
                            ConfigRefs.Equals((obj as InputConfigItem).ConfigRefs);
            }

            return areSame;
        }

        protected override IDeviceConfig GetDeviceConfig()
        {
            switch (DeviceType)
            {
                case TYPE_BUTTON:
                    return button;
                case TYPE_ENCODER:
                    return encoder;
                case TYPE_INPUT_SHIFT_REGISTER:
                    return inputShiftRegister;
                case TYPE_INPUT_MULTIPLEXER:
                    return inputMultiplexer;
                case TYPE_ANALOG:
                    return analog;
                default:
                    return null;
            }
        }
    }
}
