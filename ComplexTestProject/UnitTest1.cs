using DogsHouseService.Controllers;
using DogsHouseService.Model;
using Newtonsoft.Json;

namespace ComplexTestProject
{
    [TestFixture]
    public class Tests
    {

        /*
        Method     Path                            Status
        --------------------------------------------------

        GET        /dogs                           |*|

        POST       /dogs                           |*|

        GET        /dogs/Example                   |*|

        GET        /dogs/SortByAttributeAndOrder   |*|

        GET        /dogs/SortByAttribute           |*|

        GET        /dogs/WithPagination            |*|

        GET        /ping                           |*|
        */
        [Test]
        public async Task PingTest()
        {

            var pingController = new PingController();

            if (pingController.Get().Result == "Dogs house service. Version 1.0.1")
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }
        [Test]
        public async Task GetExampleTest()
        {
            var dogsController = new DogsController();

            var jsonExampleDog = dogsController.GetExample();
            var ExampleDog = JsonConvert.DeserializeObject<Dog>(jsonExampleDog);
            if (string.IsNullOrEmpty(ExampleDog.Name))
            {
                Assert.Fail();
            }
            else
            {
                Assert.Pass();
            }
        }
        [Test]
        public async Task AddNewAndGetAllTest()
        {

            var dogsController = new DogsController();

            Dog dog = new Dog();
            dog.Name = $"Dog For Test On {DateTime.Now}";
            dog.Color = "Yellow and Black";
            dog.TailLength = 0;
            dog.Weight = 100;

            var jsonDog = JsonConvert.SerializeObject(dog);
            dogsController.Add(jsonDog);

            var allDogsJson = dogsController.Get().Result;
            var allDogs = JsonConvert.DeserializeObject<IEnumerable<Dog>>(allDogsJson);

            if (allDogs.Any(x => x.Name == dog.Name))
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }


        [Test]
        public async Task SortingByAttributeTest()
        {
            var dogsController = new DogsController();

            Dog dog1 = new Dog();
            dog1.Name = $"1 Dog For Test On {DateTime.Now}";
            dog1.Color = "Yellow and Black";
            dog1.TailLength = 20;
            dog1.Weight = 70;
            Dog dog2 = new Dog();
            dog2.Name = $"2 Dog For Test On {DateTime.Now}";
            dog2.Color = "Yellow and Black";
            dog2.TailLength = 10;
            dog2.Weight = 30;
            Dog dog3 = new Dog();
            dog3.Name = $"3 Dog For Test On {DateTime.Now}";
            dog3.Color = "Yellow and Black";
            dog3.TailLength = 30;
            dog3.Weight = 50;


            var jsonDog1 = JsonConvert.SerializeObject(dog1);
            var jsonDog2 = JsonConvert.SerializeObject(dog2);
            var jsonDog3 = JsonConvert.SerializeObject(dog3);
            dogsController.Add(jsonDog1);
            dogsController.Add(jsonDog2);
            dogsController.Add(jsonDog3);

            var jsonDogsByAttribute = dogsController.GetByAttribute("tail_length");

            var DogsByAttribute = JsonConvert.DeserializeObject<IEnumerable<Dog>>(jsonDogsByAttribute);
            var clearedDogs = DogsByAttribute.Where(x => x.Name == dog1.Name || x.Name == dog2.Name || x.Name == dog3.Name).ToList();
            if (clearedDogs.First().TailLength == 10)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public async Task SortingByAttributeAndOrder()
        {
            var dogsController = new DogsController();

            Dog dog1 = new Dog();
            dog1.Name = $"S1 Dog For Test On {DateTime.Now}";
            dog1.Color = "Yellow and Black";
            dog1.TailLength = 20;
            dog1.Weight = 70;
            Dog dog2 = new Dog();
            dog2.Name = $"S2 Dog For Test On {DateTime.Now}";
            dog2.Color = "Yellow and Black";
            dog2.TailLength = 10;
            dog2.Weight = 30;
            Dog dog3 = new Dog();
            dog3.Name = $"S3 Dog For Test On {DateTime.Now}";
            dog3.Color = "Yellow and Black";
            dog3.TailLength = 30;
            dog3.Weight = 50;


            var jsonDog1 = JsonConvert.SerializeObject(dog1);
            var jsonDog2 = JsonConvert.SerializeObject(dog2);
            var jsonDog3 = JsonConvert.SerializeObject(dog3);
            dogsController.Add(jsonDog1);
            dogsController.Add(jsonDog2);
            dogsController.Add(jsonDog3);

            var jsonDogsByAttribute = dogsController.GetByAttributeAndOrder("weight", "desc");

            var DogsByAttribute = JsonConvert.DeserializeObject<IEnumerable<Dog>>(jsonDogsByAttribute);
            var clearedDogs = DogsByAttribute.Where(x => x.Name == dog1.Name || x.Name == dog2.Name || x.Name == dog3.Name).ToList();
            if (clearedDogs.First().Weight == 70)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }

        [Test]
        public async Task PaginationTest()
        {

            var dogsController = new DogsController();


            var allDogsJson = dogsController.Get().Result;
            var allDogs = JsonConvert.DeserializeObject<IEnumerable<Dog>>(allDogsJson);

            var startCount = allDogs.Count();

            Dog dog1 = new Dog();
            dog1.Name = $"1 Dog For Test On {DateTime.Now}";
            dog1.Color = "Yellow and Black";
            dog1.TailLength = 20;
            dog1.Weight = 100;
            Dog dog2 = new Dog();
            dog2.Name = $"2 Dog For Test On {DateTime.Now}";
            dog2.Color = "Yellow and Black";
            dog2.TailLength = 10;
            dog2.Weight = 100;
            Dog dog3 = new Dog();
            dog3.Name = $"3 Dog For Test On {DateTime.Now}";
            dog3.Color = "Yellow and Black";
            dog3.TailLength = 30;
            dog3.Weight = 100;

            var jsonDog1 = JsonConvert.SerializeObject(dog1);
            var jsonDog2 = JsonConvert.SerializeObject(dog2);
            var jsonDog3 = JsonConvert.SerializeObject(dog3);
            dogsController.Add(jsonDog1);
            dogsController.Add(jsonDog2);
            dogsController.Add(jsonDog3);

            var limit = startCount + 2;
            var paginationJson = dogsController.GetWithPagination(2, limit);
            var paginationDogs = JsonConvert.DeserializeObject<IEnumerable<Dog>>(paginationJson);

            if (paginationDogs.Count() == 1)
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }



        /**Some variant of functional tests
         
        ***INSTALL Microsoft.AspNetCore.Mvc.Testing BEFORE***

        
        [TestFixture]
        public class Tests
        ......
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;
        [SetUp]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }
        .....
        [Test]
        public async Task PingTest()
        {

            var ping = await _client.GetAsync("/ping");
            var content = await ping.Content.ReadAsStringAsync();
            if (content == "Dogs house service. Version 1.0.1")
            {
                Assert.Pass();
            }
            else
            {
                Assert.Fail();
            }
        }
        [Test]
        public async Task GetExampleTest()
        {
            try
            {
                var dogAsMessage = await _client.GetAsync("/dogs/example");
                var dogStr = await dogAsMessage.Content.ReadAsStringAsync();
                var dog = JsonConvert.DeserializeObject(dogStr);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
            Assert.Pass();
        }
         */
    }
}
