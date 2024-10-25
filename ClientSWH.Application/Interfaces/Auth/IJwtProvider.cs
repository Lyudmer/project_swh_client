using ClientSWH.Core.Models;

namespace ClientSWH.Application.Interfaces.Auth
{
    public interface IJwtProvider
    {
        string GenerateToken(Guid userId);
        string ReadToken(string token);
    }
}