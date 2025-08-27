using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocoRealt.Models
{
    public partial class Like
    {
        [Key]
        public long LikeId { get; set; }

        public long ListingId { get; set; }

        [ForeignKey("ListingId")]
        [InverseProperty("Likes")]
        public virtual Listing Listing { get; set; } = null!;  // Явная инициализация

        [Required]
        public string UserId { get; set; } = null!;  // Явная инициализация

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;  // Явная инициализация

        [Column(TypeName = "datetime")]
        public DateTime? DtLiked { get; set; } = DateTime.Now;
    }
}