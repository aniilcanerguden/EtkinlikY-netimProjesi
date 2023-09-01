using EtkinlikYönetimProjesi.Model;

namespace EtkinlikYönetimProjesi.IRepository.Repository
{
    public interface IEventRepository : IRepository<Events>
    {
        Events? GetEventById(int id);
        List<Events> GetEvents();
       
    }
}
