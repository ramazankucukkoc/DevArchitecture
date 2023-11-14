using Core.Entities;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Concrete.Configurations
{
    public class BaseEntityProjectConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntityProject, IEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.isDeleted).HasDefaultValue(false).IsRequired();
            builder.Property(x => x.CreatedDate);
            builder.Property(x => x.LastUpdatedDate);
            builder.Property(x => x.LastUpdatedUserId);
            builder.Property(x => x.CreatedUserId);
            builder.Property(x => x.Status).HasDefaultValue(true);
        }
    }
}
