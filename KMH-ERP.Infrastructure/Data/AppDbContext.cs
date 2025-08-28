using KMH_ERP.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace KMH_ERP.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Menu> Menus => Set<Menu>();
        public DbSet<UserMenuRight> UserMenuRights => Set<UserMenuRight>();
        public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();
        public DbSet<JwtLoginToken> JwtLoginTokens => Set<JwtLoginToken>();
        public DbSet<LoginActivity> LoginActivities => Set<LoginActivity>();

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<User>().ToTable("tbl_User").HasKey(u => u.UserId);
            model.Entity<Role>().ToTable("tbl_Role").HasKey(r => r.RoleId);
            model.Entity<Menu>().ToTable("tbl_Menu").HasKey(m => m.MenuId);
            model.Entity<UserMenuRight>().ToTable("tbl_UserMenuRights").HasKey(umr => umr.MenuRightId);
            model.Entity<ActivityLog>().ToTable("tbl_ActivityLog").HasKey(al => al.ActivityLogId);
            model.Entity<JwtLoginToken>().ToTable("tbl_JwtLoginToken").HasKey(jt => jt.TokenId);
            model.Entity<LoginActivity>().ToTable("tbl_LoginActivities").HasKey(la => la.LoginActivityId);

            model.Entity<Menu>()
           .HasOne(m => m.ParentMenu)
           .WithMany(m => m.SubMenus)
           .HasForeignKey(m => m.ParentMenuId);

            model.Entity<User>()
           .HasOne(u => u.Role)
           .WithMany(r => r.Users)
           .HasForeignKey(u => u.RoleId)
           .OnDelete(DeleteBehavior.Restrict);




            base.OnModelCreating(model);
        }

    }
}
