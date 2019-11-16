using System;

namespace Org.SftpTest.AzLibs.SftpTestEntry {
    public class SftpTestLibEntry : IDisposable {
        public string Run() {

            return "Ran in LibEntry.";
        }

        public void Dispose() { }
    }
}
