using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoxRehabilitationAPI.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("FOXConnection", throwIfV1Schema: false)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<ApplicationUser>().Ignore(t => t.UserName);
            modelBuilder.Entity<IdentityUser>()
                .ToTable("FOX_TBL_APPLICATION_USER")
                .Property(t => t.UserName).HasColumnName("USER_NAME");
            modelBuilder.Entity<IdentityUser>()
               .ToTable("FOX_TBL_APPLICATION_USER")
               .Property(t => t.Email).HasColumnName("EMAIL");
            modelBuilder.Entity<IdentityUser>().Ignore(s => s.PhoneNumber)
                                               .Ignore(s => s.PhoneNumberConfirmed)
                                               .Ignore(s => s.EmailConfirmed)
                                               .Ignore(s => s.LockoutEnabled)
                                               .Ignore(s => s.LockoutEndDateUtc)
                                               .Ignore(s => s.AccessFailedCount)
                                               .Ignore(s => s.TwoFactorEnabled);
            modelBuilder.Entity<IdentityUser>();

            modelBuilder.Entity<ApplicationUser>().ToTable("FOX_TBL_APPLICATION_USER");

            modelBuilder.Entity<IdentityRole>().ToTable("FOX_TBL_APPLICATION_ROLE");

            modelBuilder.Entity<IdentityUserClaim>().ToTable("FOX_TBL_APPLICATION_USER_CLAIMS");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("FOX_TBL_APPLICATION_USER_LOGIN");
            modelBuilder.Entity<IdentityUserRole>().ToTable("FOX_TBL_APPLICATION_USER_ROLES");
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
    public class TalkRehabDBContext : IdentityDbContext<ApplicationUser>
    {
        public TalkRehabDBContext()
            : base("TalkRehabConnection", throwIfV1Schema: false)
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUser>()
                .ToTable("FOX_TBL_APPLICATION_USER")
                .Property(t => t.UserName).HasColumnName("USER_NAME");
            modelBuilder.Entity<IdentityUser>()
               .ToTable("FOX_TBL_APPLICATION_USER")
               .Property(t => t.Email).HasColumnName("EMAIL");
            modelBuilder.Entity<IdentityUser>().Ignore(s => s.PhoneNumber)
                                               .Ignore(s => s.PhoneNumberConfirmed)
                                               .Ignore(s => s.EmailConfirmed)
                                               .Ignore(s => s.LockoutEnabled)
                                               .Ignore(s => s.LockoutEndDateUtc)
                                               .Ignore(s => s.AccessFailedCount)
                                               .Ignore(s => s.TwoFactorEnabled);
            modelBuilder.Entity<IdentityUser>();
            modelBuilder.Entity<ApplicationUser>().ToTable("FOX_TBL_APPLICATION_USER");
            modelBuilder.Entity<IdentityRole>().ToTable("FOX_TBL_APPLICATION_ROLE");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("FOX_TBL_APPLICATION_USER_CLAIMS");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("FOX_TBL_APPLICATION_USER_LOGIN");
            modelBuilder.Entity<IdentityUserRole>().ToTable("FOX_TBL_APPLICATION_USER_ROLES");
        }

        public static TalkRehabDBContext Create()
        {
            return new TalkRehabDBContext();
        }
    }
}