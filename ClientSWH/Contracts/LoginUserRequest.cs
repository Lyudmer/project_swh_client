using System.ComponentModel.DataAnnotations;

namespace ClientSWH.Contracts
{
    public record LoginUserRequest
    (
     [Required]
        string email,
     [Required]
        string passwordHash
    );
}
