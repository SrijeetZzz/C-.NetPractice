using MyApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MyApi.Services
{
    public class CategoryService
    {
        private readonly IMongoCollection<Category> _categoryCollection;

        public CategoryService(IOptions<MongoDBSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _categoryCollection = mongoDatabase.GetCollection<Category>(settings.Value.CategoryCollectionName);
        }

        //create
        public async Task CreateCategoryAsync(Category category) =>
            await _categoryCollection.InsertOneAsync(category);

        //get all
        public async Task<List<Category>> GetCategoriesAsync() =>
            await _categoryCollection.Find(_ => true).ToListAsync();

        //get
        public async Task<Category?> GetCategoryByIdAsync(string id) =>
            await _categoryCollection.Find(c => c.Id == id).FirstOrDefaultAsync();

        //update
        public async Task<bool> UpdateCategoryAsync(string id, Category updatedCategory)
        {
            var result = await _categoryCollection.ReplaceOneAsync(
                c => c.Id == id, updatedCategory);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
        public async Task<bool> DeleteCategoryAsync(string id)
        {
            var result = await _categoryCollection.DeleteOneAsync(c => c.Id == id);
            return result.DeletedCount > 0;
        }
        public async Task<Category> GetCategoryByNameAsync(string name) =>
            await _categoryCollection.Find(c => c.Name == name).FirstOrDefaultAsync();
    }
}