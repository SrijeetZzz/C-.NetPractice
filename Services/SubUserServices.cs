using MyApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using MyApi.DTOs;
using System.Text.RegularExpressions;


namespace MyApi.Services
{
    public class SubUserServices
    {
        private readonly IMongoCollection<SubUserModel> _subUserCollection;
        public SubUserServices(IOptions<MongoDBSettings> settings)
        {
            var mongoClient = new MongoClient(settings.Value.ConnectionString);
            var mongoDataBase = mongoClient.GetDatabase(settings.Value.DatabaseName);
            _subUserCollection = mongoDataBase.GetCollection<SubUserModel>(settings.Value.UserCollectionName);

        }
        public async Task CreateAsync(SubUserModel subUser) =>
            await _subUserCollection.InsertOneAsync(subUser);
        public async Task<List<SubUserModel>> GetUsersAsync() =>
            await _subUserCollection.Find(_ => true).ToListAsync();
        public async Task<SubUserModel?> GetByIdAsync(string id)
        {
                var objectId = new ObjectId(id);
                return await _subUserCollection.Find(sb => sb.Id == objectId.ToString()).FirstOrDefaultAsync();
        }
        public async Task<List<SubUsersList>> GetAllUsersUnderAdminAsync(int skip, int pageSize, string searchTerm)
        {
            var pipeline = new List<BsonDocument>();      
               
            pipeline.Add(new BsonDocument("$lookup",
                new BsonDocument
                {
            { "from", "Admins" },
            { "localField", "userId" },
            { "foreignField", "_id" },
            { "as", "UsersList" }
                }));

            pipeline.Add(new BsonDocument("$unwind",
                new BsonDocument
                {
            { "path", "$UsersList" },
            { "preserveNullAndEmptyArrays", true }
                }));

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string escaped = Regex.Escape(searchTerm);
                string pattern = $".*{escaped}.*";
                var regex = new BsonRegularExpression(pattern, "i");

                var orConditions = new BsonArray
                {
                    new BsonDocument("subUserName", regex),
                    new BsonDocument("subUserEmail", regex),
                    new BsonDocument("subUserPhoneNo", regex),
                    new BsonDocument("UsersList.name", regex),
                    new BsonDocument("UsersList.email", regex),
                    new BsonDocument("UsersList.phoneNo", regex),
                };

                pipeline.Add(new BsonDocument("$match", new BsonDocument("$or", orConditions)));
            }

            pipeline.Add(new BsonDocument("$project",
                new BsonDocument
                {
            { "subUserName", 1 },
            { "subUserEmail", 1 },
            { "subUserPhoneNo", 1 },
            { "adminName", "$UsersList.name" },
            { "adminEmail", "$UsersList.email" },
            { "adminPhone", "$UsersList.phoneNo" },
            // { "adminId",  "$UsersList._id"}
                }));
                // pipeline.Add(new BsonDocument("$match",
                // new BsonDocument("$expr",
                // new BsonDocument("$eq",
                // new BsonArray
                //     {
                //         "$userId",
                //         "$adminId"
                //     }))));
            

            pipeline.Add(new BsonDocument("$skip", skip));
            pipeline.Add(new BsonDocument("$limit", pageSize));

            var bsonResult = await _subUserCollection.Aggregate<BsonDocument>(pipeline).ToListAsync();

            var result = bsonResult.Select(b => new SubUsersList
            {
                subUserName = b.GetValue("subUserName", "").AsString,
                subUserEmail = b.GetValue("subUserEmail", "").AsString,
                subUserPhoneNo = b.GetValue("subUserPhoneNo", "").AsString,
                adminName = b.GetValue("adminName", "").AsString,
                adminEmail = b.GetValue("adminEmail", "").AsString,
                adminPhone = b.GetValue("adminPhone", "").AsString,
                _id = b.GetValue("_id", "").ToString()
            }).ToList();

            return result;
        }

        public async Task<long> GetTotalCountAsync() =>
            await _subUserCollection.CountDocumentsAsync(FilterDefinition<SubUserModel>.Empty);
        public async Task<long> GetFilteredCountAsync(string searchTerm)
        {
            var pipeline = new List<BsonDocument>();

            pipeline.Add(new BsonDocument("$lookup",
                new BsonDocument
                {
            { "from", "Admins" },
            { "localField", "userId" },
            { "foreignField", "_id" },
            { "as", "UsersList" }
                }));

            pipeline.Add(new BsonDocument("$unwind",
                new BsonDocument
                {
            { "path", "$UsersList" },
            { "preserveNullAndEmptyArrays", true }
                }));

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string escaped = Regex.Escape(searchTerm);
                string pattern = $".*{escaped}.*";
                var regex = new BsonRegularExpression(pattern, "i");

                var orConditions = new BsonArray
            {
                new BsonDocument("subUserName", regex),
                new BsonDocument("subUserEmail", regex),
                new BsonDocument("subUserPhoneNo", regex),
                new BsonDocument("UsersList.name", regex),
                new BsonDocument("UsersList.email", regex),
                new BsonDocument("UsersList.phoneNo", regex),
            };

                pipeline.Add(new BsonDocument("$match", new BsonDocument("$or", orConditions)));
            }

            // Count the number of matching documents
            pipeline.Add(new BsonDocument("$count", "count"));

            var result = await _subUserCollection.Aggregate<BsonDocument>(pipeline).FirstOrDefaultAsync();

            return result != null ? result.GetValue("count", 0).ToInt64() : 0;
        }
        // update
        public async Task<bool> UpdateUserAsync(string id, SubUserModel subUser)
        {
            var result = await _subUserCollection.ReplaceOneAsync(
                c => c.Id == id, subUser);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
        public async Task<bool> DeleteUserAsync(string id)
        {
            var result = await _subUserCollection.DeleteOneAsync(c => c.Id == id);
            return result.DeletedCount > 0;
        }

    }

}
