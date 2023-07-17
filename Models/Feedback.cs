namespace rbauthor.Models
{
    public class Feedback
    {
        public int GuardId { get; set; }
        public string GuardName { get; set; }
        public int Stars { get; set; }
        public string Comment { get; set; }
        public string ClientName { get; set; }
        public DateTime Date { get; set; }
        public string SiteAddress { get; set; }
    }

}
