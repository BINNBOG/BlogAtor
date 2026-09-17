namespace BlogAtor.Store;

using BlogAtor.Core;
using BlogAtor.Store.Sql.Data;

using Microsoft.EntityFrameworkCore;

public class BlogContext : DbContext
{
    private readonly IStartupConfigProvider _startupConfigProvider;
    public BlogContext(IStartupConfigProvider startupConfigProvider)
    {
        _startupConfigProvider = startupConfigProvider;
    }
    internal DbSet<DbConfig> Configs { get; set; }

    internal DbSet<DbUser> Users { get; set; }
    internal DbSet<DbUserRole> UserRoles { get; set; }

    internal DbSet<DbDataSource> DataSources { get; set; }
    internal DbSet<DbSourceCollector> SourceCollectors { get; set; }
    public DbSet<DbDataItem> DataItems { get; set; }
    internal DbSet<DbDataContent> DataContents { get; set; }
    internal DbSet<ET> GetDbSet<ET>() where ET : class
    {
        if (typeof(ET) == typeof(DbDataSource))
        {
            return DataSources as DbSet<ET> ?? throw new NotImplementedException();
        }
        else if (typeof(ET) == typeof(DbSourceCollector))
        {
            return SourceCollectors as DbSet<ET> ?? throw new NotImplementedException();
        }
        else if (typeof(ET) == typeof(DbDataItem))
        {
            return DataItems as DbSet<ET> ?? throw new NotImplementedException();
        }
        else if (typeof(ET) == typeof(DbDataContent))
        {
            return DataContents as DbSet<ET> ?? throw new NotImplementedException();
        }
        else if (typeof(ET) == typeof(DbUser))
        {
            return Users as DbSet<ET> ?? throw new NotImplementedException();
        }
        throw new NotImplementedException();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseNpgsql(_startupConfigProvider.StartupConfig.DbConnection);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DbUserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.Roles)
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DbDataItem>()
            .HasOne(di => di.DataSource)
            .WithMany(ds => ds.DataItems)
            .HasForeignKey(di => di.DataSourceId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<DbDataItem>()
            .HasOne(di => di.Collector)
            .WithMany(c => c.DataItems)
            .HasForeignKey(di => di.CollectorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DbSourceCollector>()
            .HasOne(c => c.DataSource)
            .WithMany(ds => ds.Collectors)
            .HasForeignKey(c => c.DataSourceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DbDataContent>()
            .HasOne(dc => dc.DataItem)
            .WithMany(di => di.Contents)
            .HasForeignKey(dc => dc.DataItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}