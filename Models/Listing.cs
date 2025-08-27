using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LocoRealt.Models
{
    [Index(nameof(CategoryId), Name = "IX_Listings_Category")]
    [Index(nameof(DblPrice), Name = "IX_Listings_Price")]
    [Index(nameof(SubCategoryId), Name = "IX_Listings_SubCategory")]
    public partial class Listing
    {
       
        [Key]
        [Column("ListingId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ListingId { get; set; }
        public ListingGeo? ListingGeo { get; set; }

        public string? UserId { get; set; } // FK в AspNetUsers

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; } = null!;

        public int ListingTypeId { get; set; }
        public long ListingStatusId { get; set; }
        public long SubCategoryId { get; set; }

        [StringLength(2040)]
        public string TxtTitle { get; set; } = null!;

        public string? MemDescription { get; set; }
        [Column("MemSEODescription")]
        public string? MemSEODescription { get; set; }

        [Column("MemSEOKeywords")]
        public string? MemSEOKeywords { get; set; }

        [Column(TypeName = "decimal(18, 0)")]
        public decimal DblPrice { get; set; }

         public bool IsForSale { get; set; }
        public bool IsForRent { get; set; }
        public bool IsBargain { get; set; }
        public bool IsLongTerm { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime DtCreated { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? DtExpiry { get; set; }

        public bool IntShow { get; set; }

        public long CategoryId { get; set; }

        public bool HasGasHeating { get; set; }
        public bool HasCentralWater { get; set; }
        public bool HasCentralHeating { get; set; }


        public bool HasCentralSCanalisation { get; set; }

        public bool HasInternet { get; set; }
        public bool HasParking { get; set; }

        public int? Rooms { get; set; }
        public int? Floor { get; set; }

        [Column(TypeName = "decimal(9, 2)")]
        public decimal? TotalArea { get; set; }

        [Column(TypeName = "decimal(9, 2)")]
        public decimal? LandArea { get; set; }

        [StringLength(20)]
        public string? Status { get; set; }

        [ForeignKey(nameof(CategoryId))]
        [InverseProperty("Listings")]
        public virtual Category Category { get; set; } = null!;

        [ForeignKey(nameof(ListingStatusId))]
        [InverseProperty("Listings")]
        public virtual ListingStatus ListingStatus { get; set; } = null!;

   

        [ForeignKey(nameof(ListingTypeId))]
        [InverseProperty("Listings")]
        public virtual ListingType ListingType { get; set; } = null!;

        [ForeignKey(nameof(SubCategoryId))]
        [InverseProperty("Listings")]
        public virtual SubCategory SubCategory { get; set; } = null!;

        [InverseProperty(nameof(Like.Listing))]
        public virtual ICollection<Like> Likes { get; set; } = [];

        [InverseProperty(nameof(ListingImage.Listing))]
        public virtual ICollection<ListingImage> ListingImages { get; set; } = [];

        [InverseProperty(nameof(SpecCommercial.Listing))]
        public virtual SpecCommercial? SpecCommercial { get; set; }

        [InverseProperty(nameof(SpecFlat.Listing))]
        public virtual SpecFlat? SpecFlat { get; set; }

        [InverseProperty(nameof(SpecHouse.Listing))]
        public virtual SpecHouse? SpecHouse { get; set; }

        [InverseProperty(nameof(SpecLandPlot.Listing))]
        public virtual SpecLandPlot? SpecLandPlot { get; set; }
    }
}
