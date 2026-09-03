namespace Domain;

public readonly record struct LoginInformation
{
    private const int LockDurationMinutes = 10;
    private const int MaxFailedAttempts = 3;

    public int FailedLoginAttempts { get; }
    public DateTimeOffset? LockoutEnd { get; }
    public DateTimeOffset? LastLoginAt { get; }

    private LoginInformation(
        int failedLoginAttempts,
        DateTimeOffset? lockoutEnd,
        DateTimeOffset? lastLoginAt)
    {
        FailedLoginAttempts = failedLoginAttempts;
        LockoutEnd = lockoutEnd;
        LastLoginAt = lastLoginAt;
    }

    // состояние нового пользователя - ни одной попытки входа ещё не было
    public static LoginInformation Initial() => new(0, null, null);

    // для восстановления состояния из БД 
    public static LoginInformation Restore(
        int failedLoginAttempts,
        DateTimeOffset? lockoutEnd,
        DateTimeOffset? lastLoginAt) =>
        new(failedLoginAttempts, lockoutEnd, lastLoginAt);

    public bool IsLockedOut =>
        LockoutEnd is not null && LockoutEnd > DateTimeOffset.UtcNow;

    public LoginInformation RegisterFailedLogin()
    {
        var attempts = FailedLoginAttempts + 1;

        DateTimeOffset? lockoutEnd = LockoutEnd;
        if (attempts >= MaxFailedAttempts)
        {
            lockoutEnd = DateTimeOffset.UtcNow.AddMinutes(LockDurationMinutes);
        }

        return new LoginInformation(attempts, lockoutEnd, LastLoginAt);
    }

    // успешный вход сбрасывает счётчик и блокировку
    public LoginInformation RegisterSuccessfulLogin() =>
        new(0, null, DateTimeOffset.UtcNow);
}