using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.BatchDownloaderUC;

namespace UniversalDownloaderUnitTests
{
    [TestFixture]
    public class UtilitiesUnitTests
    {

        [Test]
        public void FunctionsUnitTests()
        {
            Assert.AreEqual(Functions.GetProtocol("ftp://192.168.8.100"), Enums.Protocol.Ftp);
            //Assert.AreEqual(Functions.GetProtocol("https://www.google.com"), Enums.Protocol.Http);

            Assert.AreEqual(Functions.ConvertSizeToUnit(1048576).Contains("MB"), true);

            Assert.Throws(typeof(Exception),
                delegate
                {
                    Functions.ConvertSizeToUnit(-1);
                });

            Assert.AreNotEqual(Functions.FormatTimeToString(10000), "");
            Assert.AreEqual(Functions.FormatTimeToString(0), "");

            Assert.Throws(typeof(Exception),
                delegate
                {
                    Functions.FormatTimeToString(-1);
                });

            string downloadsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),"Downloads");
            string fileName = "universaldownloader-filforutilitiessunnittest.txt";
            string filePath = Path.Combine(downloadsFolder, fileName);
            StreamWriter writer = File.CreateText(filePath);
            writer.Write("123123");
            writer.Flush();
            writer.Close();
            string newFileName = Functions.GetDistinguishedFileNameForSaving(fileName, downloadsFolder);
            Assert.AreNotEqual(newFileName,fileName);
            File.Delete(filePath);
        }
        [Test]
        public void ValidationUnitTests()
        {
            string url= "http://c.tadst.com/gfx/600w/doomsday-rule.jpg?1";
            string destination = "C:/Program Files";
            Assert.AreEqual(ValidationTests.IsUrlValid(url), true);
            Assert.AreEqual(ValidationTests.IsUrlValid("www.google.com"), false);
            Assert.AreEqual(ValidationTests.IsDestinationValid(destination), true);
            Assert.AreEqual(ValidationTests.IsDestinationValid("/Program Files"), false);
        }   

        

    }
}
