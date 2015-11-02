using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using M101DotNet.ConsoleApp.Model.HomeworkGrade;
using M101DotNet.ConsoleApp.Model.HomeworkSchool;

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
            //await homeworkGrade("students", "grades");
            await homeworkSchool("school", "students");
        }

        private static async Task homeworkSchool(string dbName, string collection)
        {
            //Homework 3.1

            BsonClassMap.RegisterClassMap<Student>(cm => cm.AutoMap());
            //BsonClassMap.RegisterClassMap<Grade>(cm => cm.AutoMap());

            var client = new MongoClient("mongodb://localhost:27017");
            var db = client.GetDatabase(dbName);
            var col = db.GetCollection<Student>(collection);

            var studentsList = await col.Find("{}").ToListAsync();

            //studentsList.ForEach(s => s.removeWorseHomework());
            
            foreach(var student in studentsList)
            {
                student.removeWorseHomework();
                var filter = Builders<Student>.Filter.Eq("Id", student.Id);
                var update = Builders<Student>.Update.Set("Scores", student.Scores);

                await col.UpdateOneAsync(filter, update);
            }
        }

        private static async Task homeworkGrade(string dbName, string collection)
        {
            // Homework 2.2

            //var ConventionPack = new ConventionPack();
            //ConventionPack.Add(new CamelCaseElementNameConvention());
            //ConventionRegistry.Register("camelCase", ConventionPack, t => true);

            BsonClassMap.RegisterClassMap<Grade>(cm =>
            {
                cm.AutoMap();
            });

            var client = new MongoClient("mongodb://localhost:27017");
            var db = client.GetDatabase(dbName);
            var col = db.GetCollection<Grade>(collection);

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
                if (deleteNext == true)
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
