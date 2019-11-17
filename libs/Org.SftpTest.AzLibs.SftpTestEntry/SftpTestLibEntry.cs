using Microsoft.Extensions.Logging;
using Org.SftpTest.Shared;
using System;
using System.Threading.Tasks;

namespace Org.SftpTest.AzLibs.SftpTestEntry {
    public class SftpTestLibEntry : IDisposable {
        string crlf = Environment.NewLine;

        public async Task<SftpTestResult> Run(ILogger log, SftpParms sftpParms) {
            log.LogInformation($"Running Sftp Library '{sftpParms.SftpLibrary}'{crlf}  URL:{sftpParms.SftpUrl}{crlf}  HostFingerprint: {sftpParms.SftpHostFingerprint}{crlf}  " + 
                   $"UserName:{sftpParms.SftpUserName}{crlf}  Password:{sftpParms.SftpPassword}{crlf}  KellermanUser:{sftpParms.SftpKellermanUser}{crlf}  " +
                   $"KellermanLicense:{sftpParms.SftpKellermanLicense}{crlf}");

            if (sftpParms.SftpLibrary == "Kellerman") {
                var kellermanWrapper = new Org.SftpTest.Kellerman.LibWrapper();
                return await kellermanWrapper.Run(log, sftpParms);
            }
            else {
                var winScpWrapper = new Org.SftpTest.WinScp.LibWrapper();
                return await winScpWrapper.Run(log, sftpParms);
            }
        }

        public void Dispose() { }
    }
}
