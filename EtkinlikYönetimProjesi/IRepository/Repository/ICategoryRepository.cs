using EtkinlikYönetimProjesi.Model;

namespace EtkinlikYönetimProjesi.IRepository.Repository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Category? GetCategoryById(int? id);
        Category? GetCategoryByName(string name);
    }
}
