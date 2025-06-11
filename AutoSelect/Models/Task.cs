namespace AutoSelect.Models
{
    public class Task
    {
        public int Id { get; set; }
        public int MechanicId { get; set; }
        public Mechanic Mechanic { get; set; }  // навігаційна властивість

        public int CarId { get; set; }
        public Car Car { get; set; }  // навігаційна властивість

        public string Description { get; set; }
        public string Status { get; set; } // Pending, InProgress, Completed
        public DateTime CreatedAt { get; set; }
    }
}