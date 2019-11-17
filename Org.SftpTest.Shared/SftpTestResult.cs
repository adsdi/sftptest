using System;

namespace Org.SftpTest.Shared {
    public class SftpTestResult {
        public string RunStatus { get; set; }
        public int FileCount { get; set; }
        public Exception Exception { get; set; }
        public string Report { get { return getReport(); } }
        private string crlf = Environment.NewLine;

        public SftpTestResult() {
            this.RunStatus = "Initial";
            this.FileCount = 0;
            this.Exception = null;
        }

        private string getReport() {
            if (this.RunStatus == "Failed") {
                return $"SftpTestResult{crlf}" +
                       $"  RunStatus: {this.RunStatus}{crlf}" +
                       $"  FileCount: {this.FileCount.ToString()}{crlf}" +
                       $"  Exception: {this.Exception.ToReport()}{crlf}";
            }

            return $"SftpTestResult{crlf}" +
                   $"  RunStatus: {this.RunStatus}{crlf}" +
                   $"  FileCount: {this.FileCount.ToString()}{crlf}";
        }
    }
}
