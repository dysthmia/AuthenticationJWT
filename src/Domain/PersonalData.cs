using System.Net.Mail;

namespace Domain;

public readonly partial record struct PersonalData
{
    private const int MinNameLength = 2;
    private const int MaxNameLength = 50;
    private const int MaxEmailLength = 254;

    public string FirstName {get;}
    public string LastName {get;}
    public string Email {get;}

    private PersonalData (string firstName, 
                        string lastName, 
                        string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public static PersonalData Create(string firstName, 
                        string lastName, 
                        string email)
    {
        ValidateName(firstName,lastName);
        ValidateEmail(email);
        return new PersonalData(firstName.Trim(),lastName.Trim(),email.Trim());
    }
    
    private static void ValidateName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName) ||
            firstName.Length < MinNameLength ||
            firstName.Length > MaxNameLength)
        {
            throw new ArgumentException("wrong first name");    
        }
        if (string.IsNullOrWhiteSpace(lastName) ||
            lastName.Length < MinNameLength ||
            lastName.Length > MaxNameLength)
        {
            throw new ArgumentException("wrong last name");    
        }
    }
    private static  void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email) ||
            email.Length>MaxEmailLength)
            throw new ArgumentException("wromg email");

        var address = new MailAddress(email);
        if (address.Address != email)
            throw new ArgumentException("wromg email");        
    }

    public override string ToString() => $"{FirstName} {LastName}";
}
