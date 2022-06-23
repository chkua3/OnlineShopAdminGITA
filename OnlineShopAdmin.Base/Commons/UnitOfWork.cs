using System.Data;
using Microsoft.EntityFrameworkCore;
using OnlineShopAdmin.Base.Interfaces;

namespace OnlineShopAdmin.Base.Commons;

public class UnitOfWork : IUnitOfWork
{
    private DbContext _context;

    public UnitOfWork(DbContext context)
    {
        _context = context;
    }

    public ITransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
    {
        return new DbTransaction(_context.Database.BeginTransaction());
    }
    
    public void Add<T>(T entity) where T : class, IDbEntity
    {
        entity.ModifiedDate = DateTime.Now;
        var set = _context.Set<T>();
        set.Add(entity);
    }
    
    public async Task AddAsync<T>(T entity) where T : class, IDbEntity
    {
        entity.ModifiedDate = DateTime.Now;
        var set = _context.Set<T>();
        await set.AddAsync(entity);
    }
    
    public void Attach<T>(T entity) where T : class, IDbEntity
    {
        entity.ModifiedDate = DateTime.Now;
        var set = _context.Set<T>();
        set.Attach(entity);
    }
    
    public void Remove<T>(T entity) where T :class, IDbEntity
    {
        entity.ModifiedDate = DateTime.Now;
        var set = _context.Set<T>();
        set.Remove(entity);
    }
    
    public void RemoveRange<T>(IEnumerable<T> entities) where T : class, IDbEntity
    {
        var dbEntities = entities.ToList();
        dbEntities.ForEach(x => x.ModifiedDate = DateTime.Now);

        var set = _context.Set<T>();
        set.RemoveRange(dbEntities);
    }

    public void Update<T>(T entity) where T : class, IDbEntity
    {
        entity.ModifiedDate = DateTime.Now;
        var set = _context.Set<T>();
        set.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }
    
    public IQueryable<T> Query<T>() where T : class, IDbEntity
    {
        return _context.Set<T>();
    }

    public void Commit()
    {
        _context.SaveChanges();
    }
    
    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }
    
    public void Dispose()
    {
        _context = null;
    }
}