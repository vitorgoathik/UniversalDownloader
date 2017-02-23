using System;
using UniversalDownloader;
using NUnit.Framework;
using BatchDownloaderUC;
using System.Collections.Generic;
using System.Linq;

namespace UniversalDownloaderUnitTests
{
    [TestFixture]
    public class MainTests
    {
        Enums.UrlSplittingChar splittingChar = Enums.UrlSplittingChar.Semicolon;
        string urls = "http://c.tadst.com/gfx/600w/doomsday-rule.jpg?1";
        string destination = "C:/Program Files";
        int timeout = 1000;
        [Test]
        public void UriFormatTests()
        {

            //empty check
            Assert.That(string.IsNullOrEmpty(urls), Is.False, "Urls cannot be empty");
            //format check
            string invalidUrls = "";
            Assert.That(ValidationTests.AreAllUrlsValid(urls, splittingChar, out invalidUrls), Is.True, "error processing urls");
            Assert.AreEqual(invalidUrls, "", "Uri's in the wrong format: " + invalidUrls);
            Queue<string> urlsQueue = Functions.ConvertStringToQueueUrls(urls, splittingChar);
            Assert.Greater(urlsQueue.Count, 0, "Url queue is empty");
        }

        [Test]
        public void DestinationTests()
        {
            //empty folder path
            Assert.That(string.IsNullOrEmpty(destination), Is.False, "Files destination cannot be empty");
            //format check
            Assert.AreEqual(ValidationTests.IsDestinationValid(destination), true);
            Assert.Greater(Functions.GetTotalFreeSpace(destination), 0, "Error returning driver space");



        }

        [Test]
        public void DownloaderUCTests()
        {
            UriFormatTests();
            DestinationTests();
            Assert.GreaterOrEqual(timeout, 1000, "Timeout is too short");
            DownloaderUC downloader = new DownloaderUC(urls,splittingChar,destination,timeout);

            //size check
            Queue<string> urlsQueue = Functions.ConvertStringToQueueUrls(urls, splittingChar);

            Assert.Greater(Functions.CheckListFileTotalSize(urlsQueue.ToList()), 0, "Files size must be greater than 0 bytes");
            Assert.That(ValidationTests.IsDriverSpaceSufficient(destination, urlsQueue.ToList()), Is.True, "Insufficient memory disk space");

            //size and unit

        }


    }
}
