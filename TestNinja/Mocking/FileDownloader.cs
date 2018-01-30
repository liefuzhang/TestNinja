using System.Net;

namespace TestNinja.Mocking {
    public interface IFileDownloader {
        void DownloadFileToDestination(string customerName, string installerName, string destinationFile);
    }

    public class FileDownloader : IFileDownloader {
        public void DownloadFileToDestination(string customerName, string installerName, string destinationFile) {
            var client = new WebClient();
            client.DownloadFile(
                string.Format("http://example.com/{0}/{1}",
                    customerName,
                    installerName),
                destinationFile);
        }
    }
}