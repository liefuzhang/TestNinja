using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace TestNinja.Mocking {
    public static class HousekeeperService {
        public static IUnitOfWork UnitOfWork { get; set; }
        public static IStatementGenerator StatementGenerator { get; set; }
        public static IEmailSender EmailSender { get; set; }
        public static IXtraMessageBox XtraMessageBox { get; set; }

        static HousekeeperService() {
            UnitOfWork = new UnitOfWork();
            StatementGenerator = new StatementGenerator();
            EmailSender = new EmailSender();
        }

        public static void SendStatementEmails(DateTime statementDate) {
            var housekeepers = UnitOfWork.Query<Housekeeper>();

            foreach (var housekeeper in housekeepers) {
                SendStatementEmail(statementDate, housekeeper);
            }
        }

        private static void SendStatementEmail(DateTime statementDate, Housekeeper housekeeper) {
            if (string.IsNullOrWhiteSpace(housekeeper.Email))
                return;

            var statementFilename = StatementGenerator.SaveStatement(housekeeper.Oid, housekeeper.FullName, statementDate);

            if (string.IsNullOrWhiteSpace(statementFilename))
                return;

            try {
                EmailSender.EmailFile(housekeeper.Email, housekeeper.StatementEmailBody, statementFilename,
                    string.Format("Sandpiper Statement {0:yyyy-MM} {1}", statementDate, housekeeper.FullName));
            } catch (Exception e) {
                XtraMessageBox.Show(e.Message, string.Format("Email failure: {0}", housekeeper.Email),
                    MessageBoxButtons.OK);
            }
        }
    }

    public enum MessageBoxButtons {
        OK
    }

    public interface IXtraMessageBox {
        void Show(string s, string housekeeperStatements, MessageBoxButtons button);
    }

    public class XtraMessageBox : IXtraMessageBox {
        public void Show(string s, string housekeeperStatements, MessageBoxButtons button) {
        }
    }

    public class MainForm {
        public bool HousekeeperStatementsSending { get; set; }
    }

    public class DateForm {
        public DateForm(string statementDate, object endOfLastMonth) {
        }

        public DateTime Date { get; set; }

        public DialogResult ShowDialog() {
            return DialogResult.Abort;
        }
    }

    public enum DialogResult {
        Abort,
        OK
    }

    public class SystemSettingsHelper {
        public static string EmailSmtpHost { get; set; }
        public static int EmailPort { get; set; }
        public static string EmailUsername { get; set; }
        public static string EmailPassword { get; set; }
        public static string EmailFromEmail { get; set; }
        public static string EmailFromName { get; set; }
    }

    public class Housekeeper {
        public string Email { get; set; }
        public int Oid { get; set; }
        public string FullName { get; set; }
        public string StatementEmailBody { get; set; }
    }

    public class HousekeeperStatementReport {
        public HousekeeperStatementReport(int housekeeperOid, DateTime statementDate) {
        }

        public bool HasData { get; set; }

        public void CreateDocument() {
        }

        public void ExportToPdf(string filename) {
        }
    }
}