// ReSharper disable RedundantUsingDirective
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable RedundantNameQualifier
// ReSharper disable UnusedMember.Global
#pragma warning disable 1591    //  Ignore "Missing XML Comment" warning

using System.CodeDom.Compiler;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sales.DbContext
{
    public partial class ErrorLogBuilder : IEntityTypeConfiguration<ErrorLog>
    {
        public void Configure(EntityTypeBuilder<ErrorLog> entity)
        {
            entity.ToTable("ErrorLog", "dbo");
            entity.HasKey(e => e.ErrorLogID);

            entity.Property(e => e.ErrorLogID)
                .HasColumnName("ErrorLogID")
                .HasColumnType("int")
                .UseIdentityColumn();

            entity.Property(e => e.ErrorTime)
                .HasColumnName("ErrorTime")
                .HasColumnType("datetime")
                .HasDefaultValueSql("getdate()");

            entity.Property(e => e.UserName)
                .HasColumnName("UserName")
                .HasColumnType("nvarchar(128)")
                .IsRequired()
                .HasMaxLength(128);

            entity.Property(e => e.ErrorNumber)
                .HasColumnName("ErrorNumber")
                .HasColumnType("int");

            entity.Property(e => e.ErrorSeverity)
                .HasColumnName("ErrorSeverity")
                .HasColumnType("int");

            entity.Property(e => e.ErrorState)
                .HasColumnName("ErrorState")
                .HasColumnType("int");

            entity.Property(e => e.ErrorProcedure)
                .HasColumnName("ErrorProcedure")
                .HasColumnType("nvarchar(126)")
                .HasMaxLength(126);

            entity.Property(e => e.ErrorLine)
                .HasColumnName("ErrorLine")
                .HasColumnType("int");

            entity.Property(e => e.ErrorMessage)
                .HasColumnName("ErrorMessage")
                .HasColumnType("nvarchar(4000)")
                .IsRequired()
                .HasMaxLength(4000);

        }
    }
}