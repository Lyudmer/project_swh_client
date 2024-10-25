namespace ClientSWH.Application.Interfaces
{
    public interface IReceivFromServer
    {
        Task<int> LoadMessage();
    }
}
