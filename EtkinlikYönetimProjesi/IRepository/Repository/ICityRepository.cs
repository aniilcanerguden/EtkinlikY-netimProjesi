using EtkinlikYönetimProjesi.Model;

namespace EtkinlikYönetimProjesi.IRepository.Repository
{
    public interface ICityRepository : IRepository<City>
    {
        City? GetCityById(int? id);
        City? GetCityByName(string name);
    }
}
