using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AutoSelect.Models
{
    public class Mechanic
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        public bool IsAvailable { get; set; }

        [JsonIgnore]
        public List<Task> Tasks { get; set; }
    }
}
