using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ontap_Net104_319.Models;

namespace Ontap_Net104_319.Configurations
{
    public class AccountConfig : IEntityTypeConfiguration<Account>
    {
        // FluentAPI cho phép chúng ta cấu hình đè lên các ràng buộc cơ bản và DataAnotation
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            // Cấu hình cho bảng tương ưng với model
            builder.HasKey(p => p.Username);
            // Cấu hình cho thuộc tính
            builder.Property(p => p.Password).HasColumnName("MatKhau").HasColumnType("varchar(256)");
            builder.Property(p => p.Address).IsUnicode(true).IsFixedLength(true).HasMaxLength(256);
            // IsUnicode(true).IsFixedLength(true).HasMaxLength(256) tương đương với nvarchar(256)
        }
    }
}
