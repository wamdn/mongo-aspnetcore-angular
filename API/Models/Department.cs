using MongoDB.Bson.Serialization.Attributes;

namespace API.Models;

public class Department
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonRequired]
    [BsonElement("name")]
    public string? Name { get; set; }
}
