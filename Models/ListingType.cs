using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LocoRealt.Models;

public enum ListingDealType
{
    Sale,
    Rent
}

public partial class ListingType
{
    [Key]
    public int ListingTypeId { get; set; }

    [StringLength(2040)]
    public string TxtTitle { get; set; } = null!;

    public bool IntShow { get; set; }

    public int LngSort { get; set; }

    [InverseProperty("ListingType")]
    public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();

    public static List<(string Key, string Title)> GetDealTypes()
    {
        return new List<(string, string)>
        {
            ("sale", "Продажа"),
            ("rent", "Аренда")
        };
    }
}
