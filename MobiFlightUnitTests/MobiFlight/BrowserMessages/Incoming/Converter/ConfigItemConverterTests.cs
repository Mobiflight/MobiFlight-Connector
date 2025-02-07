﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using MobiFlight.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MobiFlight.BrowserMessages.Incoming.Converter.Tests
{
    [TestClass()]
    public class ConfigItemConverterTests
    {
        [TestMethod]
        public void CanConvertTest()
        {
            var converter = new ConfigItemConverter();
            Assert.IsTrue(converter.CanConvert(typeof(InputConfigItem)));
            Assert.IsTrue(converter.CanConvert(typeof(OutputConfigItem)));
            Assert.IsFalse(converter.CanConvert(typeof(string)));
        }

        [TestMethod]
        public void ReadJson_InputConfigItem_DeserializesCorrectly()
        {
            var json = "{\"Type\":\"MobiFlight.InputConfigItem\",\"Name\":\"SomeValue\"}";
            var result = JsonConvert.DeserializeObject<ConfigItem>(json);

            Assert.IsInstanceOfType(result, typeof(InputConfigItem));
            Assert.AreEqual("SomeValue", ((InputConfigItem)result).Name);
        }

        [TestMethod]
        public void ReadJson_OutputConfigItem_DeserializesCorrectly()
        {
            var json = "{\"Type\":\"MobiFlight.OutputConfigItem\",\"Name\":\"SomeValue\"}";
            var result = JsonConvert.DeserializeObject<ConfigItem>(json);

            Assert.IsInstanceOfType(result, typeof(OutputConfigItem));
            Assert.AreEqual("SomeValue", ((OutputConfigItem)result).Name);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ReadJson_UnsupportedType_ThrowsNotSupportedException()
        {
            var json = "{\"Type\":\"MobiFlight.UnsupportedConfigItem\"}";
            JsonConvert.DeserializeObject<ConfigItem>(json);
        }

        [TestMethod]
        public void WriteJson_InputConfigItem_SerializesCorrectly()
        {
            var item = new InputConfigItem { Name = "SomeValue" };
            var json = JsonConvert.SerializeObject(item);

            StringAssert.Contains(json, "\"Type\":\"MobiFlight.InputConfigItem\"");
            StringAssert.Contains(json, "\"Name\":\"SomeValue\"");
        }

        [TestMethod]
        public void WriteJson_OutputConfigItem_SerializesCorrectly()
        {
            var item = new OutputConfigItem { Name = "SomeValue" };
            var json = JsonConvert.SerializeObject(item);

            StringAssert.Contains(json, "\"Type\":\"MobiFlight.OutputConfigItem\"");
            StringAssert.Contains(json, "\"Name\":\"SomeValue\"");
        }
    }
}