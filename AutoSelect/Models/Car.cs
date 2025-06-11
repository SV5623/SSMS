using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoSelect.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required, StringLength(100)]
        public string Model { get; set; }

        [Required, StringLength(20)]
        public string LicensePlate { get; set; }

        public double LocationLat { get; set; }
        public double LocationLng { get; set; }

        // Навігаційна властивість
        public virtual Client Client { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
