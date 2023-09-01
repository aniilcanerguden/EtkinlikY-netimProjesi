using EtkinlikYönetimProjesi.Model;

namespace EtkinlikYönetimProjesi.Dtos
{
    public class EventListViewModel
    {
        public IQueryable<Events> EventList { get; set; }
    }
}
