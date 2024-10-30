namespace ClientSWH.Contracts
{
   
    public record StatusAddRequest(
       int Id,
       string StatusName,
       bool RunWf,
       bool MkRes,
       bool SendMess
    );
}
