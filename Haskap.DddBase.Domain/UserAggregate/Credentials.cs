using Ardalis.GuardClauses;

namespace Haskap.DddBase.Domain.UserAggregate;
public class Credentials : ValueObject
{
    public string UserName { get; private set; }
    public Password Password { get; private set; }

    private Credentials()
    {
    }

    public Credentials(string userName, Password password)
    {
        Guard.Against.NullOrWhiteSpace(userName, nameof(userName), "Kullanıcı Adı boş olamaz!");
        Guard.Against.Null(password);
        var hashedUserName = Password.ComputeHash(userName, password.Salt);
        Guard.Against.InvalidInput(hashedUserName, nameof(password), x => x != password.HashedValue, "Kullanıcı Adı ve Şifre birbirine eşit olamaz!");
        

        UserName = userName;
        Password = password;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return UserName;
        yield return Password;
    }
}
