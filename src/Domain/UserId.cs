using System.Security.Cryptography;
using System.Text;

namespace Domain;

public readonly partial record struct UserId
{
    private const char Symbol = '#';

    private const int MaxLength = 17;
    private const int LettersLength = 16;
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyz";

    public string Value {get;}
    private UserId(string value)
    {
        Value = value;
    }

    public static UserId Create()
    {
        var sb = new StringBuilder();
        for (int i=0;i<LettersLength; i++)
        {
            int rndmIndex = RandomNumberGenerator.GetInt32(Alphabet.Length);
            sb.Append(Alphabet[rndmIndex]);
        }
        string value = "#" + sb.ToString();
        return new UserId(value);
    }
    public static UserId From(string value)
    {
        ValidateValue(value);
        return new UserId(value);
    }

    private static void ValidateValue(string value)
    {
        if  (string.IsNullOrWhiteSpace(value) || 
            value.Length!=MaxLength ||
            value[0]!=Symbol)
            throw new ArgumentException("wrong user id");

        for (int i=1;i<value.Length; i++)
        {
            if (!Alphabet.Contains(value[i]))
                throw new ArgumentException("wrong user id");
        }
    }

    public override string ToString() => Value;
}