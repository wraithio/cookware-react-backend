using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cookware_react_backend.Models;
using Microsoft.EntityFrameworkCore;


namespace cookware_react_backend.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<ProductModel> Products { get; set; }
        public DbSet<AdminModel> Admins { get; set; }
        public DbSet<ReviewModel> Reviews { get; set; }
        public DbSet<DetailsModel> Details { get; set; }


    }
}