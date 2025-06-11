using System.ComponentModel.DataAnnotations;

namespace AutoSelect.Models
{
    public class Part
    {
        public int Id { get; set; }

        [Required]
        public int TaskId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public virtual Task Task { get; set; }
    }
}
