using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrete.Configurations
{
    public class WarehouseInformationEntityConfiguration : IEntityTypeConfiguration<WarehouseInformation>
    {
        public void Configure(EntityTypeBuilder<WarehouseInformation> builder)
        {
            builder.Property(x => x.ProductId).IsRequired();
            builder.Property(x => x.ReadyForSale).HasDefaultValue(true).IsRequired();
            builder.Property(x => x.Count).IsRequired();
        }
    }
}
