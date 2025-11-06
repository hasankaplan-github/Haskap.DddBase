using Haskap.DddBase.Domain.Providers;
using System.Security.Cryptography;
using System.Text;

namespace Haskap.DddBase.Infra.Providers;

public class HashProvider : IHashProvider
{
    public string HashData(string data, string salt)
    {
        var dataAndSaltBytes = Encoding.UTF8.GetBytes(data + salt);
        var hashedDataBytes = SHA256.HashData(dataAndSaltBytes);
        var hashedData = Convert.ToBase64String(hashedDataBytes);

        return hashedData;
    }

    public bool IsVerified(string hash, string data, string salt)
    {
        return HashData(data, salt) == hash;
    }
}
