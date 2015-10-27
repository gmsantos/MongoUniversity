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
            var ConventionPack = new ConventionPack();
            ConventionPack.Add(new CamelCaseElementNameConvention());
            ConventionRegistry.Register("camelCase", ConventionPack, t => true);

            var client = new MongoClient("mongodb://localhost:27017");
            var db = client.GetDatabase("students");
            var col = db.GetCollection<Grade>("grades");

            await col.Find(new BsonDocument()).ForEachAsync(doc => Console.WriteLine(doc));
          
        }
    }
}
