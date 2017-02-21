using System;
using UniversalDownloaderAgoda;
using NUnit.Framework;

namespace UniversalDownloaderUnitTests
{
    [TestFixture]
    public class MainTests
    {
        Functions f = new Functions();
        [Test]
        public void UriFormatTests()
        {
            //empty check
            Assert.That(string.IsNullOrEmpty(f.uri), Is.False, "File string must not be null or empty");
            //format check
            Assert.AreEqual(f.IsUriValid(), true, "Uri's in the wrong format");
        }

        [Test]
        public void DestinationTests()
        {
            //format check
            Assert.AreEqual(f.IsDestinationValid(), true);
            //empty file name test
            Assert.That(string.IsNullOrEmpty(f.GetFileName()), Is.False, "File string must not be null or empty");

        }

        [Test]
        public void SizeTest()
        {
            //size check
            Assert.Greater(f.CheckFileSize(), 0, "File size must be greater than 0 bytes");
        }


    }
}
