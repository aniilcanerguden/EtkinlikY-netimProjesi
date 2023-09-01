using EtkinlikYönetimProjesi.Validation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace EtkinlikYönetimProjesi.Model
{
    
    public class User
    {
        [Key]
        public int ID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
        public string? PasswordRepeat { get; set; }
        public string UserRole { get; set; } // Kullanıcının rollerini saklamak için koleksiyon
        public ICollection<Events> Event { get; set; }
    }
 
}
