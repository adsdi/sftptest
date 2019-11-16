using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Org.SftpTest.AzLibs.SftpTestEntry;

namespace SftpTest
{
    public static class SftpTestFunctionEntry
    {
        [FunctionName("SftpTestFunction")]
        public static void Run([TimerTrigger("0 */60 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"The SftpTest Azure Function is beginning at: {DateTime.Now}");

            using (var sftpTestLibEntry = new SftpTestLibEntry()) {
                log.LogInformation(sftpTestLibEntry.Run());
            }

            log.LogInformation($"The SftpTest Azure Function is beginning at: {DateTime.Now}");
        }

        public static string RunFromConsole(string option) {

            string result = string.Empty;

            using (var sftpTestLibEntry = new SftpTestLibEntry()) {
                result = sftpTestLibEntry.Run();
            }
            
            return "result";
        }
    }
}
