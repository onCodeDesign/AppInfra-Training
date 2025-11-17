using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonsManagement.DataModel.Persons;

namespace PersonsManagement.Infrastructure;

public class PersonBuilder : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> entity)
    {
        entity.ToTable("Person", "Persons");
        entity.HasKey(e => e.PersonID);

        entity.Property(e => e.PersonID)
            .HasColumnName("PersonID")
            .UseIdentityColumn();

        entity.Property(e => e.NameStyle)
            .IsRequired()
            .HasDefaultValue(false);

        entity.Property(e => e.Title)
            .HasMaxLength(8);

        entity.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        entity.Property(e => e.MiddleName)
            .HasMaxLength(50);

        entity.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(50);

        entity.Property(e => e.Suffix)
            .HasMaxLength(10);

        entity.Property(e => e.EmailAddress)
            .HasMaxLength(50);

        entity.Property(e => e.Phone)
            .HasMaxLength(25);

        entity.Property(e => e.CompanyName)
            .HasMaxLength(128);

        entity.Property(e => e.PasswordHash)
            .IsRequired()
            .HasMaxLength(128)
            .IsUnicode(false);

        entity.Property(e => e.PasswordSalt)
            .IsRequired()
            .HasMaxLength(10)
            .IsUnicode(false);

        entity.Property(e => e.Rowguid)
            .HasColumnName("rowguid")
            .HasDefaultValueSql("newid()")
            .ValueGeneratedOnAdd();

        entity.Property(e => e.ModifiedDate)
            .HasDefaultValueSql("getdate()");
    }
}
