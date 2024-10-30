namespace ClientSWH.DocsRecordCore.Models
{
    public class DocRecordBase
    {
        public string DocId { get; set; }
        public string DocText { get; set; } = null!;
    }
    public class DocRecordDocId
    {
        public DocRecordBase MongoDocRecord { get; set; }

    }
}