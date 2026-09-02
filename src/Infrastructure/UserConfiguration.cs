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

        builder.Property(x => x.RegisteredAt)
            .HasColumnName("registered_at")
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .IsRequired();
    }
}