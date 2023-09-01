using EtkinlikYönetimProjesi.IRepository.Repository;
using EtkinlikYönetimProjesi.Model;
using Model;

namespace EtkinlikYönetimProjesi.IRepository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public Category? GetCategoryById(int? id)
        {
            return _context.Categories.FirstOrDefault(s => s.Id == id);
        }

        public Category? GetCategoryByName(string name)
        {
            return _context.Categories.FirstOrDefault(x=> x.Name == name);
        }
    }
}
