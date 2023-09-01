using EtkinlikYönetimProjesi.Model;

namespace EtkinlikYönetimProjesi.IRepository.Repository
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Company? GetCompanyById(int id);
        List<Company> GetCompanies();
    }
}
