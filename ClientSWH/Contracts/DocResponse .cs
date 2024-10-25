using System.ComponentModel.DataAnnotations;

namespace ClientSWH.Contracts
{
    public record PkgSendResponse
    (
        [Required]
        int Pid
     
    );
    
}
