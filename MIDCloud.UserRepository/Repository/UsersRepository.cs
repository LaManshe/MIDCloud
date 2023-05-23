using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;
using MIDCloud.Shared.Models.Interfaces.Users;
using MIDCloud.UserRepository.DB;
using MIDCloud.UserRepository.Models;
using MIDCloud.UserRepository.Repository.Interfaces;

namespace MIDCloud.UserRepository.Repository;

public class UsersRepository : IRepository<IUser>
{
    private readonly DbSet<User> _set;
    
    private readonly AppDbContext _context;

    public IQueryable<IUser> Items => _set;

    public UsersRepository(AppDbContext context)
    {
        _context = context;
        
        _set = _context.Set<User>();
    }

    public IUser? Get(int id)
    {
        return Items.SingleOrDefault(item => item.Id == id);
    }

    public Task<IUser> GetAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IUser Add(IUser item)
    {
        Guard.Against.Null(item, nameof(item));

        _context.Entry(item).State = EntityState.Added;
        _context.SaveChanges();

        return item;
    }

    public Task<IUser> AddAsync(IUser item, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Update(IUser item)
    {
        Guard.Against.Null(item, nameof(item));

        _context.Entry(item).State = EntityState.Modified;

        _context.SaveChanges();
    }

    public Task UpdateAsync(IUser item, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public void Remove(int id)
    {
        var item = _set.Local.FirstOrDefault(i => i.Id == id);

        if (item is null)
        {
            return;
        }

        _context.Remove(item);

        _context.SaveChanges();
    }

    public Task RemoveAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}