using System.ComponentModel.DataAnnotations;

namespace EtkinlikYönetimProjesi.Model
{
    public class Events
    {
        
            [Key]
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTime EventDate { get; set; }
            public DateTime ApplicationDeadline { get; set; }
            public string? CityName { get; set; }   
            public string? CategoryName { get; set; }   
            public bool IsApproved { get; set; }
            public bool? IsCancelled { get; set; }
            public int? Capacity { get; set; }
            public int Participants { get; set; }
            public bool? IsJoin { get ; set; }   
            public string? Address { get; set; }
            public int? CategoryId { get; set; }
            public Category Category { get; set; }
            public int? CityId { get; set; }
            public City City { get; set; }
            public int UserId { get; set; }
            public User User { get; set; }
            public string Description { get; set; }
            public bool IsTicketed { get; set; }
            public decimal? EventPayment { get; set; }
            public List<Company> Companies { get; set; }


    }
}
