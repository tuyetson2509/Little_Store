using UserService.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using static UserService.Model.UserServiceModel;

namespace UserService.DBContext
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
        }

        public DbSet<TaiKhoan> TaiKhoans { get; set; }
        public DbSet<PhanQuyen> PhanQuyens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<TaiKhoan>().ToTable("TaiKhoan");
            modelBuilder.Entity<PhanQuyen>().ToTable("PhanQuyen");

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.Email)
                .IsUnicode(false);
            modelBuilder.Entity<TaiKhoan>()
            .HasKey(t => t.MaNguoiDung);
            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.Dienthoai)
                .IsUnicode(false);

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.Matkhau)
                .IsUnicode(false);

            modelBuilder.Entity<PhanQuyen>()
                .HasNoKey();
            modelBuilder.Entity<TaiKhoan>()
            .Property(t => t.MaNguoiDung)
            .ValueGeneratedOnAdd();

            modelBuilder.Entity<TaiKhoan>()
            .Property(t => t.IDQuyen)
            .HasDefaultValue(2);
        }
    }
}
