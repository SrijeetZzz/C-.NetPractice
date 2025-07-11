
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
            return await _subCategoryCollection.Find(sc =>
                sc.CategoryId == categoryId &&
                sc.SubName.ToLowerInvariant() == subName.ToLowerInvariant())
                .FirstOrDefaultAsync();
        }

        public async Task CreateSubCategoryAsync(SubCategory subCategory) =>
            await _subCategoryCollection.InsertOneAsync(subCategory);

        public async Task<List<SubCategoryWithCategoryName>> GetAllWithCategoryNamesAsync(int skip, int pageSize)
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
                    { "_id", 1 }
                }),
                new BsonDocument("$skip", skip),
                new BsonDocument("$limit", pageSize)
            };

            var bsonResults = await _subCategoryCollection.Aggregate<BsonDocument>(pipeline).ToListAsync();

            var result = bsonResults.Select(b => new SubCategoryWithCategoryName
            {
                SubName = b.Contains("subName") && b["subName"].IsString ? b["subName"].AsString : "",
                CategoryType = b.Contains("CategoryType") && b["CategoryType"].IsString ? b["CategoryType"].AsString : ""
            }).ToList();

            return result;
        }

        public async Task<List<SubCategoryWithNameAndLabel>> GetAllCategoryNamesWithLabels(int skip, int pageSize)
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
                    { "CategoryName", "$CategoryDetails.name" }
                }),
                new BsonDocument("$set", new BsonDocument("CategoryLabel", new BsonDocument("$concat", new BsonArray
                {
                    "$subName",
                    "(",
                    new BsonDocument("$ifNull", new BsonArray { "$CategoryName", "" }),
                    ")"
                }))),
                new BsonDocument("$skip", skip),
                new BsonDocument("$limit", pageSize)
            };

            var bsonResults = await _subCategoryCollection.Aggregate<BsonDocument>(pipeline).ToListAsync();

            var result = bsonResults.Select(b => new SubCategoryWithNameAndLabel
            {
                SubName = b.GetValue("subName", BsonNull.Value).ToString(),
                CategoryType = b.Contains("CategoryName") && b["CategoryName"].IsString ? b["CategoryName"].AsString : "",
                CategoryLabel = b.Contains("CategoryLabel") && b["CategoryLabel"].IsString ? b["CategoryLabel"].AsString : ""
            }).ToList();

            return result;
        }

        public async Task<long> GetTotalCountAsync() =>
            await _subCategoryCollection.CountDocumentsAsync(FilterDefinition<SubCategory>.Empty);
    }
}
