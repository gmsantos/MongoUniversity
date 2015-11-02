using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using M101DotNet.ConsoleApp.Model.homeworkSchool;

namespace M101DotNet.ConsoleApp.Model.HomeworkSchool
{
    class Student
    {
        public int Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("scores")]
        public IList<Score> Scores { get; set; }

        public void removeWorseHomework()
        {
            var lowerScore = (from s in this.Scores
                             where s.Type == "homework"
                             orderby s.ScoreValue
                             select s).First();

            this.Scores.Remove(lowerScore);
        }
    }
}
