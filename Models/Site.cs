namespace rbauthor.Models
{
    public class Site
    {
        public int SiteId { get; set; }
        public string SiteAddress { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public ICollection<Guard> Guards { get; set; } // Collection navigation property to access the associated Guards
    }
}
