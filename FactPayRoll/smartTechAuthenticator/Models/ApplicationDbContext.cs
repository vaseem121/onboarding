using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace smartTechAuthenticator.Models
{
    public partial class ApplicationDbContext : DbContext
    { 
        public ApplicationDbContext()
        { 

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            this.Database.SetCommandTimeout(120); // <-- 90 seconds
        }

        public virtual DbSet<CompanyInfo> CompanyInfo { get; set; }
        public virtual DbSet<CustomerInfo> CustomerInfo { get; set; }
        public virtual DbSet<TestKits> TestKits { get; set; }
        public virtual DbSet<TestkitCheckList> TestkitCheckList { get; set; }
        public virtual DbSet<TrackingForms> TrackingForms { get; set; }
        public virtual DbSet<StateMater> StateMaters { get; set; }
        public virtual DbSet<DistricttMaster> DistricttMasters { get; set; }
        public virtual DbSet<CustomerAddresses> CustomerAddressMasters { get; set; }
        public virtual DbSet<QrCodeMaster> QrCodeMasters { get; set; }
        public virtual DbSet<ProductMaster> ProductMasters { get; set; } 
        public virtual DbSet<ProductCategory> ProductCategories { get; set; } 
        public virtual DbSet<BannerCarousel> BannerCarousel { get; set; } 
        public virtual DbSet<ProductImages> ProductImages { get; set; } 
        public virtual DbSet<News> News { get; set; } 
        public virtual DbSet<Ticket> Ticket { get; set; } 
        public virtual DbSet<AddToCart> AddToCart { get; set; } 
        public virtual DbSet<Shipping> Shipping { get; set; } 
        public virtual DbSet<Payment> Payment { get; set; } 
        public virtual DbSet<ShippingTracking> ShippingTracking { get; set; } 
        public virtual DbSet<Order> Order { get; set; } 
        public virtual DbSet<Order_Items> Order_Items { get; set; } 
        public virtual DbSet<HelpGuid> HelpGuid { get; set; } 
        public virtual DbSet<TermPrivacy> TermPrivacy { get; set; } 
        public virtual DbSet<About> About { get; set; } 
        public virtual DbSet<FAQ> FAQ { get; set; } 
        public virtual DbSet<ProductGallery> ProductGallery { get; set; } 
        public virtual DbSet<Form> Form { get; set; } 
        public virtual DbSet<FormProperty> FormProperty { get; set; } 
        public virtual DbSet<MultipleChoice> MultipleChoice { get; set; } 
        public virtual DbSet<Dropdown> Dropdown { get; set; } 
        public virtual DbSet<CheckBox> CheckBox { get; set; } 
        public virtual DbSet<FormResponce> FormResponce { get; set; } 
        public virtual DbSet<FormPropertyResponce> FormPropertyResponce { get; set; } 
        public virtual DbSet<MultipleChoiceResponce> MultipleChoiceResponce { get; set; } 
        public virtual DbSet<CheckboxResponce> CheckboxResponce { get; set; } 
        public virtual DbSet<ProductColor> ProductColor { get; set; } 
        public virtual DbSet<ProductSize> ProductSize { get; set; } 
        public virtual DbSet<LoginActivity> LoginActivity { get; set; } 
        public virtual DbSet<TicketMessageSystem> TicketMessageSystem { get; set; } 
        public virtual DbSet<ProductTag> ProductTag { get; set; } 
        public virtual DbSet<MenuPermission> MenuPermission { get; set; }
        public virtual DbSet<CompanyDetails> CompanyDetails { get; set; }
        public virtual DbSet<EmployeeDetails> EmployeeDetails { get; set; }
        public virtual DbSet<HtmlResponseData> HtmlResponseData { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            { 
                optionsBuilder.UseSqlServer(ConfigurationManager.AppSettings["ConnectionString"].ToString());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyInfo>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address1)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Address2)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Address3)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(6)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedTs)
                    .HasColumnName("CreatedTS")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(400);

                entity.Property(e => e.LoginId)
                    .IsRequired()
                    .HasColumnName("LoginID")
                    .HasMaxLength(50);

                entity.Property(e => e.LoginPass)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CustomerInfo>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Address1)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Address2)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Address3)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyId).HasDefaultValueSql("('0')");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DeviceId)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.MobileNo)
                    .IsRequired()
                    .HasMaxLength(13)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Nric)
                    .IsRequired()
                    .HasColumnName("NRIC")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.UserPass)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<TestKits>(entity =>
            {
                entity.HasKey(e => e.Qrcode)
                    .HasName("PK_AntigenKits");

                entity.Property(e => e.Qrcode)
                    .HasColumnName("QRCode")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedTs)
                    .HasColumnName("CreatedTS")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Product)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<TestkitCheckList>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedTs)
                    .HasColumnName("CreatedTS")
                    .HasColumnType("datetime");

                entity.Property(e => e.Qrcode)
                    .IsRequired()
                    .HasColumnName("QRCode")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TrackingForms>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AntigenType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CustId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Date)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.LotNumber)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MobileNo)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Place)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TestResults)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Time)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        internal (int TotalCount, int FilteredCount, dynamic News) GetNewsDetails(int skip, int take, string sortColumn, string sortColumnDir, string searchValue)
        {
            throw new NotImplementedException();
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
