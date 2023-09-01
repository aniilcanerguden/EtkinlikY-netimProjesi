using EtkinlikYönetimProjesi.Model;

namespace EtkinlikYönetimProjesi.IRepository.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        User? GetUserById(int id);
    }
}
