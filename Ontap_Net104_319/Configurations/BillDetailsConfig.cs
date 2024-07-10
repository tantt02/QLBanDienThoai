using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ontap_Net104_319.Models;

namespace Ontap_Net104_319.Configurations
{
    public class BillDetailsConfig : IEntityTypeConfiguration<BillDetails>
    {
        public void Configure(EntityTypeBuilder<BillDetails> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(p=>p.Bill).WithMany(p=>p.Details).
                HasForeignKey(p=>p.BillId);
            builder.HasOne(p => p.Product).WithMany(p => p.BillDetails).
                HasForeignKey(p => p.ProductId);
        }
    }
}
