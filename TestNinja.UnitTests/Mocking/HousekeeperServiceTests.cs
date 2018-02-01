using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework.Internal;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking {

    [TestFixture]
    public class HousekeeperServiceTests {
        private Mock<IUnitOfWork> _unitOfWork;
        private Mock<IStatementGenerator> _statementGenerator;
        private Mock<IEmailSender> _emailSender;
        private Mock<IXtraMessageBox> _messageBox;
        private DateTime _statementDate;
        private Housekeeper _housekeeper;
        private string _statementFilename;

        [SetUp]
        public void Setup() {
            _housekeeper = new Housekeeper { Email = "a", FullName = "b", Oid = 1, StatementEmailBody = "c" };
            _unitOfWork = new Mock<IUnitOfWork>();
            _statementGenerator = new Mock<IStatementGenerator>();
            _emailSender = new Mock<IEmailSender>();
            _messageBox = new Mock<IXtraMessageBox>();
            _statementDate = new DateTime(2018, 1, 2);
            _statementFilename = "fileName";

            HousekeeperService.UnitOfWork = _unitOfWork.Object;
            HousekeeperService.StatementGenerator = _statementGenerator.Object;
            HousekeeperService.EmailSender = _emailSender.Object;
            HousekeeperService.XtraMessageBox = _messageBox.Object;

            _unitOfWork.Setup(uow => uow.Query<Housekeeper>()).Returns(new List<Housekeeper> {
                _housekeeper
            }.AsQueryable());

            _statementGenerator
                .Setup(sg => sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate))
                // Use lamda to allow lazy evaluation, as we are overriding string value _statementFilename in some tests
                .Returns(() => _statementFilename);
        }

        [Test]
        public void SendStatementEmails_WhenCalled_GenerateStatement() {
            HousekeeperService.SendStatementEmails(_statementDate);

            _statementGenerator.Verify(sg => sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate));
        }


        [Test]
        public void SendStatementEmails_WhenCalled_EmailTheStatement() {

            HousekeeperService.SendStatementEmails(_statementDate);

            _emailSender.Verify(es => es.EmailFile(
                _housekeeper.Email,
                _housekeeper.StatementEmailBody,
                _statementFilename,
                It.IsAny<string>()));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void SendStatementEmails_HousekeeperEmailIsNullOrWhiteSpace_ShouldNotGenerateStatementOrSendEmail(string email) {
            _housekeeper.Email = email;

            HousekeeperService.SendStatementEmails(_statementDate);

            _statementGenerator.Verify(sg => sg.SaveStatement(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Never);
            _emailSender.Verify(es => es.EmailFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void SendStatementEmails_StatementFileNameIsNullOrWhiteSpace_ShouldNotSendEmail(string statementFileName) {
            _statementFilename = statementFileName;

            HousekeeperService.SendStatementEmails(_statementDate);

            _emailSender.Verify(es => es.EmailFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void SendStatementEmails_EmailFileFails_ShowMessageBox() {
            _emailSender.Setup(es => es.EmailFile(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()))
                .Throws<Exception>();

            HousekeeperService.SendStatementEmails(_statementDate);

            _messageBox.Verify(xmb => xmb.Show(It.IsAny<string>(), It.IsAny<string>(), MessageBoxButtons.OK));
        }
    }
}