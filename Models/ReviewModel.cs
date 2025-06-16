using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cookware_react_backend.Models
{
    public class ReviewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? Name { get; set; }
        public string? ReviewText { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}