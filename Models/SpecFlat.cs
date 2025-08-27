using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LocoRealt.Models;

[Table("Spec_Flats")]
[Index("ListingId", Name = "UQ_Flats_ListingId", IsUnique = true)]
public partial class SpecFlat
{
    [Key]
    public long FlatId { get; set; }

    public int? Rooms { get; set; }

    public bool? IsStudio { get; set; }

    public bool? IsRoom { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal? KitchenArea { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal? LivingArea { get; set; }

    [Column(TypeName = "decimal(6, 2)")]
    public decimal? TotalArea { get; set; }

    [StringLength(50)]
    public string? BathroomType { get; set; }

    [StringLength(20)]
    public string? Balcony { get; set; }

    public int? FloorNum { get; set; }

    public int? TotalFloors { get; set; }

    [StringLength(100)]
    public string? WindowView { get; set; }

    [StringLength(100)]
    public string? HeatingType { get; set; }

    [StringLength(100)]
    public string? RepairState { get; set; }

    public long ListingId { get; set; }

    [ForeignKey("ListingId")]
    [InverseProperty("SpecFlat")]
    public virtual Listing Listing { get; set; } = null!;
}
