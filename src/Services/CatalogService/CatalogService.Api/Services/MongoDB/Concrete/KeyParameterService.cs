using CatalogService.Api.Entities.MongoDB;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.Settings;
using CatalogService.Api.Services.MongoDB.Abstract;
using CatalogService.Api.Utilities.Results;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CatalogService.Api.Services.MongoDB.Concrete;

public class KeyParameterService 
    : IKeyParameterService<KeyParameter, StringModel>
{
    private readonly IMongoCollection<KeyParameter> _keyParameterCollection;
    private readonly MongoDbSettings _mongoDbSettings;

    public KeyParameterService(
        IOptions<MongoDbSettings> mongoDbSettings)
    {
        _mongoDbSettings = mongoDbSettings.Value;

        MongoClient client = new MongoClient(_mongoDbSettings.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(_mongoDbSettings.DatabaseName);
        _keyParameterCollection = database.GetCollection<KeyParameter>(_mongoDbSettings.CollectionName);
    }

    public async Task CreateAsync(KeyParameter model)
    {
        await _keyParameterCollection.InsertOneAsync(model);
        return;
    }

    public async Task<Result> UpdateAsync(KeyParameter model)
    {
        FilterDefinition<KeyParameter> filter = Builders<KeyParameter>.Filter.Eq("Id", model.Id);

        var result = await _keyParameterCollection.ReplaceOneAsync(filter, model);
        return result.ModifiedCount > 0 ? new SuccessResult("Update completed") : new ErrorResult("Update not successful");
    }

    public async Task<Result> DeleteAsync(StringModel id)
    {
        var result = await _keyParameterCollection.DeleteOneAsync(k => k.Id == id.Value);
        return result.DeletedCount > 0 ? new SuccessResult("Delete completed") : new ErrorResult("Delete not successful");
    }

    public async Task<List<KeyParameter>> GetAllAsync()
    {
        var result = await _keyParameterCollection.FindAsync(new BsonDocument());
        return await result.ToListAsync();
    }

    public async Task<KeyParameter> GetAsync(StringModel id)
    {
        var result = await _keyParameterCollection.FindAsync(k => k.Id == id.Value);
        return await result.FirstOrDefaultAsync();
    }

    public async Task<KeyParameter> GetByKeyAsync(StringModel key)
    {
        var result = await _keyParameterCollection.FindAsync(k => k.Key == key.Value);
        return await result.FirstOrDefaultAsync();
    }
}
