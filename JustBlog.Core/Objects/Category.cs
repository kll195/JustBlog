﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace JustBlog.Core.Objects
{
    public class Category
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Name: Field is required")]
        [StringLength(500, ErrorMessage = "Name: Length should not exceed 500 Characters")]
        public virtual string  Name { get; set; }

        [Required(ErrorMessage = "UrlSlug: Field is required")]
        [StringLength(500, ErrorMessage = "UrlSlug: Length should not exceed 500 characters")]
        public virtual string UrlSlug { get; set; }

        public virtual string Description { get; set; }

        [JsonIgnore]
        public virtual IList<Post> Posts  { get; set; }
    }
}
