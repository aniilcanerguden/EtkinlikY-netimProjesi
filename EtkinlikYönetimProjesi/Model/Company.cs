using System.ComponentModel.DataAnnotations;

namespace EtkinlikYönetimProjesi.Model
{
    public class Company
    {
        [Key]
        public int Id { get; set; } 
        public string? Name { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? EMail { get; set; }
        public string? Password { get; set; }  
        public int? EventId { get; set; }
        public Events Event { get; set; }  
    }
}
