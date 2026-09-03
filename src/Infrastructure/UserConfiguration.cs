using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        // Primary Key
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                userId => userId.Value,
                value => UserId.From(value))
            .HasColumnName("id")
            .HasMaxLength(17)
            .IsRequired();

        // PersonalData как Value 
        builder.ComplexProperty(
            x => x.PersonalData,
            personalData =>
            {
                personalData.Property(x => x.FirstName)
                    .HasColumnName("first_name")
                    .HasMaxLength(50)
                    .IsRequired();

                personalData.Property(x => x.LastName)
                    .HasColumnName("last_name")
                    .HasMaxLength(50)
                    .IsRequired();

                personalData.Property(x => x.Email)
                    .HasColumnName("email")
                    .HasMaxLength(254)
                    .IsRequired();
            });

        builder.Property(x => x.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(200) 
            .IsRequired();

        // LoginInformation как Value Object
        builder.ComplexProperty(
            x => x.LoginInformation,
            loginInformation =>
            {
                loginInformation.Property(x => x.FailedLoginAttempts)
                    .HasColumnName("failed_login_attempts")
                    .IsRequired();

                loginInformation.Property(x => x.LockoutEnd)
                    .HasColumnName("lockout_end")
                    .IsRequired(false);

                loginInformation.Property(x => x.LastLoginAt)
                    .HasColumnName("last_login_at")
                    .IsRequired(false);
            });

        builder.Property(x => x.RegisteredAt)
            .HasColumnName("registered_at")
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .IsRequired();
    }
}