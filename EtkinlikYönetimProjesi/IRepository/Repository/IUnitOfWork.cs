namespace EtkinlikYönetimProjesi.IRepository.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository User { get; }
        IEventRepository Event { get; }

        ICategoryRepository Category { get; }
        ICompanyRepository Company { get; }
        ICityRepository City { get; }
  
        void Save();
    }
}
