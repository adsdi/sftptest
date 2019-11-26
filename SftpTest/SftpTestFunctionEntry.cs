using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Org.SftpTest.AzLibs.SftpTestEntry;
using Org.SftpTest.Shared;

namespace SftpTest
{
    public static class SftpTestFunctionEntry
    {
        [FunctionName("SftpTestFunction")]
        public async static Task Run([TimerTrigger("0 */10 * * * *")]TimerInfo myTimer, ILogger log, ExecutionContext context)
        {
            log.LogInformation($"The SftpTest Azure Function is beginning at: {DateTime.Now}");

            var sftpParms = getSftpParms(context);

            using (var sftpTestLibEntry = new SftpTestLibEntry()) {
                var result = await sftpTestLibEntry.Run(log, sftpParms);
                log.LogInformation(result.Report);
            }

            log.LogInformation($"The SftpTest Azure Function is beginning at: {DateTime.Now}");
        }

        public async static Task<SftpTestResult> RunFromConsole(ILogger log, SftpParms sftpParms) {
            using (var sftpTestLibEntry = new SftpTestLibEntry()) {
                var result = await sftpTestLibEntry.Run(log, sftpParms);
                log.LogInformation(result.Report);
                return result;
            }
        }

        private static SftpParms getSftpParms(ExecutionContext context) {
            var sftpParms = new SftpParms();
            sftpParms.SftpUrl = System.Environment.GetEnvironmentVariable("SftpUrl");
            sftpParms.SftpHostFingerprint = System.Environment.GetEnvironmentVariable("SftpHostFingerprint");
            sftpParms.SftpLibrary = System.Environment.GetEnvironmentVariable("SftpLibrary");
            if (DateTime.Now.Minute.ToString()[0].ToString().In("0,2,4")) {
                sftpParms.SftpLibrary = "Kellerman";
            }
            else {
                sftpParms.SftpLibrary = "WinScp";
            }
            sftpParms.SftpUserName = System.Environment.GetEnvironmentVariable("SftpUserName");
            sftpParms.SftpPassword = System.Environment.GetEnvironmentVariable("SftpPassword");
            sftpParms.SftpKellermanUser = System.Environment.GetEnvironmentVariable("SftpKellermanUser");
            sftpParms.SftpKellermanLicense = System.Environment.GetEnvironmentVariable("SftpKellermanLicense");
            sftpParms.WinScpBinDirectory = context.FunctionAppDirectory;
            return sftpParms;
        }
    }
}
