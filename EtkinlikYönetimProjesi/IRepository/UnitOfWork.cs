using EtkinlikYönetimProjesi.IRepository.Repository;
using Model;

namespace EtkinlikYönetimProjesi.IRepository
{
    public class UnitOfWork : IUnitOfWork
    {

        private ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEventRepository Event => new EventRepository(_context);

    public ICompanyRepository Company => new CompanyRepository(_context);

    public ICityRepository City => new CityRepository(_context);


    public ICategoryRepository Category => new CategoryRepository(_context);

    public IUserRepository User => new UserRepository(_context);


       
       

    public void Dispose()
    {
        _context.Dispose();
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}
}
