using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LocoRealt.Models;

public partial class ListingStatus
{
    [Key]
    public long ListingStatusId { get; set; }

    [StringLength(2040)]
    public string TxtTitle { get; set; } = null!;

    public bool IntShow { get; set; }

    public int LngSort { get; set; }

    [InverseProperty("ListingStatus")]
    public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();
}
