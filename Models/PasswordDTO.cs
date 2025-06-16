using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cookware_react_backend.Models
{
    public class PasswordDTO
    {
        public string? Salt {get;set;}
        public string? Hash {get;set;}
    }
}