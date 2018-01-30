using System.Net;

namespace TestNinja.Mocking {
    public interface IFileDownloader {
        void DownloadFileToDestination(string url, string path);
    }

    public class FileDownloader : IFileDownloader {
        public void DownloadFileToDestination(string url, string path) {
            var client = new WebClient();
            client.DownloadFile(url, path);
        }
    }
}