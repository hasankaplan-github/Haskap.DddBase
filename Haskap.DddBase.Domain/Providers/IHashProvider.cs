namespace Haskap.DddBase.Domain.Providers;
public interface IHashProvider
{
    string HashData(string data, string salt);
    bool IsVerified(string hash, string data, string salt);
}
