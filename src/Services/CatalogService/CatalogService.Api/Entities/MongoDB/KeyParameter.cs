using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace CatalogService.Api.Entities.MongoDB
{
    public class KeyParameter
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("key")]
        [JsonPropertyName("key")]
        public string Key { get; set; } = null!;

        [BsonElement("value")]
        [JsonPropertyName("value")]
        public object Value { get; set; } = null!;
    }
}
