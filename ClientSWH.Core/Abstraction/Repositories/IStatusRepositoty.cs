using ClientSWH.Core.Models;


namespace ClientSWH.Core.Abstraction.Repositories
{
    public interface IStatusRepositoty
    {
        Task<int> Add(Status status);
        Task Delete(int Id);
        Task Update(Status status);
        Task<Status> GetById(int Id);
    }
}