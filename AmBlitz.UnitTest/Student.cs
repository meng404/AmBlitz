using AmBlitz.Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AmBlitz.UnitTest
{
    [Entity(DbName = "EventBigData", TableName = "Student")]
    public class Student : IEntity<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
