using ClientSWH.Core.Models;

namespace ClientSWH.Core.Abstraction.Repositories
{
    public interface IHistoryPkgRepository
    {
        Task<HistoryPkg> Add(HistoryPkg HpPkg);
        Task<HistoryPkg> GetById(int Pid);
    }
}