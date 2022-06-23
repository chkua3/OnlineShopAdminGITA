using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace OnlineShopAdmin.Common.Extensions;

public static class CommonExtensions
{
    [Obsolete("Obsolete")]
    public static string CreateSalt(int size)
    {
        //Generate a cryptographic random number.
        var rng = new RNGCryptoServiceProvider();
        var buff = new byte[size];
        rng.GetBytes(buff);
        return Convert.ToBase64String(buff);
    }

    [Obsolete("Obsolete")]
    public static string GenerateHash(string input, string salt)
    {
        var bytes = Encoding.UTF8.GetBytes(input + salt);
        var sHa256ManagedString = new SHA256Managed();
        var hash = sHa256ManagedString.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

    public static string GetDisplayName(this Enum enumValue)
    {
        return enumValue.GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<DisplayAttribute>()
            ?.GetName();
    }
}