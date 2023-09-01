using EtkinlikYönetimProjesi.IRepository.Repository;
using EtkinlikYönetimProjesi.Model;
using Model;

namespace EtkinlikYönetimProjesi.IRepository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private ApplicationDbContext _context;
        public CompanyRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _context = applicationDbContext;
        }
   
        public Company? GetCompanyById(int id)
        {
            return _context.Companies.FirstOrDefault(s => s.Id == id);
        }

        public List<Company> GetCompanies()
        {
            return _context.Companies.ToList();
        }
    }
}
