using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using DAL.Database.Repositories.Base;
using MIDCloud.GlobalInterfaces.Models;

namespace DAL.Database.Repositories
{
    internal class Repository<T> : IRepository<T>
        where T : Entity, new()
    {
        private readonly DbSet<T> _set;
        private readonly AppDbContext _context;

        public virtual IQueryable<T> Items => _set;

        public Repository(AppDbContext context)
        {
            _context = context;
            _set = _context.Set<T>();
        }

        public T Add(T item)
        {
            Guard.Against.Null(item, nameof(item));

            _context.Entry(item).State = EntityState.Added;
            _context.SaveChanges();

            return item;
        }

        public async Task<T> AddAsync(T item, CancellationToken Cancel = default)
        {
            Guard.Against.Null(item, nameof(item));

            _context.Entry(item).State = EntityState.Added;
            await _context.SaveChangesAsync(Cancel).ConfigureAwait(false);
            return item;
        }

        public T Get(int id)
        {
            return Items.SingleOrDefault(item => item.Id == id);
        }

        public async Task<T> GetAsync(int id, CancellationToken Cancel = default)
        {
            return await Items.SingleOrDefaultAsync(item => item.Id == id, Cancel).ConfigureAwait(false);
        }

        public void Remove(int id)
        {
            var item = _set.Local.FirstOrDefault(i => i.Id == id) ?? new T { Id = id };

            _context.Remove(item);

            _context.SaveChanges();
        }

        public async Task RemoveAsync(int id, CancellationToken Cancel = default)
        {
            _context.Remove(new T { Id = id });
            await _context.SaveChangesAsync(Cancel).ConfigureAwait(false);
        }

        public void Update(T item)
        {
            Guard.Against.Null(item, nameof(item));

            _context.Entry(item).State = EntityState.Modified;

            _context.SaveChanges();
        }

        public async Task UpdateAsync(T item, CancellationToken Cancel = default)
        {
            Guard.Against.Null(item, nameof(item));

            _context.Entry(item).State = EntityState.Modified;

            await _context.SaveChangesAsync(Cancel).ConfigureAwait(false);
        }
    }
}
