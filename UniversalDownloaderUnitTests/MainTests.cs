﻿using System;
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
        //string urls = "http://c.tadst.com/gfx/600w/doomsday-rule.jpg?1";
        //string destination = "C:/Program Files";
        //int timeout = 1000;
        //[Test]
        //public void UriFormatTests()
        //{
             
        //    //format check
        //    string invalidUrls = "";
        //    //Assert.AreEqual(invalidUrls, "", "Uri's in the wrong format: " + invalidUrls);
        //    //Assert.Greater(Functions.ConvertStringToQueueUrls(urls, Delimiter).Count,0);
            
        //    //Assert.Throws(typeof(Exception),
        //    //    delegate 
        //    //    {
        //    //        Queue<string> urlsQueueFail = Functions.ConvertStringToQueueUrls("www.wrongformat,com", Delimiter);

        //    //    }); 
        //}

        //[Test]
        //public void DestinationTests()
        //{

        //    Assert.DoesNotThrow(
        //        delegate
        //        {

        //            Assert.AreEqual(ValidationTests.IsDestinationValid(destination), true);
        //            Assert.AreNotEqual(ValidationTests.IsDestinationValid("/Program Files"), false);
        //            Assert.Greater(Functions.GetTotalFreeSpace(destination), 0, "Error returning driver space");
        //            List<string> urlsList = Functions.ConvertStringToQueueUrls(urls, Delimiter).ToList();
        //            long size = Functions.CheckListFileTotalSize(urlsList);
        //            Assert.Greater(size, 0, "Error CheckListFileTotalSize");
        //            Assert.Equals(Functions.ConvertSizeToUnit(size).Contains("MB"),true);
        //            string downloads = Functions.GetDownloadsFolder();
        //        });
        //    Assert.Throws(typeof(Exception),
        //        delegate
        //        {
        //            Functions.CheckListFileTotalSize(new List<string>() { "www.google.com" });

        //        });
            
        //}

        //[Test]
        //public void DownloaderUCTests()
        //{
        //    UriFormatTests();
        //    DestinationTests();
        //    Assert.GreaterOrEqual(timeout, 1000, "Timeout is too short");
        //    DownloaderUC downloader = new DownloaderUC(urls,Delimiter,destination,timeout);

        //    //size check
        //    Queue<string> urlsQueue = Functions.ConvertStringToQueueUrls(urls, Delimiter);

        //    Assert.Greater(Functions.CheckListFileTotalSize(urlsQueue.ToList()), 0, "Files size must be greater than 0 bytes");
        //    Assert.That(ValidationTests.IsDriverSpaceSufficient(destination, urlsQueue.ToList()), Is.True, "Insufficient memory disk space");

        //    //size and unit

        //}


    }
}
