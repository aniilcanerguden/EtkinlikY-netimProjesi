using EtkinlikYönetimProjesi.IRepository.Repository;
using EtkinlikYönetimProjesi.Model;
using Microsoft.EntityFrameworkCore;
using Model;

namespace EtkinlikYönetimProjesi.IRepository
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        private ApplicationDbContext _context;
        public CityRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _context = applicationDbContext;
        }

      
        public City? GetCityById(int? id)
        {
            return _context.Cities.FirstOrDefault(s => s.Id == id);
        }

        public City? GetCityByName(string name)
        {
            return _context.Cities.FirstOrDefault(x=> x.Name == name);  
        }
    }
}
