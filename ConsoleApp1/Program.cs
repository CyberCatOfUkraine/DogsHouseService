using DB;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Console.WriteLine();


            string json = @"[
            {""Name"": ""Neo"", ""Color"": ""red & amber"", ""TailLength"": 22, ""Weight"": 32},
            {""Name"": ""Jessy"", ""Color"": ""black & white"", ""TailLength"": 7, ""Weight"": 14}
        ]";

            List<DB.Model.Dog> animalList = JsonConvert.DeserializeObject<List<DB.Model.Dog>>(json);

            // Додаткові екземпляри для додавання в список
            DB.Model.Dog additionalAnimal1 = new DB.Model.Dog
            {
                Name = "Max",
                Color = "brown",
                TailLength = 15,
                Weight = 20
            };

            DB.Model.Dog additionalAnimal2 = new DB.Model.Dog
            {
                Name = "Lucy",
                Color = "gray",
                TailLength = 10,
                Weight = 12
            };

            // Додавання екземплярів до списку
            animalList.Add(additionalAnimal1);
            animalList.Add(additionalAnimal2);


            DBManager dBManager = new();
            var repo = dBManager.DogRepository;
            /*foreach (var dog in repo.GetAll())
            {
                repo.Remove(dog);
            }*/
            //repo.RemoveRange(repo.GetAll());
            repo.AddRange(animalList);

            foreach (var dog in repo.GetAll())
            {
                Console.WriteLine(dog.Id + ":" + dog.Name + ":" + dog.Color + ":" + dog.TailLength + ":" + dog.Weight);
            }



            Console.WriteLine();
            Console.WriteLine("Finish!");
        }
    }
}