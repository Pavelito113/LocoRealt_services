using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LocoRealt.Models;

public partial class Country
{
    [Key]
    public long CountryId { get; set; }

    [StringLength(1020)]
    public string TxtTitle { get; set; } = null!;

    public bool IntShow { get; set; }

    public int LngSort { get; set; }

    [InverseProperty("Country")]
    public virtual ICollection<City> Cities { get; set; } = new List<City>();


    [InverseProperty("Country")]
    public virtual ICollection<Region> Regions { get; set; } = new List<Region>();

    [InverseProperty("Country")]
    public virtual ICollection<ListingGeo> ListingGeos { get; set; } = new List<ListingGeo>();
}
