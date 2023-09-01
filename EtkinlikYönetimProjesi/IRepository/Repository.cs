using EtkinlikYönetimProjesi.IRepository.Repository;
using Microsoft.EntityFrameworkCore;
using Model;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;

namespace EtkinlikYönetimProjesi.IRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> _dbset;
        public Repository(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
            _dbset = _context.Set<T>();
        }
        public void Add(T entities)
        {
            _context.Entry(entities).State = EntityState.Added;
            
        }

        public void Delete(T entities)
        {
            _context.Entry(entities).State = EntityState.Deleted;
           
        }



        public T GetById(int id)
        {
            return _dbset.Find(id);

        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = _dbset;
            if (filter != null)
            {
                query = query.Where(filter);

            }

            if (includeProperties != null)
            {

                foreach (var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);

                }

            }
            return query.FirstOrDefault();
        }

        public IQueryable<T> Getlist(Expression<Func<T, bool>> filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = _dbset;
            if (filter != null)
            {
                query = query.Where(filter);

            }

            if (includeProperties != null)
            {

                foreach (var item in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);

                }

            }
            return query;
        }

        public void Update(T entities)
        {
            _context.Entry(entities).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var stringBuilder = new StringBuilder();

            for (int i = 0; i < hashedBytes.Length; i++)
            {
                stringBuilder.Append(hashedBytes[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}
