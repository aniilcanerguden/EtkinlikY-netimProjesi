using EtkinlikYönetimProjesi.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EtkinlikYönetimProjesi.Settings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(new User
            {

                ID = 12,
                FirstName = "Admin",
                LastName = "Admin",
                Email = "admin@gmail.com",
                Password = "admin1234",
                PasswordRepeat = "admin1234",
                UserRole = "Admin"
               
            });          
        }
       
    }
}
