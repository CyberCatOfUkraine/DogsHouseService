using DB.Model;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DB.Repository
{
    public class DogRepository : IRepository<Dog>
    {
        public DogRepository(DbSet<Dog> dogs, DbContext context)
        {
            _dogs = dogs;
            _context = context;
        }

        private DbSet<Dog> _dogs { get; }
        public DbContext _context { get; }

        public async void Add(Dog entity)
        {
            await _dogs.AddAsync(entity);
            _context.SaveChanges();
        }

        public async void AddRange(IEnumerable<Dog> entities)
        {
            await _dogs.AddRangeAsync(entities);
            _context.SaveChangesAsync().Wait();
        }

        public IEnumerable<Dog> Find(Expression<Func<Dog, bool>> predicate)
        {
            var d = async delegate ()
            {
                return await Task.Run(() => { return _dogs.Where(predicate).ToList(); });
            };
            return d.Invoke().Result;
        }
        public IEnumerable<Dog> GetAll()
        {
            return _dogs.ToList();
        }
        public void Remove(Dog entity)
        {
            _dogs.Remove(entity);
            _context.SaveChanges();
        }
        public void RemoveRange(IEnumerable<Dog> entities)
        {
            _dogs.RemoveRange(entities);
            _context.SaveChanges();
        }
        public void Update(Dog entity)
        {
            _dogs.Update(entity);
            _context.SaveChanges();
        }
    }
}
