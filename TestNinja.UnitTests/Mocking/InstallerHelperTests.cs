using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking {
    [TestFixture]
    public class InstallerHelperTests {
        private InstallerHelper _installerHelper;
        private Mock<IFileDownloader> _downloader;

        [SetUp]
        public void Setup() {
            _downloader = new Mock<IFileDownloader>();
            _installerHelper = new InstallerHelper(_downloader.Object);
        }

        [Test]
        public void DownloadInstaller_DownloadFileSuceeds_ReturnTrue() {
            var result = _installerHelper.DownloadInstaller("customer", "installer");

            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void DownloadInstaller_DownloadFileThrowWebException_ReturnFalse() {
            _downloader.Setup(w => w.DownloadFileToDestination("customer", null)).Throws<WebException>();

            var result = _installerHelper.DownloadInstaller("customer", "installer");

            Assert.That(result, Is.EqualTo(false));
        }

    }
}
