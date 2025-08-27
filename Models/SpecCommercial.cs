using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LocoRealt.Models;

[Table("Spec_Commercial")]
[Index("ListingId", Name = "UQ_CommercialListings_ListingId", IsUnique = true)]
public partial class SpecCommercial
{
    [Key]
    public long CommercialListingId { get; set; }

    [StringLength(100)]
    public string? Purpose { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? TotalArea { get; set; }

    public int? FloorNum { get; set; }

    public int? TotalFloors { get; set; }

    [StringLength(100)]
    public string? Condition { get; set; }

    public bool? HasHeating { get; set; }

    public bool? HasWater { get; set; }

    public bool? HasSewerage { get; set; }

    public bool? HasSecurity { get; set; }

    public long ListingId { get; set; }

    [ForeignKey("ListingId")]
    [InverseProperty("SpecCommercial")]
    public virtual Listing Listing { get; set; } = null!;
}
