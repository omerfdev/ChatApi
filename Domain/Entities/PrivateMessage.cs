using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class PrivateMessage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("text_body")]
        public string TextBody { get; set; }
        [BsonElement("creation_date")]
        public DateTime CreationDate { get; set; }
        [BsonElement("sender_id")]
        public string SenderId { get; set; }
        [BsonElement("sender")]
        public User Sender { get; set; }
        [BsonElement("receiver_id")]
        public string ReceiverId { get; set; }
        [BsonElement("receiver")]
        public User Receiver { get; set; }
    }
}
