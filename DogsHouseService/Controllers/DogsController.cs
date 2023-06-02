using AutoMapper;
using DB;
using DogsHouseService.AppMapper;
using DogsHouseService.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace DogsHouseService.Controllers
{
    [Route("dogs")]
    [ApiController]
    public class DogsController : ControllerBase
    {
        DBManager _dBManager;

        IMapper mapper = new Mapper(DogsMapperConfiguration.GetConfiguration());

        public DogsController()
        {
            _dBManager = new();
        }

        [HttpGet]
        public async Task<string> Get()
        {
            return await Task.Run(() =>
            {
                var dogs = mapper.Map<IEnumerable<Dog>>(_dBManager.DogRepository.GetAll());

                return JsonConvert.SerializeObject(dogs);
            });
        }

        [HttpGet("Example")]
        public string GetExample()
        {
            var exampleDog = new Dog("Example dog", "#000000", 0, 0);
            return JsonConvert.SerializeObject(exampleDog);
        }

        [HttpGet("SortByAttributeAndOrder")]
        public string GetByAttributeAndOrder([FromQuery] string attribute, [FromQuery] string order)
        {
            var dogs = GetDogsBy(attribute);

            if (order == "desc")
                dogs = dogs.Reverse();

            return JsonConvert.SerializeObject(dogs);
        }

        [HttpGet("SortByAttribute")]
        public string GetByAttribute([FromQuery] string attribute)
        {
            return JsonConvert.SerializeObject(GetDogsBy(attribute));
        }
        private IEnumerable<Dog> GetDogsBy(string attribute)
        {
            var dogs = mapper.Map<IEnumerable<Dog>>(_dBManager.DogRepository.GetAll());
            switch (attribute)
            {
                case "name":
                    return dogs.OrderBy(x => x.Name).ToList();
                case "color":
                    return dogs.OrderBy(x => x.Color).ToList();
                case "tail_length":
                    return dogs.OrderBy(x => x.TailLength).ToList();
                case "weight":
                    return dogs.OrderBy(x => x.Weight).ToList();
                default:
                    return dogs;
            }

        }

        [HttpGet("WithPagination")]
        public string GetWithPagination([FromQuery] int pageNumber = 1, [FromQuery] int limit = 10)
        {
            if (pageNumber < 0)
            {
                return "[]";
            }
            if (limit < 1)
            {
                return "[]";
            }

            var allDogs = mapper.Map<IEnumerable<Dog>>(_dBManager.DogRepository.GetAll());

            var dogs = new List<Dog>();

            var partCounter = 0;
            var counter = 0;
            var page = --pageNumber;
            foreach (var dog in allDogs)
            {
                if (counter == limit)
                {
                    partCounter++;
                    counter = 0;
                }
                if (page == partCounter)
                {
                    dogs.Add(dog);
                }
                counter++;
            }
            return JsonConvert.SerializeObject(dogs);
        }

        private readonly string schemaJson = @"{
            'type': 'object',
            'properties': {
                'name': { 'type': 'string' },
                'color': { 'type': 'string' },
                'tail_length': { 'type': 'integer' },
                'weight': { 'type': 'integer' }
            },
            'required': ['name', 'color', 'tail_length', 'weight']
        }";

        [HttpPost]
        public IActionResult Add(string jsonDog)
        {
            #region Try
            try
            {
                #endregion
                #region Check and adding
                bool valid = false;

                try
                {
                    JSchema schema = JSchema.Parse(schemaJson);
                    JObject dogObject = JObject.Parse(jsonDog);
                    valid = dogObject.IsValid(schema);
                }
                catch (Exception e)
                {
                    if (e.InnerException != null)
                    {
                        Console.WriteLine(e.InnerException.Message);
                    }
                    Console.WriteLine(e.Message);

                    return BadRequest("This isn't JSON. Or JSON is invalid! Check the example!");
                }

                if (!valid)
                {
                    return BadRequest("This isn't JSON. Or JSON is invalid! Check the example!");
                }

                var dog = JsonConvert.DeserializeObject<Dog>(jsonDog);

                if (dog.TailLength < 0)
                {
                    return BadRequest("The tail cannot be less than zero!");
                }

                if (dog.Weight < 0)
                {
                    return BadRequest("The weight cannot be less than zero!");
                }
                if (_dBManager.DogRepository.Find(x => x.Name == dog.Name).Count() > 0)
                {
                    return BadRequest("Can't add the dog with this name!");
                }

                _dBManager.DogRepository.Add(mapper.Map<DB.Model.Dog>(dog));

                return Ok("Dog is added!");
                #endregion
                #region Catch
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    return BadRequest(e.InnerException.Message);
                }
                return BadRequest(e.Message);
            }
            #endregion
        }
    }
}
