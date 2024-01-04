using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]       
        public string  Id { get; set; }
        [BsonElement("username")]
        public string Username { get; set; } = string.Empty;
        [BsonElement("about")]
        public string About { get; set; } = string.Empty;
        [BsonElement("is_dark_theme")]
        public bool IsDarkTheme { get; set; } = false;
        [BsonElement("password_hash")]
        public byte[] PasswordHash { get; set; }
        [BsonElement("password_salt")]
        public byte[] PasswordSalt { get; set; }
        [BsonElement("image_id")]
        public string? ImageId { get; set; }
        [BsonElement("image")]
        public Images Image { get; set; }
        public ICollection<PrivateMessage> SendedPrivateMessages { get; set; }
        public ICollection<PrivateMessage> ReceivedPrivateMessages { get; set; }
    }
}
