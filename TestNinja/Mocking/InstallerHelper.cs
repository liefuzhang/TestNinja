using System.Net;

namespace TestNinja.Mocking {
    public class InstallerHelper {
        private string _setupDestinationFile;
        private readonly IFileDownloader _downloader;

        public InstallerHelper(IFileDownloader downloader = null) {
            _downloader = downloader ?? new FileDownloader();
        }
        public bool DownloadInstaller(string customerName, string installerName) {
            try {
                _downloader.DownloadFileToDestination(
                    $"http://example.com/{customerName}/{installerName}",
                    _setupDestinationFile);

                return true;
            } catch (WebException) {
                return false;
            }
        }
    }
}