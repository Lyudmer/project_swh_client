using System.ComponentModel.DataAnnotations;

namespace ClientSWH.Contracts
{
    public record LoadFileRequest(

      [Required]
        string FileName,
      [Required]
      string Token
      );
}
