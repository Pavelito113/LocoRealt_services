using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LocoRealt.Models;

public partial class City
{
    [Key]
    public long CityId { get; set; }

    public long CountryId { get; set; }

    public long RegionId { get; set; }

    [StringLength(1020)]
    public string TxtTitle { get; set; } = null!;

    public bool IntShow { get; set; }

    public bool IntPopular { get; set; }

    [ForeignKey("CountryId")]
    [InverseProperty("Cities")]
    public virtual Country Country { get; set; } = null!;


    [ForeignKey("RegionId")]
    [InverseProperty("Cities")]
    public virtual Region Region { get; set; } = null!;

    [InverseProperty("City")]
    public virtual ICollection<ListingGeo> ListingGeos { get; set; } = new List<ListingGeo>();
}
