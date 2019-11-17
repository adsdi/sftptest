using KellermanSoftware.NetSFtpLibrary;
using Microsoft.Extensions.Logging;
using Org.SftpTest.Shared;
using System;
using System.Threading.Tasks;

namespace Org.SftpTest.Kellerman {
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
                    using (var client = getClient()) {
                        Log.LogInformation("Successfully created Kellerman SFTP client.");
                        client.Connect();
                        Log.LogInformation("Successfully connected.");
                        client.CurrentDirectory = @"/upload/get";
                        var files = client.GetDirectoryListing("*.txt", false);
                        client.Disconnect();
                        Log.LogInformation("Successfully closed Kellerman SFTP client.");
                        return files.Count;
                    }
                }
                catch (Exception ex) {
                    Log.LogInformation("Exception occurred in getPendingFilesCountAsync." + Environment.NewLine + ex.ToReport());
                    throw new Exception("An exception occurred while attempting to get the count of pending files.", ex);
                }
            });
        }

        private SFTP getClient() {
            try {
                return new SFTP(sftpParms.SftpKellermanUser, sftpParms.SftpKellermanLicense) { 
                    HostAddress = sftpParms.SftpUrl, 
                    UserName = sftpParms.SftpUserName, 
                    Password = sftpParms.SftpPassword                    
                };
            }
            catch (Exception ex) {
                throw new Exception("An exception occurred while attempting to create the SFTP client.", ex);
            }
        }
    }
}
