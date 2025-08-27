using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LocoRealt.Models;

public partial class Category
{
    [Key]
    public long CategoryId { get; set; }

    [StringLength(1020)]
    public string TxtTitle { get; set; } = null!;

    public int LngSort { get; set; }

    public bool IntShow { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();

    [InverseProperty("Category")]
    public virtual ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
}
