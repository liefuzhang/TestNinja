﻿using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking {
    [TestFixture]
    public class VideoServiceTests {
        private Mock<IFileReader> _fileReader;
        private Mock<IVideoRepository> _videoRepo;
        private VideoService _videoService;

        [SetUp]
        public void Setup() {
            _fileReader = new Mock<IFileReader>();
            _videoRepo = new Mock<IVideoRepository>();
            _videoService = new VideoService(_fileReader.Object, _videoRepo.Object);
        }

        [Test]
        public void ReadVideoTitle_EmptyFile_ReturnError() {
            _fileReader.Setup(fr => fr.Read("video.txt")).Returns("");

            var result = _videoService.ReadVideoTitle();


            Assert.That(result, Does.Contain("error").IgnoreCase);
        }

        [Test]
        public void GetUnprocessedVideosAsCsv_AllVideosAreProcessed_ReturnEmptyString() {
            _videoRepo.Setup(r => r.GetUnprocessedVideos()).Returns(new List<Video>());

            var result = _videoService.GetUnprocessedVideosAsCsv();

            Assert.That(result, Is.EqualTo(""));
        }

        [Test]
        public void GetUnprocessedVideosAsCsv_AFewUnprocessedVideos_ReturnAStringWithIdOfUnprocessedVi() {
            _videoRepo.Setup(r => r.GetUnprocessedVideos()).Returns(new List<Video> {
                new Video { Id = 1 },
                new Video { Id = 2 },
                new Video { Id = 3 }
            });

            var result = _videoService.GetUnprocessedVideosAsCsv();

            Assert.That(result, Is.EqualTo("1,2,3"));
        }

    }
}
