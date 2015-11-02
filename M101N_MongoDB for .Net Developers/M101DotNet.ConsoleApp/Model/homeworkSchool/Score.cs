using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M101DotNet.ConsoleApp.Model.homeworkSchool
{
    class Score
    {
        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("score")]
        public double ScoreValue { get; set; }
        
    }
}
