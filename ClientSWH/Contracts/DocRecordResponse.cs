using MongoDB.Bson.Serialization.Attributes;

namespace ClientSWH.Contracts
{
    public record DocRecordResponse
    (
        Guid Id,
        Guid DocId,
        string DocText,
        DateTime CreateDate,
        DateTime ModifyDate
     );
}
