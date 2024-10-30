using ClientSWH.Core.Models;

namespace ClientSWH.Core.Abstraction.Services
{
    public interface IStatusServices
    {
        Task<string> AddStatus(Status status);
        Task<string> DelStatus(int Id);

        Task<List<Status>> GetAllStatus();
    }
}