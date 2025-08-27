namespace LocoRealt.Models
{
    public class ListingItemDto
    {
        public long ListingId { get; set; }
        public string TxtTitle { get; set; } = null!;
        public decimal DblPrice { get; set; }
        public string? TxtImageUrl { get; set; }
        public string? Location { get; set; } // Should come from ListingGeo
        public DateTime DtCreated { get; set; }
        public bool IsBargain { get; set; }
        public bool IsForSale { get; set; }
        public bool IsForRent { get; set; }
        public Category Category { get; set; } = null!;
        public ListingType ListingType { get; set; } = null!;
        public SubCategory SubCategory { get; set; } = null!;

        // Additional useful properties for cards
        public int? Rooms { get; set; }
        public int? Floor { get; set; }
        public decimal? TotalArea { get; set; }
        public decimal? LandArea { get; set; }
        public string? Status { get; set; }
    }
}
