
using MyApi.DTOs;
using MongoDB.Bson;
using MongoDB.Driver;
using MyApi.Models;
using Microsoft.Extensions.Options;

namespace MyApi.Services
{
    public class SubCategoryService
    {
        private readonly IMongoCollection<SubCategory> _subCategoryCollection;

        public SubCategoryService(IOptions<MongoDBSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _subCategoryCollection = mongoDatabase.GetCollection<SubCategory>(settings.Value.SubCategoryCollectionName);
        }

        public async Task<List<SubCategory>> GetAllAsync() =>
            await _subCategoryCollection.Find(_ => true).ToListAsync();

        public async Task<SubCategory?> GetByIdAsync(string id) =>
            await _subCategoryCollection.Find(sc => sc.Id == id).FirstOrDefaultAsync();

        public async Task<SubCategory?> GetByCategoryIdAndSubNameAsync(string? categoryId, string subName)
        {
            if (string.IsNullOrWhiteSpace(categoryId))
            {
                return await _subCategoryCollection.Find(sc =>
                    sc.CategoryId == null &&
                    sc.SubName.ToLowerInvariant() == subName.ToLowerInvariant())
                    .FirstOrDefaultAsync();
            }
            else
            {
                return await _subCategoryCollection.Find(sc =>
                    sc.CategoryId == categoryId &&
                    sc.SubName.ToLowerInvariant() == subName.ToLowerInvariant())
                    .FirstOrDefaultAsync();
            }
        }


        //     public async Task<SubCategory?> GetByCategoryIdAndSubNameAsync(string? categoryId, string subName)
        //     {
        //         var filterBuilder = Builders<SubCategory>.Filter;
        //         var filters = new List<FilterDefinition<SubCategory>>
        // {
        //     filterBuilder.Eq(sc => sc.SubName, subName)
        // };

        //         if (!string.IsNullOrWhiteSpace(categoryId))
        //         {
        //             filters.Add(filterBuilder.Eq(sc => sc.CategoryId, categoryId));
        //         }
        //         else
        //         {
        //             filters.Add(filterBuilder.Eq(sc => sc.CategoryId, null));
        //         }

        //         var filter = filterBuilder.And(filters);
        //         return await _subCategoryCollection.Find(filter).FirstOrDefaultAsync();
        //     }

        public async Task CreateSubCategoryAsync(SubCategory subCategory) =>
            await _subCategoryCollection.InsertOneAsync(subCategory);

        public async Task<List<SubCategoryWithCategoryName>> GetAllWithCategoryNamesAsync()
        {
            var pipeline = new[]
            {
                new BsonDocument("$lookup", new BsonDocument
                {
                    { "from", "Categories" },
                    { "localField", "categoryId" },
                    { "foreignField", "_id" },
                    { "as", "CategoryDetails" }
                }),
                new BsonDocument("$unwind", new BsonDocument
                {
                    { "path", "$CategoryDetails" },
                    { "preserveNullAndEmptyArrays", true }
                }),
                new BsonDocument("$project", new BsonDocument
                {
                    { "subName", 1 },
                    { "CategoryType", "$CategoryDetails.name" },
                    { "_id",1}
                })
            };

            var bsonResults = await _subCategoryCollection
                .Aggregate<BsonDocument>(pipeline)
                .ToListAsync();

            var result = bsonResults.Select(b => new SubCategoryWithCategoryName
            {
                SubName = b.GetValue("subName", "").AsString,
                CategoryType = b.GetValue("CategoryType", "").AsString
            }).ToList();

            return result;
        }
    }
}
