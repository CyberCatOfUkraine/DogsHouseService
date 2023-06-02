using Newtonsoft.Json;

namespace DogsHouseService.Model
{
    public class Dog
    {
        public Dog()
        {

        }
        public Dog(string name, string color, int tailLength, int weight)
        {
            Name = name;
            Color = color;
            TailLength = tailLength;
            Weight = weight;
        }
        [JsonIgnore]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("tail_length")]
        public int TailLength { get; set; }

        [JsonProperty("weight")]
        public int Weight { get; set; }
    }
}

