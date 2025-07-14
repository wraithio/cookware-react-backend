using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cookware_react_backend.Models
{
    public class DetailsModel
    {
        public int Id { get; set; }
        public int ForeignKey { get; set; }
        public string? Material { get; set; }
        public string? Capacity { get; set; }
        public string? Dimensions { get; set; }
        public string? Weight { get; set; }
        public string? Care { get; set; }
        public string? Description { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

    }
}