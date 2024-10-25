using System.ComponentModel.DataAnnotations;

namespace ClientSWH.Contracts
{
    public record PackageRequest
    (
        [Required]
        int Pid
     
    );
    
}
