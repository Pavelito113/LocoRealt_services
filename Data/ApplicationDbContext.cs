using LocoRealt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LocoRealt.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<City> Cities { get; set; } = null!;
        public virtual DbSet<Country> Countries { get; set; } = null!;
        public virtual DbSet<Like> Likes { get; set; } = null!;
        public virtual DbSet<Listing> Listings { get; set; } = null!;
        public virtual DbSet<ListingGeo> ListingGeos { get; set; } = null!;
        public virtual DbSet<ListingImage> ListingImages { get; set; } = null!;
        public virtual DbSet<ListingStatus> ListingStatuses { get; set; } = null!;
        public virtual DbSet<ListingType> ListingTypes { get; set; } = null!;
        public virtual DbSet<Region> Regions { get; set; } = null!;
        public virtual DbSet<SpecCommercial> SpecCommercials { get; set; } = null!;
        public virtual DbSet<SpecFlat> SpecFlats { get; set; } = null!;
        public virtual DbSet<SpecHouse> SpecHouses { get; set; } = null!;
        public virtual DbSet<SpecLandPlot> SpecLandPlots { get; set; } = null!;
        public virtual DbSet<SubCategory> SubCategories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Listing entity
            modelBuilder.Entity<Listing>(entity =>
            {
                entity.ToTable("Listings");
                entity.HasKey(e => e.ListingId);
                entity.Property(e => e.ListingId).HasColumnName("ListingId").ValueGeneratedOnAdd();

                // FK к AspNetUsers (UserId)
                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                // One-to-one с ListingGeo
                entity.HasOne(d => d.ListingGeo)
                    .WithOne(g => g.Listing)
                    .HasForeignKey<ListingGeo>(g => g.ListingId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Category
                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Listings)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                // ListingStatus
                entity.HasOne(d => d.ListingStatus)
                    .WithMany(p => p.Listings)
                    .HasForeignKey(d => d.ListingStatusId)
                    .OnDelete(DeleteBehavior.Restrict);

                // ListingType
                entity.HasOne(d => d.ListingType)
                    .WithMany(p => p.Listings)
                    .HasForeignKey(d => d.ListingTypeId)
                    .OnDelete(DeleteBehavior.Restrict);

                // SubCategory
                entity.HasOne(d => d.SubCategory)
                    .WithMany(p => p.Listings)
                    .HasForeignKey(d => d.SubCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            // Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId);
                entity.Property(e => e.IntShow).HasDefaultValue(true);
            });

            // City
            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(e => e.CityId);
                entity.Property(e => e.IntShow).HasDefaultValue(true);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.RegionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            // Country
            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.CountryId);
                entity.Property(e => e.IntShow).HasDefaultValue(true);
            });

            // Like
            modelBuilder.Entity<Like>(entity =>
            {
                entity.HasKey(e => e.LikeId);
                entity.Property(e => e.DtLiked).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Listing)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.ListingId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            // ListingGeo
            modelBuilder.Entity<ListingGeo>(entity =>
            {
                entity.HasKey(e => e.ListingGeoId);

                // Relationships with City, Country, Region
                entity.HasOne(d => d.City)
                    .WithMany(c => c.ListingGeos)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.ListingGeos)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.ListingGeos)
                    .HasForeignKey(d => d.RegionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            // ListingImage
            modelBuilder.Entity<ListingImage>(entity =>
            {
                entity.HasKey(e => e.ImageId);

                entity.HasOne(d => d.Listing)
                    .WithMany(p => p.ListingImages)
                    .HasForeignKey(d => d.ListingId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            // ListingStatus
            modelBuilder.Entity<ListingStatus>(entity =>
            {
                entity.HasKey(e => e.ListingStatusId);
                entity.Property(e => e.IntShow).HasDefaultValue(true);
            });

            // ListingType
            modelBuilder.Entity<ListingType>(entity =>
            {
                entity.HasKey(e => e.ListingTypeId);
                entity.Property(e => e.IntShow).HasDefaultValue(true);
            });

            // Region
            modelBuilder.Entity<Region>(entity =>
            {
                entity.HasKey(e => e.RegionId);
                entity.Property(e => e.IntShow).HasDefaultValue(true);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Regions)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // SpecCommercial
            modelBuilder.Entity<SpecCommercial>(entity =>
            {
                entity.HasKey(e => e.ListingId);

                entity.HasOne(d => d.Listing)
                    .WithOne(p => p.SpecCommercial)
                    .HasForeignKey<SpecCommercial>(d => d.ListingId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // SpecFlat
            modelBuilder.Entity<SpecFlat>(entity =>
            {
                entity.HasKey(e => e.ListingId);

                entity.HasOne(d => d.Listing)
                    .WithOne(p => p.SpecFlat)
                    .HasForeignKey<SpecFlat>(d => d.ListingId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // SpecHouse
            modelBuilder.Entity<SpecHouse>(entity =>
            {
                entity.HasKey(e => e.ListingId);

                entity.HasOne(d => d.Listing)
                    .WithOne(p => p.SpecHouse)
                    .HasForeignKey<SpecHouse>(d => d.ListingId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // SpecLandPlot
            modelBuilder.Entity<SpecLandPlot>(entity =>
            {
                entity.HasKey(e => e.ListingId);

                entity.HasOne(d => d.Listing)
                    .WithOne(p => p.SpecLandPlot)
                    .HasForeignKey<SpecLandPlot>(d => d.ListingId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // SubCategory
            modelBuilder.Entity<SubCategory>(entity =>
            {
                entity.HasKey(e => e.SubCategoryId);
                entity.Property(e => e.IntShow).HasDefaultValue(true);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.SubCategories)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}