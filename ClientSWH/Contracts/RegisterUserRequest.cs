using System.ComponentModel.DataAnnotations;

namespace ClientSWH.Contracts
{
    public record RegisterUserRequest(
         string UserName,
      [Required]
      [EmailAddress]
        string Email,
      [Required]
        string PasswordHash
      );
}
