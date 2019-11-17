using Microsoft.Extensions.Logging;
using Org.SftpTest.Shared;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WinSCP;

namespace Org.SftpTest.WinScp {
    public class LibWrapper : ILibWrapper {
        private SftpParms sftpParms;
        private ILogger Log;

        public async Task<SftpTestResult> Run(ILogger log, SftpParms parms) {
            Log = log;
            sftpParms = parms;
            var sftpTestResult = new SftpTestResult();
            try {
                sftpTestResult.FileCount = await getPendingFilesCountAsync();
                sftpTestResult.RunStatus = "Success";
                return sftpTestResult;
            }
            catch (Exception ex) {
                sftpTestResult.Exception = ex;
                sftpTestResult.RunStatus = "Failed";
                return sftpTestResult;
            }
        }

        private Task<int> getPendingFilesCountAsync() {
            return Task<int>.Factory.StartNew(() => {
                try {

                    var sessionOptions = new SessionOptions();
                    sessionOptions.ParseUrl("sftp://" + sftpParms.SftpUrl);
                    sessionOptions.FtpMode = FtpMode.Passive;
                    sessionOptions.Protocol = Protocol.Sftp;
                    sessionOptions.UserName = sftpParms.SftpUserName;
                    sessionOptions.Password = sftpParms.SftpPassword;
                    sessionOptions.SshHostKeyFingerprint = sftpParms.SftpHostFingerprint;
                    var winscpExePath = $@"{sftpParms.WinScpBinDirectory}\winscp.exe";

                    if (!File.Exists(winscpExePath))
                        throw new Exception($"WinScp executable is not found at path '{winscpExePath}'.");

                    using (Session session = new Session()) {
                        Log.LogInformation("Before opening WinScp session...");
                        session.ExecutablePath = winscpExePath;
                        session.Open(sessionOptions);
                        Log.LogInformation("WinScp session successfully opened.");
                        var dirList = session.ListDirectory(@"/upload/get");
                        int fileCount = dirList.Files.Where(f => f.Name.Contains(".txt")).Count();
                        session.Close();
                        Log.LogInformation("WinScp session closed.");
                        return fileCount;
                    }
                }
                catch (Exception ex) {
                    Log.LogInformation("Exception occurred in getPendingFilesCountAsync." + Environment.NewLine + ex.ToReport());
                    throw new Exception("An exception occurred while attempting to get the count of pending files.", ex);
                }
            });
        }
    }
}
