using System.Linq.Expressions;

namespace EtkinlikYönetimProjesi.IRepository.Repository
{
    public interface IRepository<T> where T : class
    {

        void Add(T entities);
        void Update(T entities);
        void Delete(T entities);
        T GetById(int id);
        IQueryable<T> Getlist(Expression<Func<T, bool>> filter = null, string? includeProperties = null);
        T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);
        string HashPassword(string password);
    }
}
