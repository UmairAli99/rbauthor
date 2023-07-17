using rbauthor.Models;

namespace rbauthor.Models
{
    public class Shift
    {
        public int ShiftId { get; set; }
        public ICollection<int> GuardIds { get; set; } // Selected guard IDs
        public ICollection<string> GuardNames { get; set; }
        public ICollection<Guard> Guards { get; set; } // Navigation property for the selected guards
        public string SiteAddress { get; set; } // Changed to string type for SiteAddress

        // Additional shift properties
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Notes { get; set; }
    }
    public class GuardLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

}
