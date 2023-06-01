using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DB.Model
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
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public int TailLength { get; set; }
        public int Weight { get; set; }
    }
}

