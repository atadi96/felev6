using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hirportal.Persistence
{
    [Table("Articles")]
    public class Article
    {
        public Article()
        {
            Images = new HashSet<ArticleImage>();
            Modified = DateTime.Now;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("User")]
        public Author Author { get; set; }

        [Required]
        public string Title { get; set; }

        [Required, StringLength(1000)]
        public string Description { get; set; }

        [Required]
        public string Content { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Modified { get; set; }

        [Required]
        public bool Leading { get; set; }

        public ICollection<ArticleImage> Images { get; set; }
    }
}
