using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etkinlik.Dtos
{
    public class EventDto
    {
       
        public string Name { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime ApplicationDeadline { get; set; }
        public bool IsApproved { get; set; }
        public int Capacity { get; set; }
        public string? Address { get; set; }
        public int CategoryId { get; set; }      
        public int CityId { get; set; }        
        public string? Organizer { get; set; }
        public int? User { get; set; }
        public string? Description { get; set; }
        public bool IsTicketed { get; set; }
        public decimal EventPayment { get; set; }
        public string? CityName { get; set; }
        public string? CategoryName { get; set; }
    }
}
