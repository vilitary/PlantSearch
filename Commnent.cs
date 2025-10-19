using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlantSearch.Models;

    public class Comment
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("plant_id")]
        public int PlantId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("content")]
        public string? Content { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        // Khóa ngoại (navigation)
        [ForeignKey("PlantId")]
        public Plant? Plant { get; set; }

        [ForeignKey("UserId")]
        public Account? Account { get; set; }
    }

