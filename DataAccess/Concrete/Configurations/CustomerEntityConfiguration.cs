using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrete.Configurations
{
    public class CustomerEntityConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Code).IsRequired().HasMaxLength(50);
            builder.Property(x => x.MobilePhones).IsRequired().HasMaxLength(30);
            builder.Property(x => x.Email).HasMaxLength(50);
            builder.Property(x => x.Address).HasMaxLength(200);

        }
    }
}
