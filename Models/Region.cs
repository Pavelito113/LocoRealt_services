using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LocoRealt.Models;

public partial class Region
{
    [Key]
    public long RegionId { get; set; }

    public long CountryId { get; set; }

    [StringLength(1020)]
    public string TxtTitle { get; set; } = null!;

    public bool IntShow { get; set; }

    [InverseProperty("Region")]
    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    [ForeignKey("CountryId")]
    [InverseProperty("Regions")]
    public virtual Country Country { get; set; } = null!;

    [InverseProperty("Region")]
    public virtual ICollection<ListingGeo> ListingGeos { get; set; } = new List<ListingGeo>();
}
