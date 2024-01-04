using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Images
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("cloudinary_identifier")]
        public string CloudinaryIdentifier { get; set; }
        [BsonElement("image_path")]
        public string ImagePath { get; set; }
    }
}
