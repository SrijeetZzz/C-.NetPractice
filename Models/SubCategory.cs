// using System.ComponentModel.DataAnnotations;
// using MongoDB.Bson;
// using MongoDB.Bson.Serialization.Attributes;

// namespace MyApi.Models
// {
//     public class SubCategory
//     {

//         [BsonId]
//         [BsonRepresentation(BsonType.ObjectId)]

//         public string? Id { get; set; }

//         [BsonElement("categoryId")]
//         public int CategoryId { get; set; }

//         [BsonElement("subId")]
//         public int SubId { get; set; }

//         [BsonElement("subName")]
//         public string SubName { get; set; } = null!;
//     }
// }