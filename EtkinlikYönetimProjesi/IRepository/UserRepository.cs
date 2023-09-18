using EtkinlikYönetimProjesi.IRepository.Repository;
using EtkinlikYönetimProjesi.Model;
using Microsoft.EntityFrameworkCore;
using Model;

namespace EtkinlikYönetimProjesi.IRepository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public void AdminControl(User user, string HashPassword)
        {
            
             user.Password = HashPassword;
             user.PasswordRepeat = HashPassword;
             _context.SaveChanges();
            
           
        }

        public User? GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(s => s.ID == id);
        }

        public User? GetUserByRole(string name)
        {
            return _context.Users.FirstOrDefault(s => s.UserRole == name); 
        }
    }
}
