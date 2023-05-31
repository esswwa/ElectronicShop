using System;
using System.Collections.Generic;
using ElectronicShop.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace ElectronicShop.Data;

public partial class ElectronickshopContext : DbContext
{
    public ElectronickshopContext()
    {
    }

    public ElectronickshopContext(DbContextOptions<ElectronickshopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Basket> Baskets { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Favourity> Favourities { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<HelperBasket> HelperBaskets { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderHelper> OrderHelpers { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<StatusOrder> StatusOrders { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("server=localhost;user=root;password=Qwerty123;database=electronickshop", ServerVersion.Parse("8.0.25-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Basket>(entity =>
        {
            entity.HasKey(e => e.Idbasket).HasName("PRIMARY");

            entity.ToTable("basket");

            entity.HasIndex(e => e.IdUser, "FK_idUsert_from_basket_to_user_idx");

            entity.Property(e => e.Idbasket)
                .ValueGeneratedNever()
                .HasColumnName("idbasket");
            entity.Property(e => e.IdUser).HasColumnName("idUser");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Baskets)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_idUsert_from_basket_to_user");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Idcategory).HasName("PRIMARY");

            entity.ToTable("category");

            entity.Property(e => e.Idcategory).HasColumnName("idcategory");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(45)
                .HasColumnName("categoryName");
        });

        modelBuilder.Entity<Favourity>(entity =>
        {
            entity.HasKey(e => e.Idfavourities).HasName("PRIMARY");

            entity.ToTable("favourities");

            entity.HasIndex(e => e.IdProduct, "FK_idProductFromFavourities_idx");

            entity.HasIndex(e => e.IdUser, "FK_userid_for_user_idx");

            entity.Property(e => e.Idfavourities)
                .ValueGeneratedNever()
                .HasColumnName("idfavourities");
            entity.Property(e => e.IdProduct).HasColumnName("idProduct");
            entity.Property(e => e.IdUser).HasColumnName("idUser");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.Favourities)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_idProductFromFavourities");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Favourities)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_userid_for_user");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Idfeedback).HasName("PRIMARY");

            entity.ToTable("feedback");

            entity.HasIndex(e => e.IdProduct, "FK_idProductFromFeedBack_idx");

            entity.HasIndex(e => e.IdUser, "FK_idUser_FromFeedBack_idx");

            entity.Property(e => e.Idfeedback).HasColumnName("idfeedback");
            entity.Property(e => e.Feedbacks)
                .HasMaxLength(100)
                .HasColumnName("feedbacks");
            entity.Property(e => e.IdProduct).HasColumnName("idProduct");
            entity.Property(e => e.IdUser).HasColumnName("idUser");
            entity.Property(e => e.Reiting).HasColumnName("reiting");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_idProductFromFeedBack");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_idUser_FromFeedBack");
        });

        modelBuilder.Entity<HelperBasket>(entity =>
        {
            entity.HasKey(e => e.IdhelperOrder).HasName("PRIMARY");

            entity.ToTable("helper_basket");

            entity.HasIndex(e => e.IdBasket, "FK_idHelper_from_basket_to_helperBasket_idx");

            entity.HasIndex(e => e.IdProduct, "fk_idproduct_from_helper_to_product_idx");

            entity.Property(e => e.IdhelperOrder).HasColumnName("idhelper_order");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.IdBasket).HasColumnName("id_basket");
            entity.Property(e => e.IdProduct).HasColumnName("id_product");

            entity.HasOne(d => d.IdBasketNavigation).WithMany(p => p.HelperBaskets)
                .HasForeignKey(d => d.IdBasket)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_idHelper_from_basket_to_helperBasket");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.HelperBaskets)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_idproduct_from_helper_to_product");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Idorder).HasName("PRIMARY");

            entity.ToTable("order");

            entity.HasIndex(e => e.IdStatusOrder, "FK_idStatusOrder_FromCheque_idx");

            entity.HasIndex(e => e.IdUser, "FK_idUser_FromCheque_idx");

            entity.Property(e => e.Idorder)
                .ValueGeneratedNever()
                .HasColumnName("idorder");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.IdStatusOrder).HasColumnName("idStatusOrder");
            entity.Property(e => e.IdUser).HasColumnName("idUser");

