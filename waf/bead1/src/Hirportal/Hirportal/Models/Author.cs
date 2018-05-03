using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Hirportal.Models
{
    public class Author : IdentityUser<int>
    {
        public Author()
        {
            Articles = new HashSet<Article>();
        }

        [Required]
        public string Name { get; set; }

        public ICollection<Article> Articles;
    }
}
