namespace Domain;

public class User
{
    public UserId Id { get; private set; }
    public PersonalData PersonalData { get; private set; }
    public LoginInformation LoginInformation { get; private set; }
    public DateTime RegisteredAt { get; private set; }
    public bool IsActive { get; private set; }

    // хеш пароля считается снаружи через IPasswordHasher
    public string PasswordHash { get; private set; }

    private User(
        UserId id,
        PersonalData personalData,
        string passwordHash,
        LoginInformation loginInformation,
        DateTime registeredAt,
        bool isActive)
    {
        Id = id;
        PersonalData = personalData;
        PasswordHash = passwordHash;
        LoginInformation = loginInformation;
        RegisteredAt = registeredAt;
        IsActive = isActive;
    }

    // регистрация нового пользователя.
    // passwordHash - уже посчитанный хеш 
    public static User Register(
        string firstName,
        string lastName,
        string email,
        string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("password hash is required");

        UserId userId = UserId.Create();

        PersonalData personalData =
            PersonalData.Create(firstName, lastName, email);

        return new User(
            userId,
            personalData,
            passwordHash,
            LoginInformation.Initial(),
            DateTime.UtcNow,
            isActive: true);
    }

    // восстановление уже существующего пользователя (маппинг из БД)
    public static User Restore(
        UserId id,
        PersonalData personalData,
        string passwordHash,
        LoginInformation loginInformation,
        DateTime registeredAt,
        bool isActive)
    {
        return new User(
            id,
            personalData,
            passwordHash,
            loginInformation,
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

    public bool IsLockedOut() => LoginInformation.IsLockedOut;

    // вызывается из AuthService после неудачной проверки пароля
    public void RegisterFailedLogin() =>
        LoginInformation = LoginInformation.RegisterFailedLogin();

    // вызывается из AuthService после успешного логина
    public void RegisterSuccessfulLogin() =>
        LoginInformation = LoginInformation.RegisterSuccessfulLogin();

    public void ChangePasswordHash(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new ArgumentException("password hash is required");
        PasswordHash = newPasswordHash;
    }
}