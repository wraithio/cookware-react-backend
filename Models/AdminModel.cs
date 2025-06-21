using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cookware_react_backend.Models
{
    public class AdminModel
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Salt { get; set; }
        public string? Hash { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastLogin { get; set; }
        public bool IsActive { get; set; } = true;
    }
}