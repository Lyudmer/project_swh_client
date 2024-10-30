using ClientSWH.DocsRecordCore.Models;


namespace ClientSWH.Contracts
{
    public class DocRecordResponse
    {
        public IEnumerable<DocRecord> MongoDocRecord { get; set; }

    }
}
