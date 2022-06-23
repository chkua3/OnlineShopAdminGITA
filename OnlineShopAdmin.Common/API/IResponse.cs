namespace OnlineShopAdmin.Common.API;

public interface IResponse<TResponse>
{
    TResponse Data { get; set; }

    Status Status { get; set; }
}