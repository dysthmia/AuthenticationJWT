namespace Domain;

public class User
{
    public UserId Id { get; private set; }
    public PersonalData PersonalData { get; private set; }
    public DateTime RegisteredAt { get; private set; }
    public bool IsActive { get; private set; }

    private User(
        UserId id,
        PersonalData personalData,
        DateTime registeredAt,
        bool isActive)
    {
        Id = id;
        PersonalData = personalData;
        RegisteredAt = registeredAt;
        IsActive = isActive;
    }

    // регистрация нового пользователя
    public static User Register(
        string firstName,
        string lastName,
        string email)
    {
        UserId userId = UserId.Create();

        PersonalData personalData =
            PersonalData.Create(
                firstName,
                lastName,
                email);

        DateTime registeredAt = DateTime.UtcNow;

        return new User(
            userId,
            personalData,
            registeredAt,
            true);
    }

    // востановление уже существующего пользователя
    public static User Restore(
        UserId id,
        PersonalData personalData,
        DateTime registeredAt,
        bool isActive)
    {
        return new User(
            id,
            personalData,
            registeredAt,
            isActive);
    }
    
    public void Deactivate()
    {
        if (!IsActive) return;
        IsActive = false;
    }

    public void Activate()
    {
        if (IsActive) return;
        IsActive = true;
    }
}