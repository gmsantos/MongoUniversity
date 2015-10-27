using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace M101DotNet.ConsoleApp
{
    class Grade
    {
        public ObjectId Id { get; set; }

        public string Student_Id { get; set; }

        public string Type { get; set; }

        public double Score { get; set; }

    }
}
