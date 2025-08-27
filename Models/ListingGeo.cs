    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace LocoRealt.Models
    {
        public class ListingGeo
        {
            [Key]
            public long ListingGeoId { get; set; }

            [Required]
            public long ListingId { get; set; }

            public long? CountryId { get; set; }
            public long? RegionId { get; set; }
            public long? CityId { get; set; }

            public string? TxtAddress { get; set; }
            public decimal? DblLatitude { get; set; }
            public decimal? DblLongitude { get; set; }
            public DateTime? DtTimeStamp { get; set; }
            public bool IntGoogleMap { get; set; }

            [ForeignKey(nameof(ListingId))]
            public Listing Listing { get; set; } = null!;

            [ForeignKey(nameof(CountryId))]
            public Country? Country { get; set; }

            [ForeignKey(nameof(RegionId))]
            public Region? Region { get; set; }

            [ForeignKey(nameof(CityId))]
            public City? City { get; set; }
        }
    }

