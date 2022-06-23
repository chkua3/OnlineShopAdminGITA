namespace OnlineShopAdmin.Base.Interfaces;

public interface ITransaction : IDisposable
{
    void Commit();
    void Rollback();
}