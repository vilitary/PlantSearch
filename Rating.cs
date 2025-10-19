using System;

namespace PlantSearch.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Rating
{
     [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("plant_id")]
        public int PlantId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("rating_value")]
        public int RatingValue { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [ForeignKey("PlantId")]
        public Plant? Plant { get; set; }

        [ForeignKey("UserId")]
        public Account? Account { get; set; }
}

