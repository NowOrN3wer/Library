using Library.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Configurations;

internal sealed class ApiLogConfiguration : IEntityTypeConfiguration<ApiLog>
{
    public void Configure(EntityTypeBuilder<ApiLog> builder)
    {
        builder.Property(p => p.Path).
            HasMaxLength(250);
        builder.Property(p => p.IPAddress).
            HasMaxLength(50);        
        builder.Property(p => p.Method).
            HasMaxLength(100);
        builder.Property(p => p.RequestBody).
            HasColumnType("jsonb");
        builder.Property(p => p.ResponseBody).
            HasColumnType("jsonb");
    }
}