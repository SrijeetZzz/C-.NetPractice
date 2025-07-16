// namespace MyApi.Models
// {
//     public class Product
//     {
//         public int Id { get; set; }
//         public string Name { get; set; }
//     }
// }


// using MongoDB.Bson;
// using MongoDB.Bson.Serialization.Attributes;

// namespace MyApi.Models
// {
//     public class Product
//     {
//         [BsonId]
//         [BsonRepresentation(BsonType.ObjectId)]
//         public string? Id { get; set; }

//         public string Name { get; set; }
//     }
// }
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyApi.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? Price { get; set; } 
    }
}
