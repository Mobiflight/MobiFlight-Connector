﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text.RegularExpressions;

namespace MobiFlight.Tests
{
    [TestClass()]
    public class ConfigFileTests
    {
        [TestMethod()]
        public void ConfigFileTest()
        {

            ConfigFile o = new ConfigFile(@"assets\Base\ConfigFile\OpenConfigFile.xml");
            Assert.IsNotNull(o, "Object is null");
        }

        [TestMethod()]
        //[Ignore]
        public void OpenFileTest()
        {
            String inFile = @"assets\Base\ConfigFile\OpenFileTest.xml";
            String expFile = @"assets\Base\ConfigFile\OpenFileTest.xml.exp";
            String inFileTemp = @"assets\Base\ConfigFile\temp_OpenFileTest.xml";

            ConfigFile o = new ConfigFile(inFile);
            o.OpenFile();

            ConfigFile oTemp = new ConfigFile(inFileTemp);
            oTemp.OutputConfigItems = o.GetOutputConfigItems();
            oTemp.InputConfigItems = o.GetInputConfigItems();
            oTemp.SaveFile();

            String s1 = System.IO.File.ReadAllText(expFile);
            String s2 = System.IO.File.ReadAllText(inFileTemp);

            // get rid of the individual version number
            // that varies every time we increment the MobiFlight version
            s1 = replaceTestIrrelevantXmlInformation(s1);
            s2 = replaceTestIrrelevantXmlInformation(s2);

            Assert.AreEqual(s1, s2, inFile + ": Files are not the same");
            System.IO.File.Delete(inFileTemp);


            inFile = @"assets\Base\ConfigFile\OpenFileTest.2912.xml";
            expFile = @"assets\Base\ConfigFile\OpenFileTest.2912.xml.exp";
            inFileTemp = @"assets\Base\ConfigFile\temp_OpenFileTest.2912.xml";

            o = new ConfigFile(inFile);
            o.OpenFile();

            oTemp = new ConfigFile(inFileTemp);
            oTemp.OutputConfigItems = o.GetOutputConfigItems();
            oTemp.InputConfigItems = o.GetInputConfigItems();
            oTemp.SaveFile();

            s1 = System.IO.File.ReadAllText(expFile);
            s2 = System.IO.File.ReadAllText(inFileTemp);

            // get rid of the individual version number
            // that varies every time we increment the MobiFlight version
            s1 = replaceTestIrrelevantXmlInformation(s1);
            s2 = replaceTestIrrelevantXmlInformation(s2);

            Assert.AreEqual(s1, s2, inFile + ": Files are not the same");
            System.IO.File.Delete(inFileTemp);

            inFile = @"assets\Base\ConfigFile\7.5.0-7.5.1-upgrade.mcc";
            inFileTemp = @"assets\Base\ConfigFile\7.5.0-7.5.1-upgrade.mcc.tmp";
            expFile = @"assets\Base\ConfigFile\7.5.0-7.5.1-upgrade.mcc.exp";

            o = new ConfigFile(inFile);
            o.OpenFile();

            oTemp = new ConfigFile(inFileTemp);
            oTemp.OutputConfigItems = o.GetOutputConfigItems();
            oTemp.InputConfigItems = o.GetInputConfigItems();
            oTemp.SaveFile();

            s1 = System.IO.File.ReadAllText(expFile);
            s2 = System.IO.File.ReadAllText(inFileTemp);

            // get rid of the individual version number
            // that varies every time we increment the MobiFlight version
            s1 = replaceTestIrrelevantXmlInformation(s1);
            s2 = replaceTestIrrelevantXmlInformation(s2);

            Assert.AreEqual(s1, s2, inFile + ": Files are not the same");
            System.IO.File.Delete(inFileTemp);

            // Load the new version was problematic\
            inFile = @"assets\Base\ConfigFile\7.5.0-7.5.1-upgrade.mcc.exp";
            inFileTemp = @"assets\Base\ConfigFile\7.5.0-7.5.1-upgrade.mcc.tmp";
            expFile = @"assets\Base\ConfigFile\7.5.0-7.5.1-upgrade.mcc.exp";

            o = new ConfigFile(inFile);
            o.OpenFile();

            oTemp = new ConfigFile(inFileTemp);
            oTemp.OutputConfigItems = o.GetOutputConfigItems();
            oTemp.InputConfigItems = o.GetInputConfigItems();
            oTemp.SaveFile();

            s1 = System.IO.File.ReadAllText(expFile);
            s2 = System.IO.File.ReadAllText(inFileTemp);

            // get rid of the individual version number
            // that varies every time we increment the MobiFlight version
            s1 = replaceTestIrrelevantXmlInformation(s1);
            s2 = replaceTestIrrelevantXmlInformation(s2);

            Assert.AreEqual(s1, s2, inFile + ": Files are not the same");
            System.IO.File.Delete(inFileTemp);

            // the files are partially from this project
            // and also the example files are copied over
            // from the examples folder (Main project)
            foreach (string file in System.IO.Directory.GetFiles(@"assets\Base\ConfigFile\", "*.mcc"))
            {
                inFile = file;
                expFile = inFile;
                if (System.IO.File.Exists(inFile + ".exp")) expFile = inFile + ".exp";
                inFileTemp = inFile + ".tmp";

                o = new ConfigFile(inFile);
                o.OpenFile();

                oTemp = new ConfigFile(inFileTemp);
                oTemp.OutputConfigItems = o.GetOutputConfigItems();
                oTemp.InputConfigItems = o.GetInputConfigItems();
                oTemp.SaveFile();

                s1 = System.IO.File.ReadAllText(expFile);
                s2 = System.IO.File.ReadAllText(inFileTemp);

                // get rid of the individual version number
                // that varies every time we increment the MobiFlight version
                s1 = replaceTestIrrelevantXmlInformation(s1);
                s2 = replaceTestIrrelevantXmlInformation(s2);

                Assert.AreEqual(s1, s2, inFile + ": Files are not the same");
                System.IO.File.Delete(inFileTemp);
            }

            // load a broken config
            inFile = @"assets\Base\ConfigFile\OpenFileBrokenTest.xml";
            inFileTemp = @"assets\Base\ConfigFile\OpenFileBrokenTest.xml.tmp";

            bool exceptionThrown = false;

            try
            {
                s1 = "a";
                s2 = "b";
                o = new ConfigFile(inFile);
                o.OpenFile();

                oTemp = new ConfigFile(inFileTemp);
                oTemp.OutputConfigItems = o.GetOutputConfigItems();
                oTemp.InputConfigItems = o.GetInputConfigItems();
                oTemp.SaveFile();

                s1 = System.IO.File.ReadAllText(inFile);
                s2 = System.IO.File.ReadAllText(inFileTemp);

                // get rid of the individual version number
                // that varies every time we increment the MobiFlight version
                s1 = replaceTestIrrelevantXmlInformation(s1);
                s2 = replaceTestIrrelevantXmlInformation(s2);
            }
            catch (Exception)
            {
                exceptionThrown = true;
            }
            Assert.IsTrue(exceptionThrown);
        }

        [TestMethod()]
        public void SaveFileTest()
        {
            // implicitly tested in OpenFileTest();
            Assert.IsTrue(true);
        }

        [TestMethod()]
        public void getInputConfigTest()
        {
            String inFile = @"assets\Base\ConfigFile\OpenFileTest.xml";
            ConfigFile o = new ConfigFile(inFile);
            o.OpenFile();
            var xr = o.GetInputConfigItems();

            Assert.IsNotNull(xr);
        }

        [TestMethod()]
        public void getOutputConfigTest()
        {
            String inFile = @"assets\Base\ConfigFile\OpenFileTest.xml";
            ConfigFile o = new ConfigFile(inFile);
            o.OpenFile();
            var xr = o.GetOutputConfigItems();

            Assert.IsNotNull(xr);
        }

        string replaceTestIrrelevantXmlInformation(string s1)
        {
            s1 = Regex.Replace(s1, @"Version=[^,]+", "Version=1.0.0.0");
            s1 = s1.Replace(" msdata:InstanceType=\"MobiFlight.OutputConfigItem, MFConnector, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\"", "");
            s1 = s1.Replace(" msdata:InstanceType=\"MobiFlight.InputConfigItem, MFConnector, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\"", "");
            s1 = s1.Replace(" xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\"", "");
            s1 = s1.Replace("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>", "<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            return s1;
        }
    }
        
}