using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Org.SftpTest.Shared {
    public interface ILibWrapper {
        Task<SftpTestResult> Run(ILogger log, SftpParms sftpParms);
    }
}
