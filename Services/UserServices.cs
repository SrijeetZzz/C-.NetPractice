using MyApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MyApi.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserService(IOptions<MongoDBSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _userCollection = mongoDatabase.GetCollection<User>(settings.Value.AdminCollectionName);
        }

        public async Task<List<User>> GetAsync() =>
            await _userCollection.Find(_ => true).ToListAsync();

        public async Task<User?> GetByEmailAsync(string email) =>
            await _userCollection.Find(u => u.Email == email).FirstOrDefaultAsync();

        public async Task CreateAsync(User user) =>
            await _userCollection.InsertOneAsync(user);
    }
}