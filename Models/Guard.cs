using System.ComponentModel.DataAnnotations;

namespace rbauthor.Models
{
    public class Guard
    {
        public int guardId { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public Location Location { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        [Phone]
        public string PhoneNo { get; set; }

        public int SiteId { get; set; } // Foreign key referencing the Site entity
        public Site Site { get; set; } // Navigation property to access the associated Site
    }
    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
