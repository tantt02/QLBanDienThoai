using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ontap_Net104_319.Models;

namespace Ontap_Net104_319.Configurations
{
    public class CartDetailsConfig : IEntityTypeConfiguration<CartDetails>
    {
        public void Configure(EntityTypeBuilder<CartDetails> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(p => p.Cart).WithMany(p => p.Details).
                HasForeignKey(p => p.Username);
            builder.HasOne(p => p.Product).WithMany(p => p.CartDetails).
                HasForeignKey(p => p.ProductId);
        }
    }
}
