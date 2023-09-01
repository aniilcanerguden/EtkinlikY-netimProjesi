using EtkinlikYönetimProjesi.IRepository.Repository;
using EtkinlikYönetimProjesi.Model;
using Model;

namespace EtkinlikYönetimProjesi.IRepository
{
    public class EventRepository : Repository<Events>, IEventRepository
    {
        private ApplicationDbContext _context;
        public EventRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public Events? GetEventById(int id)
        {
            return _context.Events.FirstOrDefault(s => s.Id == id);
        }

        public List<Events> GetEvents()
        {
            return _context.Events.ToList();
        }
    }
}
