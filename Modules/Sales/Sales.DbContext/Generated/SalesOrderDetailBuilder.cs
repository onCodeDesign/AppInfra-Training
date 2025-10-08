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
using Sales.DataModel.SalesLT;

namespace Sales.DbContext
{
    public partial class SalesOrderDetailBuilder : IEntityTypeConfiguration<SalesOrderDetail>
    {
        public void Configure(EntityTypeBuilder<SalesOrderDetail> entity)
        {
            entity.ToTable("SalesOrderDetail", "SalesLT", tb => tb.HasTrigger("iduSalesOrderDetail"));
            entity.HasKey(e => new { e.SalesOrderID, e.SalesOrderDetailID });

            entity.Property(e => e.SalesOrderDetailID)
                .HasColumnName("SalesOrderDetailID")
                .HasColumnType("int")
                .UseIdentityColumn();

            entity.Property(e => e.OrderQty)
                .HasColumnName("OrderQty")
                .HasColumnType("smallint");

            entity.Property(e => e.UnitPrice)
                .HasColumnName("UnitPrice")
                .HasColumnType("money");

            entity.Property(e => e.UnitPriceDiscount)
                .HasColumnName("UnitPriceDiscount")
                .HasColumnType("money")
                .HasDefaultValueSql("0.0");

            entity.Property(e => e.LineTotal)
                .HasColumnName("LineTotal")
                .HasColumnType("numeric(38, 6)")
                .HasComputedColumnSql("(isnull(([UnitPrice]*((1.0)-[UnitPriceDiscount]))*[OrderQty],(0.0)))");

            entity.Property(e => e.Rowguid)
                .HasColumnName("rowguid")
                .HasColumnType("uniqueidentifier")
                .HasDefaultValueSql("newid()")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.ModifiedDate)
                .HasColumnName("ModifiedDate")
                .HasColumnType("datetime")
                .HasDefaultValueSql("getdate()");

            entity.HasOne(e => e.Product)
                .WithMany(p => p.SalesOrderDetails)
                .HasForeignKey(p => p.ProductID)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_SalesOrderDetail_Product_ProductID");

            entity.HasOne(e => e.SalesOrderHeader)
                .WithMany(p => p.SalesOrderDetails)
                .HasForeignKey(p => p.SalesOrderID)
                .OnDelete(DeleteBehavior.ClientCascade)
                .HasConstraintName("FK_SalesOrderDetail_SalesOrderHeader_SalesOrderID");

        }
    }
}