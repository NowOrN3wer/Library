using GenericRepository;
using Library.Domain.Abstractions;
using Library.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;

namespace Library.Infrastructure.Context;

public sealed class ApplicationDbContext
    : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>, IUnitOfWork
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public DbSet<Writer> Writers { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Publisher> Publishers { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Auto-apply IEntityTypeConfiguration<TEntity> implementations
        builder.ApplyConfigurationsFromAssembly(typeof(DependencyInjection).Assembly);

        // Ignore Identity tables you don't need
        builder.Ignore<IdentityUserLogin<Guid>>();
        builder.Ignore<IdentityRoleClaim<Guid>>();
        builder.Ignore<IdentityUserToken<Guid>>();
        builder.Ignore<IdentityUserRole<Guid>>();
        builder.Ignore<IdentityUserClaim<Guid>>();

        // Apply CreatedAt / UpdatedAt to all entities inheriting from Entity base
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (typeof(Entity).IsAssignableFrom(entityType.ClrType))
            {
                builder.Entity(entityType.ClrType).Property(nameof(Entity.CreatedAt))
                       .HasColumnType("timestamptz")
                       .IsRequired();

                builder.Entity(entityType.ClrType).Property(nameof(Entity.UpdatedAt))
                       .HasColumnType("timestamptz");
            }
        }

        // Book → Writer (many-to-one)
        builder.Entity<Book>()
            .HasOne(b => b.Writer)
            .WithMany(w => w.Books)
            .HasForeignKey(b => b.WriterId)
            .OnDelete(DeleteBehavior.Restrict);

        // Book → Category (many-to-one)
        builder.Entity<Book>()
            .HasOne(b => b.Category)
            .WithMany(c => c.Books)
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Book → Publisher (many-to-one)
        builder.Entity<Book>()
            .HasOne(b => b.Publisher)
            .WithMany(p => p.Books)
            .HasForeignKey(b => b.PublisherId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    public override int SaveChanges()
    {
        UpdateVersionAndTimestamps();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateVersionAndTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateVersionAndTimestamps()
    {
        var userName =
            _httpContextAccessor.HttpContext?.User?.Identity?.Name
            ?? _httpContextAccessor.HttpContext?.User?.FindFirst("UserName")?.Value
            ?? "System";

        var entries = ChangeTracker.Entries<Entity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.Version += 1;
                entry.Entity.UpdatedAt = GetTurkeyTime().ToUniversalTime();
                entry.Entity.UpdatedBy = userName;
            }
            else if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = GetTurkeyTime().ToUniversalTime();
                entry.Entity.CreatedBy = userName;
            }
        }
    }
    
    private static DateTimeOffset GetTurkeyTime()
    {
        var timeZoneId = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? "Turkey Standard Time"
            : "Europe/Istanbul";

        var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        return TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, tz);
    }

}
