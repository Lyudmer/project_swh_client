using ClientSWH.Core.Models;
using System.Xml.Linq;

namespace ClientSWH.Core.Abstraction.Services
{
    public interface IPackagesServices
    {
    
        Task<string> LoadFile(Guid UserId, string xFile);
        Task<int> LoadMessage();
        Task<int> SendToServer(int Pid);
        Task<bool> SendDelPkgToServer(int Pid);
        Task<int> PkgFLK(int Pid);
        Task<string> GetPkgXml(int Pid);
        Task<HistoryPkg> HistoriPkgByPid(int Pid);
        Task<Package> GetPkgId(int Pid);
        Task<List<Package>> GetAll();
        Task<List<Document>> GetDocsPkg(int Pid);
        Task<string> DeletePkg(int Pid);
    }
}