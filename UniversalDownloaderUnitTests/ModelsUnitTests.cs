using BatchDownloaderUC.Exceptions;
using BatchDownloaderUC.Models;
using BatchDownloaderUC.Controller;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Utilities.BatchDownloaderUC.Enums;
using Utilities.BatchDownloaderUC;

namespace UniversalDownloaderUnitTests
{
    [TestFixture]
    public class ModelsUnitTests
    {

        [Test]
        public void DestinationUnitTests()
        {
            Assert.DoesNotThrow(
                delegate 
                {
                    Destination destination = new Destination("C:/");
                    long space = destination.GetTotalFreeSpace();

                    Assert.GreaterOrEqual(space, 0);

                    destination = new Destination("C:");
                    space = destination.GetTotalFreeSpace();

                    Assert.GreaterOrEqual(space, -1);

                    destination = new Destination("Cfwef:/");
                    space = destination.GetTotalFreeSpace();

                    Assert.AreEqual(space, -1);
                }

                );
        }
        [Test]
        public void DownloadUnitTests()
        {
            Assert.DoesNotThrow(
                delegate
                {
                    Download download = new Download(new BatchDownloaderUC.Models.Destination(""), new RemoteFileInfo("", "", 100));
                    download.BytesReceived = 40;
                    int percent = download.PercentCompleted();
                    Assert.AreEqual(percent, 40);
                    download.BytesReceived = 0;
                });

            Assert.DoesNotThrow(
                delegate
                {
                    Download download = new Download(new BatchDownloaderUC.Models.Destination(""), new RemoteFileInfo("", "", 100));
                    download.BytesReceived = -1;
                    int percent = download.PercentCompleted();
                    Assert.Less(percent, 0);

                    download = new Download(new BatchDownloaderUC.Models.Destination(""), new RemoteFileInfo("", "", -20));
                    download.BytesReceived = 20;
                    percent = download.PercentCompleted();
                    Assert.Less(percent, 0);
                });


            Download downloadState = new Download(new BatchDownloaderUC.Models.Destination("C:/"), new RemoteFileInfo("", "anyname.txt", 100));



            Assert.DoesNotThrow(
                delegate
                {
                    downloadState.ChangeState(Utilities.BatchDownloaderUC.Enums.DownloadState.Started);
                    Assert.AreEqual(downloadState.DownloadState, Utilities.BatchDownloaderUC.Enums.DownloadState.Started);
                   
                    downloadState.ChangeState(Utilities.BatchDownloaderUC.Enums.DownloadState.Deleted);
                    Assert.AreEqual(downloadState.DownloadState, Utilities.BatchDownloaderUC.Enums.DownloadState.Deleted);

                    StreamWriter writer = File.CreateText("C:/ anyname.txt");
                    writer.Write("sdasdasdasdasdasd");
                    writer.Close();
                    downloadState.ChangeState(Utilities.BatchDownloaderUC.Enums.DownloadState.Error);
                    Assert.AreEqual(downloadState.DownloadState, Utilities.BatchDownloaderUC.Enums.DownloadState.Error);

                    downloadState = new Download(new Destination("C:/"), new RemoteFileInfo("", "anyname.txt", 123));
                    writer = File.CreateText("C:/anyname.txt");
                    writer.Write("sdasdasdasdasdasd");
                    writer.Close();
                    downloadState.ChangeState(Utilities.BatchDownloaderUC.Enums.DownloadState.Canceled);
                    Assert.AreEqual(downloadState.DownloadState, Utilities.BatchDownloaderUC.Enums.DownloadState.Canceled);
                    

                });
            Assert.Throws(typeof(IOException),
                delegate
                {
                    downloadState = new Download(new Destination("C:/"), new RemoteFileInfo("", "anyname.txt", 123));
                    StreamWriter writer = File.CreateText("C:/anyname.txt");
                    writer.Write("sdasdasdasdasdasd");
                    Assert.AreEqual(downloadState.DownloadState, Utilities.BatchDownloaderUC.Enums.DownloadState.Pending);
                    downloadState.ChangeState(Utilities.BatchDownloaderUC.Enums.DownloadState.Started);
                    downloadState.ChangeState(Utilities.BatchDownloaderUC.Enums.DownloadState.Canceled);
                    Assert.AreEqual(downloadState.DownloadState, Utilities.BatchDownloaderUC.Enums.DownloadState.Canceled);
                });


            downloadState = new Download(new Destination("C:/"), new RemoteFileInfo("", "anyname.txt", 100));
            downloadState.BytesReceived = 20;

            Assert.DoesNotThrow(
                delegate
                {
                    Assert.AreEqual(downloadState.GetRemainingTimeString(20).Trim(), "4 seconds");
                });
            downloadState.BytesReceived = 0;
            Assert.DoesNotThrow(
                delegate
                {
                    Assert.AreEqual(downloadState.GetRemainingTimeString(0), "");
                });
        }

    }
}
