using DistantStars.Server.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DistantStars.Server.DBContext
{
    public class EFCoreContext : DbContext
    {
        public EFCoreContext(DbContextOptions<EFCoreContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //var valueConverter = new ValueConverter<string, string>(
            //    v => string.IsNullOrWhiteSpace(v) ? null : ((int)v.ToCharArray()[0]).ToString("x"),

            //    v => string.IsNullOrWhiteSpace(v) ? string.Empty : ((char)int.Parse(v, NumberStyles.HexNumber)).ToString());
            //modelBuilder.Entity<MenuInfo>().Property(p => p.MenuIcon).HasConversion(valueConverter);
            modelBuilder.Entity<RoleMenu>().HasKey(r => new { r.RoleId, r.MenuId });
        }

        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<RoleInfo> RoleInfo { get; set; }
        public DbSet<RoleMenu> RoleMenu { get; set; }
        public DbSet<MenuInfo> MenuInfo { get; set; }
        public DbSet<FileInfoModel> FileInfoModel { get; set; }


        public static readonly ILoggerFactory ConsoleLoggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddFilter((category, level) =>
                category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information
            ).AddConsole();
        });
    }
}