            entity.HasOne(d => d.IdStatusOrderNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdStatusOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_idStatusOrder_FromCheque");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_idUser_FromOrder");
        });

        modelBuilder.Entity<OrderHelper>(entity =>
        {
            entity.HasKey(e => e.IdorderHelper).HasName("PRIMARY");

            entity.ToTable("order_helper");

            entity.HasIndex(e => e.IdOrder, "fk_id_order_from_helper_idx");

            entity.HasIndex(e => e.IdProduct, "fk_id_product_from_helper_idx");

            entity.Property(e => e.IdorderHelper)
                .ValueGeneratedNever()
                .HasColumnName("idorder_helper");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.Count).HasColumnName("count");
            entity.Property(e => e.IdOrder).HasColumnName("id_order");
            entity.Property(e => e.IdProduct).HasColumnName("id_product");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.OrderHelpers)
                .HasForeignKey(d => d.IdOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_id_order_from_helper");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.OrderHelpers)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_id_product_from_helper");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct).HasName("PRIMARY");

            entity.ToTable("product");

            entity.HasIndex(e => e.CategoryProduct, "FK_categoryProduct_FromProduct_idx");

            entity.HasIndex(e => e.Status, "Fk_status_fromProduct_to_Status_idx");

            entity.Property(e => e.IdProduct)
                .ValueGeneratedNever()
                .HasColumnName("idProduct");
            entity.Property(e => e.CategoryProduct).HasColumnName("categoryProduct");
            entity.Property(e => e.CostProduct).HasColumnName("costProduct");
            entity.Property(e => e.CountProduct).HasColumnName("countProduct");
            entity.Property(e => e.FirmProduct)
                .HasMaxLength(45)
                .HasColumnName("firmProduct");
            entity.Property(e => e.ImgProduct)
                .HasColumnType("text")
                .HasColumnName("imgProduct");
            entity.Property(e => e.NameProduct)
                .HasMaxLength(45)
                .HasColumnName("nameProduct");
            entity.Property(e => e.ReitingProduct).HasColumnName("reitingProduct");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.CategoryProductNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryProduct)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_categoryProduct_FromProduct");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.Status)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk_status_fromProduct_to_Status");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("PRIMARY");

            entity.ToTable("role");

            entity.Property(e => e.IdRole)
                .ValueGeneratedNever()
                .HasColumnName("idRole");
            entity.Property(e => e.Role1)
                .HasMaxLength(45)
                .HasColumnName("role");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Idstatus).HasName("PRIMARY");

            entity.ToTable("status");

            entity.Property(e => e.Idstatus)
                .ValueGeneratedNever()
                .HasColumnName("idstatus");
            entity.Property(e => e.Status1)
                .HasMaxLength(45)
                .HasColumnName("status");
        });

        modelBuilder.Entity<StatusOrder>(entity =>
        {
            entity.HasKey(e => e.IdstatusOrder).HasName("PRIMARY");

            entity.ToTable("status_order");

            entity.Property(e => e.IdstatusOrder)
                .ValueGeneratedNever()
                .HasColumnName("idstatus_order");
            entity.Property(e => e.NameStatus)
                .HasMaxLength(45)
                .HasColumnName("nameStatus");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Iduser).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.RoleId, "fk_usRole_idx");

            entity.Property(e => e.Iduser)
                .ValueGeneratedNever()
                .HasColumnName("iduser");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.ExitCheck).HasColumnName("exitCheck");
            entity.Property(e => e.Login)
                .HasMaxLength(45)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("roleId");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_usRole");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
