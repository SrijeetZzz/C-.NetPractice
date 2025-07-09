// namespace MyApi.Models // 👈 Use your project namespace
// {
//     public class MongoDBSettings
//     {
//         public string ConnectionString { get; set; } = null!;
//         public string DatabaseName { get; set; } = null!;
//         public string CollectionName { get; set; } = null!;
//     }
// }
namespace MyApi.Models
{
    public class MongoDBSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        // public string CollectionName { get; set; } = null!;
        // public string UserCollectionName { get; set; } = null!;
        public string CategoryCollectionName { get; set; } = null!;

    }
}
