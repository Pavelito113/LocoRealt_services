using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LocoRealt.Models;

[Table("Spec_Houses")]
[Index("ListingId", Name = "UQ_HouseListings_ListingId", IsUnique = true)]
public partial class SpecHouse
{
    [Key]
    public long HouseListingId { get; set; }

    public int? FloorCount { get; set; }

    [StringLength(100)]
    public string? WallMaterial { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? HouseArea { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? LandArea { get; set; }

    [StringLength(100)]
    public string? LegalStatus { get; set; }

    [StringLength(100)]
    public string? Condition { get; set; }

    [StringLength(100)]
    public string? Heating { get; set; }

    [StringLength(100)]
    public string? WaterSupply { get; set; }

    [StringLength(100)]
    public string? Sewerage { get; set; }

    public long ListingId { get; set; }

    [ForeignKey("ListingId")]
    [InverseProperty("SpecHouse")]
    public virtual Listing Listing { get; set; } = null!;
}
