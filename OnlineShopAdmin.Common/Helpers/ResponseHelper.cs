using OnlineShopAdmin.Common.API;
using OnlineShopAdmin.Common.Enums;
using OnlineShopAdmin.Common.Extensions;

namespace OnlineShopAdmin.Common.Helpers;

public static class ResponseHelper
{
    public static IResponse<TResponse> Fail<TResponse>(StatusCode statusCode = StatusCode.Error, string message = null, TResponse data = null) where TResponse : class
    {
        var result = new AppServiceResponse<TResponse>
        {
            Data = data,
            Status = new Status
            {
                Code = statusCode,
                Message = message ?? statusCode.GetDisplayName()
            }
        };

        return result;
    }

    public static IResponse<EmptyResponse> Fail(StatusCode statusCode = StatusCode.Error, string message = null)
    {
        return Fail<EmptyResponse>(statusCode, message);
    }

    public static IResponse<TResponse> Ok<TResponse>(TResponse data, string message = null) where TResponse : class
    {
        var result = new AppServiceResponse<TResponse>
        {
            Data = data,
            Status = new Status
            {
                Code = StatusCode.Success,
                Message = message ?? StatusCode.Success.GetDisplayName()
            }
        };

        return result;
    }

    public static IResponse<EmptyResponse> Ok(string message = null)
    {
        return Ok(default(EmptyResponse), message);
    }
}