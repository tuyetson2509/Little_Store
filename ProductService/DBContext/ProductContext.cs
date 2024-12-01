using Microsoft.EntityFrameworkCore;
using ProductService.Model;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ProductService.DBContext
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }

        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<LoaiHang> LoaiHangs { get; set; }
        public DbSet<NhaCungCap> NhaCungCaps { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SanPham>().ToTable("SanPham");
            modelBuilder.Entity<LoaiHang>().ToTable("LoaiHang");
            modelBuilder.Entity<NhaCungCap>().ToTable("NhaCungCap");

            modelBuilder.Entity<SanPham>()
                .Property(e => e.GiaBan)
                .HasPrecision(18, 2);
        }
    }
}
