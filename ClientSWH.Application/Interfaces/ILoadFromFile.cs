using System.Xml.Linq;

namespace ClientSWH.Application.Interfaces
{
    public interface ILoadFromFile
    {
        Task<string> LoadFileXml(Guid userId, string InFile);
    }
}
