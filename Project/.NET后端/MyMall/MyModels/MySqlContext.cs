using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MyModels
{
    public partial class MySqlContext:DbContext 
    {
        public MySqlContext()
        {
        }

        public MySqlContext(DbContextOptions<MySqlContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admins> Admin { get; set; }
        public virtual DbSet<Albums> Album { get; set; }
        public virtual DbSet<Categorys> Category { get; set; }
        public virtual DbSet<Carts> Cart { get; set; }
        public virtual DbSet<CategoryNames> CategoryName { get; set; }
        public virtual DbSet<Deliverys> Delivery { get; set; }
        public virtual DbSet<Records> Record { get; set; }
        public virtual DbSet<Orders> Order { get; set; }
        public virtual DbSet<Users> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySQL("server=;userid=;pwd=;port=;database=storedb;charset=utf8");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity<Admins>(entity =>
            {
                entity.ToTable("admins", "storedb");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Albums>(entity =>
            {
                entity.ToTable("albums", "storedb");

                entity.HasIndex(e => e.Author)
                    .HasName("album_author");

                entity.HasIndex(e => e.Isbn)
                    .HasName("album_isbn")
                    .IsUnique();

                entity.HasIndex(e => e.Name)
                    .HasName("album_name");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Author)
                    .HasColumnName("author")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.CoverUrl)
                    .HasColumnName("cover_url")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Isbn)
                    .IsRequired()
                    .HasColumnName("isbn")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.Publisher)
                    .HasColumnName("publisher")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Sales)
                    .HasColumnName("sales")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Stock)
                    .HasColumnName("stock")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("100");
            });

            modelBuilder.Entity<Categorys>(entity =>
            {
                entity.HasKey(e => new { e.AlbumId, e.CategoryId });

                entity.ToTable("categorys", "storedb");

                entity.HasIndex(e => e.CategoryId)
                    .HasName("fk_album_category_c");

                entity.Property(e => e.AlbumId)
                    .HasColumnName("album_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CategoryId)
                    .HasColumnName("category_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Album)
                    .WithMany(p => p.Categorys)
                    .HasForeignKey(d => d.AlbumId)
                    .HasConstraintName("fk_album_category_b");

                entity.HasOne(d => d.CategoryName)
                    .WithMany(p => p.Category)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("fk_album_category_c");
            });

            modelBuilder.Entity<Carts>(entity =>
            {
                entity.ToTable("carts", "storedb");

                entity.HasIndex(e => e.AlbumId)
                    .HasName("fk_cart_album");

                entity.HasIndex(e => e.UserId)
                    .HasName("fk_cart_user");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AlbumId)
                    .HasColumnName("album_id")
                    .HasColumnType("int(11)");
                    
                entity.Property(e => e.Count)
                    .HasColumnName("count")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Album)
                    .WithMany(p => p.Cart)
                    .HasForeignKey(d => d.AlbumId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_cart_album");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Cart)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_cart_user");
            });

            modelBuilder.Entity<CategoryNames>(entity =>
            {
                entity.ToTable("category_names", "storedb");

                entity.HasIndex(e => e.Name)
                    .HasName("category_name")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Deliverys>(entity =>
            {
                entity.ToTable("deliverys", "storedb");

                entity.HasIndex(e => e.Province)
                    .HasName("delivery_province")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DeliveryFee)
                    .HasColumnName("delivery_fee")
                    .HasColumnType("decimal(10,2)")
                    .HasDefaultValueSql("0.00");

                entity.Property(e => e.DeliveryTime)
                    .HasColumnName("delivery_time")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Province)
                    .IsRequired()
                    .HasColumnName("province")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Records>(entity =>
            {
                entity.ToTable("records", "storedb");

                entity.HasIndex(e => e.AlbumId)
                    .HasName("fk_record_album");

                entity.HasIndex(e => e.OrderId)
                    .HasName("fk_record_order");

                entity.HasIndex(e => e.UserId)
                    .HasName("fk_record_user");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AlbumId)
                    .HasColumnName("album_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Count)
                    .HasColumnName("count")
                    .HasColumnType("int(11)")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.OrderId)
                    .HasColumnName("order_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Album)
                    .WithMany(p => p.Record)
                    .HasForeignKey(d => d.AlbumId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_record_album");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Record)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("fk_record_order");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Record)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_record_user");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.ToTable("orders", "storedb");

                entity.HasIndex(e => e.UserId)
                    .HasName("fk_order_user");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.DeliveryFee)
                    .HasColumnName("delivery_fee")
                    .HasColumnType("decimal(10,2)")
                    .HasDefaultValueSql("0.00");

                entity.Property(e => e.OrderTime)
                    .HasColumnName("order_time")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ReceiverAddress)
                    .IsRequired()
                    .HasColumnName("receiver_address")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ReceiverName)
                    .IsRequired()
                    .HasColumnName("receiver_name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.ReceiverPhone)
                    .IsRequired()
                    .HasColumnName("receiver_phone")
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("int(3)")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.TotalPrice)
                    .HasColumnName("total_price")
                    .HasColumnType("decimal(10,2)");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Order)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_order_user");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users", "storedb");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasColumnName("city")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.District)
                    .HasColumnName("district")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.Province)
                    .HasColumnName("province")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            }); 
        }
    }
}
