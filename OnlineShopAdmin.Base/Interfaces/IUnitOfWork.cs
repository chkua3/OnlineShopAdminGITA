using System.Data;

namespace OnlineShopAdmin.Base.Interfaces;

public interface IUnitOfWork : IDisposable
{
    ITransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);

    void Add<T>(T entity) where T : class, IDbEntity;
    
    Task AddAsync<T>(T entity) where T : class, IDbEntity;
    
    void Update<T>(T entity) where T : class, IDbEntity;
    
    void Remove<T>(T entity) where T : class, IDbEntity;

    void RemoveRange<T>(IEnumerable<T> entities) where T : class, IDbEntity;

    void Attach<T>(T entity) where T : class, IDbEntity;
    
    IQueryable<T> Query<T>() where T : class, IDbEntity;

    void Commit();

    Task CommitAsync();
}