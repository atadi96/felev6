using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hirportal.Persistence.DTO
{
    public class ArticleUploadDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 1)]
        public string Title { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 1)]
        public string Description { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public bool DeleteImages { get; set; }

        [Required]
        public byte[][] NewImages { get; set; }

        [Required]
        public bool Leading { get; set; }
    }
}
