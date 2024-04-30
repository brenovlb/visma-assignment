using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace visma_assignment.Model
{
    public class Employee
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime EmploymentDate { get; set; }
        public string HomeAddress { get; set; }
        public decimal CurrentSalary { get; set; }
        public string Role { get; set; }
        public string? BossId { get; set; }
    }
}
