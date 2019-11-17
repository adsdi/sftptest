using System;

namespace Org.SftpTest.Shared {
    public class SftpParms {
        public string SftpLibrary { get; set; }
        public string SftpUrl { get; set; }
        public string SftpHostFingerprint { get; set; }
        public string SftpUserName { get; set; }
        public string SftpPassword { get; set; }
        public string SftpKellermanUser { get; set; }
        public string SftpKellermanLicense { get; set; }
        public string WinScpBinDirectory { get; set; }
    }
}
