using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LocoRealt.Models;

public partial class ListingImage
{
    [Key]
    public long ImageId { get; set; }

    public long ListingId { get; set; } // Обычный FK

    [ForeignKey("ListingId")]
    public virtual Listing Listing { get; set; } = null!;

  
    [StringLength(2040)]
    public string? TxtImage { get; set; }

    [StringLength(2040)]
    public string? TxtImageTitle { get; set; }

    [StringLength(2040)]
    public string? TxtImageUrl { get; set; }

    public int? IntSort { get; set; }

    }
