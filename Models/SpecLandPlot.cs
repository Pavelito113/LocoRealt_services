using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LocoRealt.Models;

[Table("Spec_LandPlots")]
[Index("ListingId", Name = "UQ_LandPlotListings_ListingId", IsUnique = true)]
public partial class SpecLandPlot
{
    [Key]
    public long LandPlotListingId { get; set; }

    [StringLength(100)]
    public string? Purpose { get; set; }

    public bool? HasGas { get; set; }

    public bool? HasElectricity { get; set; }

    public bool? HasWaterWell { get; set; }

    public bool? HasCentralWater { get; set; }

    public bool? HasSeptic { get; set; }

    public long ListingId { get; set; }

    [ForeignKey("ListingId")]
    [InverseProperty("SpecLandPlot")]
    public virtual Listing Listing { get; set; } = null!;
}
