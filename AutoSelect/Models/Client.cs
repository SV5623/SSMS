using System.ComponentModel.DataAnnotations;

namespace AutoSelect.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
