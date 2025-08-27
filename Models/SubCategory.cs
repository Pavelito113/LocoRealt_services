using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LocoRealt.Models;

public partial class SubCategory
{
    [Key]
    public long SubCategoryId { get; set; }

    public long CategoryId { get; set; }

    [StringLength(1020)]
    public string TxtTitle { get; set; } = null!;

    public int LngSort { get; set; }

    public bool IntShow { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("SubCategories")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("SubCategory")]
    public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();
}
