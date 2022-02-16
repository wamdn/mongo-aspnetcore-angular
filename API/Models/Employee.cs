using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Models;

public class Employee
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonRequired]
    [BsonElement(elementName: "name")]
    public string? Name { get; set; }

    [BsonRequired]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement(elementName: "dept_id")]
    public string? DepartmentId { get; set; }

    [BsonElement(elementName: "join_date")]
    public DateTime DateOfJoining { get; set; }

    [BsonElement(elementName: "img")]
    public string ImageName { get; set; } = "anonymous.png";
}
