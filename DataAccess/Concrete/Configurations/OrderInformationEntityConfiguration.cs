using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrete.Configurations
{
    public class OrderInformationEntityConfiguration : IEntityTypeConfiguration<OrderInformation>
    {
        public void Configure(EntityTypeBuilder<OrderInformation> builder)
        {
            builder.Property(x => x.ProductId).IsRequired();
            builder.Property(x => x.CustomerId).IsRequired();
            builder.Property(x => x.Count).IsRequired();

        }
    }
}
