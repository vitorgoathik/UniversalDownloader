using System;
using UniversalDownloaderAgoda;
using NUnit.Framework;

namespace UniversalDownloaderUnitTests
{
    [TestFixture]
    public class MainTests
    {
        Functions f = new Functions();
        ValidationTests validation = new ValidationTests();
        [Test]
        public void UriFormatTests()
        {
            f.urls = "http://www.file.com/file; ftp://other.file.com/other; sftp://and.also.this/ending";
            //empty check
            Assert.That(string.IsNullOrEmpty(f.urls), Is.False, "Urls cannot be empty");
            //format check
            string invalidUrls = validation.AreAllUrlsValid(f.urls);
            Assert.AreEqual(invalidUrls, "", "Uri's in the wrong format: "+ invalidUrls); 
        }

        [Test]
        public void DestinationTests()
        {
            f.destination = "C:/Program Files";
            //empty folder path
            Assert.That(string.IsNullOrEmpty(f.destination), Is.False, "Files destination cannot be empty");
            //format check
            Assert.AreEqual(validation.IsDestinationValid(f.destination), true);
            

        }

        [Test]
        public void SizeTest()
        {
            //size check
            //Assert.Greater(f.CheckFileSize(), 0, "File size must be greater than 0 bytes");
        }


    }
}
