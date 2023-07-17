using System.ComponentModel.DataAnnotations;

namespace rbauthor.Models
{
    public class Contacts
    {
        [Required]
        [StringLength(30, ErrorMessage = "Character limit exceeding (maximum characters should be 30)")]
        public string name { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        public string subject { get; set; }
        [Required]
        public string message { get; set; }
    }
}
