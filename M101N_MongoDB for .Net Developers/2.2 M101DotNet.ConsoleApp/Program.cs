using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace M101DotNet.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
            Console.WriteLine();
            Console.WriteLine("Press Enter");
            Console.ReadLine();
        }

        static async Task MainAsync(string[] args)
        {
            //var ConventionPack = new ConventionPack();
            //ConventionPack.Add(new CamelCaseElementNameConvention());
            //ConventionRegistry.Register("camelCase", ConventionPack, t => true);

            BsonClassMap.RegisterClassMap<Grade>(cm =>
            {
                cm.AutoMap();
            });

            var client = new MongoClient("mongodb://localhost:27017");
            var db = client.GetDatabase("students");
            var col = db.GetCollection<Grade>("grades");

            var sort = Builders<Grade>.Sort.Ascending("StudentId").Ascending("Score");

            var list = await col.Find(x => x.Type == "homework").Sort(sort).ToListAsync();

            //var listToExclude = from s in list
            //                    group s by s.StudentId into g
            //                    let minScore = g.Min(s => s.Score)
            //                    select new {
            //                        StudentId = g.Key, score = minScore
            //                    };

            var deleteNext = true;

            foreach (var doc in list)
            {
                if(deleteNext == true)
                {
                    Console.WriteLine(doc.Id);
                    await col.DeleteOneAsync(x => x.Id == doc.Id);
                    deleteNext = false;
                }
                else
                {
                    deleteNext = true;
                }               
            }

        }
    }
}
