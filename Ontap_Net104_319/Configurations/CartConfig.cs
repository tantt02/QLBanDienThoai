using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ontap_Net104_319.Models;

namespace Ontap_Net104_319.Configurations
{
    public class CartConfig : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(p => p.Username);
            builder.HasOne(p => p.Account).WithOne(p => p.Cart).
                HasForeignKey<Cart>(p => p.Username); // Quan hệ 1-1
        }
    }
}
