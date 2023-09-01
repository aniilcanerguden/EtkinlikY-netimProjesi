using AutoMapper;
using Etkinlik.Dtos;
using EtkinlikYönetimProjesi.Dtos;
using EtkinlikYönetimProjesi.Model;

namespace EtkinlikYönetimProjesi.Mapper
{
    public class Mapper: Profile
    {
        public Mapper()
        {
            CreateMap<Events, EventDto>();      
        }
    }
}
