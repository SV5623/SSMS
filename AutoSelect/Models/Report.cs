using System;

namespace AutoSelect.Models
{
    public class Report
    {
        public int Id { get; set; }

        public int TaskId { get; set; }

        public string Description { get; set; }

        public DateTime? CompletedAt { get; set; }

        public virtual Task Task { get; set; }
    }
}
