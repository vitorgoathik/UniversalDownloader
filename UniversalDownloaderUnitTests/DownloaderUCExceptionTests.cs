using BatchDownloaderUC.Exceptions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalDownloaderUnitTests
{
    [TestFixture]
    public class DownloaderUCExceptionTests
    {
        [Test]
        public void MainTest()
        {
            DownloaderUCException exception = new DownloaderUCException("");
            Assert.Throws(typeof(DownloaderUCException),
                delegate
                {
                    DownloaderUCException.Throw(exception);

                });
            Assert.Throws(typeof(Exception),
                 delegate
                 {
                     DownloaderUCException.Throw(new Exception());

                 }); 
            Assert.Throws(typeof(DownloaderUCException),
                delegate
                {
                    DownloaderUCException.ThrowNewGeneral(exception);

                });
        }
    }
}
