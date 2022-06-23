namespace OnlineShopAdmin.Common.API;

public class AppServiceResponse<TResponse> : IResponse<TResponse>
{
    public TResponse Data { get; set; }

    public Status Status { get; set; }
}