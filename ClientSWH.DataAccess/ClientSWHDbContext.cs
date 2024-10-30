using ClientSWH.DataAccess.Configurations;
using ClientSWH.DataAccess.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;
using System.Diagnostics;




namespace ClientSWH.DataAccess
{
    public class ClientSWHDbContext(DbContextOptions<ClientSWHDbContext> options)
        : DbContext(options)
    {
        public DbSet<UserEntity> Users { get; set; } = null!;
        public DbSet<PackageEntity> Packages { get; set; }
        public DbSet<DocumentEntity> Document { get; set; }
        public DbSet<StatusEntity> Status { get; set; }
        public DbSet<HistoryPkgEntity> HistoryPkg { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new PackageConfiguration());
            modelBuilder.ApplyConfiguration(new DocumentConfiguration());
            modelBuilder.ApplyConfiguration(new StatusConfiguration());
            modelBuilder.ApplyConfiguration(new HistoryPkgConfiguration());

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClientSWHDbContext).Assembly);

            base.OnModelCreating(modelBuilder);

        }
        
       
    }

    //public class MyAppDbContextFactory : IDesignTimeDbContextFactory<ClientSWHDbContext>
    //{
    //    public ClientSWHDbContext CreateDbContext(string[] args)
    //    {
    //        var optionsBuilder = new DbContextOptionsBuilder<ClientSWHDbContext>();
    //        optionsBuilder.UseNpgsql("Host=localhost;User ID=postgres;Password=studadmin;Port=5432;Database=svhdb;");
    //        var b = optionsBuilder.Options;

    //        return new ClientSWHDbContext(b);
    //    }
    //}
}
