// // using MyApi.Models;
// // using MongoDB.Driver;
// // using Microsoft.Extensions.Options;

// // namespace MyApi.Services
// // {
// //     public class ProductService
// //     {
// //         private readonly IMongoCollection<Product> _products;

// //         public ProductService(IOptions<MongoDBSettings> settings)
// //         {
// //             var client = new MongoClient(settings.Value.ConnectionString);
// //             var database = client.GetDatabase(settings.Value.DatabaseName);
// //             _products = database.GetCollection<Product>(settings.Value.CollectionName);
// //         }

// //         public async Task<List<Product>> GetAllAsync() =>
// //             await _products.Find(p => true).ToListAsync();

// //         public async Task<Product?> GetByIdAsync(string id) =>
// //             await _products.Find(p => p.Id == id).FirstOrDefaultAsync();

// //         public async Task CreateAsync(Product product) =>
// //             await _products.InsertOneAsync(product);
// //     }
// // }

// using MyApi.Models;
// using Microsoft.Extensions.Options;
// using MongoDB.Driver;
// using MongoDB.Bson;

// namespace MyApi.Services
// {
//     public class ProductService
//     {
//         private readonly IMongoCollection<Product> _productCollection;

//         public ProductService(IOptions<MongoDBSettings> settings)
//         {
//             var mongoClient = new MongoClient(settings.Value.ConnectionString);
//             var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
//             _productCollection = mongoDatabase.GetCollection<Product>(settings.Value.CollectionName);
//         }
//         public async Task<List<Product>> GetAsync() =>
//             await _productCollection.Find(_ => true).ToListAsync();

//         public async Task<Product?> GetAsyncById(string id) =>
//             await _productCollection.Find(p => p.Id == id).FirstOrDefaultAsync();

//         public async Task CreateAsync(Product product) =>
//             await _productCollection.InsertOneAsync(product);
//         public async Task<bool> UpdateProductAsync(string id, Product product)
//         {
//             var result = await _productCollection.ReplaceOneAsync(
//                 c => c.Id == id, product);
//             return result.IsAcknowledged && result.ModifiedCount > 0;
//         }
//             public async Task<bool> DeleteCategoryAsync(string id)
//         {
//             var result = await _productCollection.DeleteOneAsync(c => c.Id == id);
//             return result.DeletedCount > 0;
//         }
//     }
// }

