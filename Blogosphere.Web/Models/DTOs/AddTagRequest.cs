﻿using System.ComponentModel.DataAnnotations;

namespace Blogosphere.Web.Models.DTOs
{
    public class AddTagRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string DisplayName { get; set; }
    }
}
