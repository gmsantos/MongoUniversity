using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace M101DotNet.ConsoleApp
{
    class Grade
    {
        public ObjectId Id { get; set; }

        [BsonElement("student_id")]
        public int StudentId { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("score")]
        public double Score { get; set; }

    }
}
