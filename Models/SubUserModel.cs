using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyApi.Models
{
    public class SubUserModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        public string? Id { get; set; }

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UserId { get; set; }

        [BsonElement("subUserName")]
        public string SubUserName { get; set; } = null!;

        [BsonElement("subUserEmail")]
        public string SubUserEmail { get; set; } = null!;

        [BsonElement("subUserPhoneNo")]
        public string SubUserPhoneno { get; set; } = null!;

    }
}
