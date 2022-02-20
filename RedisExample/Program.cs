using ServiceStack.Redis;
using ServiceStack.Redis.Generic;
using System;

namespace RedisExample
{
    class Program
    {
        public static RedisClient redisclient = new RedisClient("localhost", 6379);
        public static IRedisTypedClient<Person> personClient = redisclient.As<Person>();
        static void Main(string[] args)
        {

            StorePerson();
            GetAll();
            SetPerson();

        }

        static void StorePerson()
        {
            Person person = new Person()
            {
                Id = personClient.GetNextSequence(),
                Name = "Halil"
            };
            var storedPerson = personClient.Store(person);
            var lastId = storedPerson.Id;
            GetById(lastId);
        }

        static void GetAll()
        {
            foreach (Person item in personClient.GetAll())
            {
                Console.WriteLine("GetAll->Id=" + item.Id);
                Console.WriteLine("GetAll->Name=" + item.Name);
            }
        }

        static void GetById(long id)
        {
            Person result = personClient.GetById(id);
            Console.WriteLine("GetPerson->Id=" + result.Id);
            Console.WriteLine("GetPerson->Name=" + result.Name);
        }

        static void GetByKey(string key)
        {
            Person result = redisclient.Get<Person>(key);
            Console.WriteLine("GetPerson->Id=" + result.Id);
            Console.WriteLine("GetPerson->Name=" + result.Name);
        }

        static void SetPerson()
        {
            var result = redisclient.Set<Person>("2", new Person { Id = 2, Name = "Halil2" });
            GetByKey("2");
        }
    }

    public class Person
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
