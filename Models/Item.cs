using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Intro_To_Web_Api.Models
{
    public class Item
    {
        [Required]
        public long Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public bool IsValidItem()
        {
            return (this is null || this.Id <= 0 || this.Price <= 0 || string.IsNullOrWhiteSpace(this.Name));
        }
    }
}
