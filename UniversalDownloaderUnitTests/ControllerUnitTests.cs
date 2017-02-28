using BatchDownloaderUC.Controller;
using BatchDownloaderUC.Exceptions;
using BatchDownloaderUC.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Utilities.BatchDownloaderUC.Enums;

namespace UniversalDownloaderUnitTests
{
    /// <summary>
    /// This testing is half way to integration testing, since the controller is directly related to the Download model
    /// </summary>
    [TestFixture]
    public class ControllerUnitTests
    {
        DownloadsController downloadController;
        Destination destination = new Destination("C:/");
        Download download1;
        Download download2;
        Download download3;
        Download download4;
        Download download5;

        public void ValidationUnitTests()
        {
            downloadController = new DownloadsController();
            Assert.DoesNotThrow(
                delegate
                {
                    Assert.AreEqual(downloadController.CurrentDownload, null);
                });

            try
            {//invalid fields
                download1 = new Download(new Destination("edwe"), new RemoteFileInfo("werfwerf", "asdasd", 234));
                downloadController.AddDownloadToList(download1);
            }
            catch (DownloaderUCException e)
            {
                Assert.AreEqual(e.Error, ErrorType.InvalidField);
            }

            //insufficient space exception
            //CheckSpaceToAddDownload

            try
            {
                download1 = new Download(destination, new RemoteFileInfo("https://www.google.com", "anyname.exe"
                   , long.MaxValue));
                downloadController.AddDownloadToList(download1);
            }
            catch (DownloaderUCException e)
            {
                Assert.AreEqual(e.Error, ErrorType.InsufficientDiskSpaceFor);
            }

        }
        /// <summary>
        /// These our 
        /// </summary>
        [Test]
        public void DownloadsControllerUnitTests()
        {
            //DATA testing

            downloadController = new DownloadsController();
            

            //testing the remaining bytes. will add 3 downloads, and set one to started and receive 10 bytes on it.
            download1 = new Download(destination, new RemoteFileInfo("https://www.google.com", "anyname.exe", 50));
            download2 = new Download(destination, new RemoteFileInfo("https://www.google.com", "anyname.exe", 500));
            download3 = new Download(destination, new RemoteFileInfo("https://www.google.com", "anyname.exe", 100));

            downloadController.AddDownloadToList(download1);
            downloadController.AddDownloadToList(download2);
            downloadController.AddDownloadToList(download3);

            Assert.AreEqual(downloadController.DownloadsCollection.Count, 3);

            download2.BytesReceived = 10;
            download2.ChangeState(DownloadState.Started);

            Assert.AreEqual(downloadController.TotalSizeBytesRemainingToDownload(), 640);


            downloadController = new DownloadsController();

            //testing TotalSizeBytes
            download1 = new Download(destination, new RemoteFileInfo("https://www.google.com", "anyname.exe", 100));
            download2 = new Download(destination, new RemoteFileInfo("https://www.google.com", "anyname.exe", 2000));
            download3 = new Download(destination, new RemoteFileInfo("https://www.google.com", "anyname.exe", 1500));
            download4 = new Download(destination, new RemoteFileInfo("https://www.google.com", "anyname.exe", 5000));

            downloadController.AddDownloadToList(download1);
            downloadController.AddDownloadToList(download2);
            downloadController.AddDownloadToList(download3);
            downloadController.AddDownloadToList(download4);

            Assert.AreEqual(downloadController.TotalSizeBytes(), 8600);
            
            download1.ChangeState(DownloadState.Closed);
            download2.ChangeState(DownloadState.Deleted);
            download3.BytesReceived = 1500;
            download3.ChangeState(DownloadState.Completed);
            download4.ChangeState(DownloadState.Started);
            download4.BytesReceived = 300;

            Assert.AreEqual(downloadController.TotalSizeBytes(), 6500);

            //testing percent completed

            downloadController = new DownloadsController();

            download1 = new Download(destination, new RemoteFileInfo("https://www.google.com", "anyname.exe", 200));
            download2 = new Download(destination, new RemoteFileInfo("https://www.google.com", "anyname.exe", 200));
            download3 = new Download(destination, new RemoteFileInfo("https://www.google.com", "anyname.exe", 200));
            download4 = new Download(destination, new RemoteFileInfo("https://www.google.com", "anyname.exe", 200));
            download5 = new Download(destination, new RemoteFileInfo("https://www.google.com", "anyname.exe", 200));

            downloadController.AddDownloadToList(download1); //will be completed
            downloadController.AddDownloadToList(download2); //will be have progress, but then canceled
            downloadController.AddDownloadToList(download3); //will be deleted
            downloadController.AddDownloadToList(download4); //suffer an error
            downloadController.AddDownloadToList(download5); // half way to go

            
            download1.ChangeState(DownloadState.Completed);

            download2.ChangeState(DownloadState.Started);
            download2.BytesReceived = 100;
            download2.ChangeState(DownloadState.Canceled);

            download3.ChangeState(DownloadState.Deleted);
            download4.ChangeState(DownloadState.Error, false);

            download5.BytesReceived = 100;

            Assert.AreEqual(downloadController.PercentCompleted(), 50);
            
            Assert.AreNotEqual(downloadController.GetRemainingTimeString(10), "");
            Assert.AreEqual(downloadController.GetRemainingTimeString(201), "");

            Assert.Throws(typeof(ArgumentOutOfRangeException),
                delegate
                {
                    downloadController.CancelDownloads(new List<int>() { 6, 7, 8 });
                });
            downloadController.CancelDownloads(new List<int>() { 0, 1, 2, 3, 4 });
            var deleted = downloadController.DownloadsCollection.Where(o => o.DownloadState == DownloadState.Deleted).Count();
            Assert.AreEqual(deleted, 2);

            downloadController.ClearDownloads();
            var closedCount = downloadController.DownloadsCollection.Where(o => o.DownloadState == DownloadState.Closed).Count();
            Assert.AreEqual(closedCount, 1);


        }
    }
}
