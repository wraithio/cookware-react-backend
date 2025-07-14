using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cookware_react_backend.Context;
using cookware_react_backend.Models;

namespace cookware_react_backend.Services
{
    public class ProductServices
    {
        private readonly DataContext _dataContext;

        public ProductServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<ProductModel>> GetAllProductsAsync() => await _dataContext.Products.ToListAsync();

        public async Task<List<ProductModel>> GetLiveProductsAsync()
        {
            return await _dataContext.Products
                .Where(p => p.IsArchived == false)
                .GroupBy(p => p.Id) // Group by product ID
                .Select(group => group.OrderByDescending(p => p.ModifiedDate).First()) // Get most recent from each group
                .ToListAsync();
        }
        public async Task<ProductModel?> GetProductByIdAsync(int id) => await _dataContext.Products.Where(p => p.Id == id)
                .OrderByDescending(p => p.ModifiedDate)
                .FirstOrDefaultAsync();

        public async Task<ProductModel?> GetProductByProductNameAsync(string name) => await _dataContext.Products.Where(p => p.Name == name)
                .OrderByDescending(p => p.ModifiedDate)
                .FirstOrDefaultAsync();

        public async Task<bool> AddProductAsync(ProductModel product)
        {
            product.ForeignKey = GetNextProjectForeignKey();
            product.CreatedDate = DateTime.UtcNow;
            product.ModifiedDate = DateTime.UtcNow;
            await _dataContext.Products.AddAsync(product);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        //used to archive a product
        public async Task<bool> UpdateProductEntryAsync(ProductModel product)
        {
            var existingProduct = await _dataContext.Products
                .Where(p => p.Id == product.Id)
                .OrderByDescending(p => p.CreatedDate)
                .FirstOrDefaultAsync();

            if (existingProduct == null)
                return false;

            _dataContext.Entry(existingProduct).CurrentValues.SetValues(product);

            // Ensure audit fields are updated
            existingProduct.ModifiedDate = DateTime.UtcNow;

            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> HardDeleteProductEntriesAsync(int id)
        {
            var products = await _dataContext.Products
        .Where(p => p.Id == id)
        .OrderByDescending(p => p.CreatedDate)
        .ToListAsync();
            if (!products.Any()) return false;

            _dataContext.Products.RemoveRange(products);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<List<ProductModel>> GetArchivedProductsAsync()
        {
            return await _dataContext.Products
                .Where(p => p.IsArchived == true)
                .GroupBy(p => p.Id) // Group by product ID
                .Select(group => group.OrderByDescending(p => p.ModifiedDate).First()) // Get most recent from each group
                .ToListAsync();
        }

        private int GetNextProjectForeignKey()
        {
            int nextForeignKey = 1;
            try
            {
                var mostRecentProject = _dataContext.Products
                                                .OrderByDescending(p => p.CreatedDate) // Order by creation date to get the most recent
                                                .FirstOrDefault();

                if (mostRecentProject != null)
                {
                    nextForeignKey = mostRecentProject.ForeignKey + 1; // Increment the foreign key
                }
            }
            catch (Exception ex)
            {
                // Handle any database access errors (e.g., log the exception)
                Console.WriteLine($"Error retrieving the most recent project foreign key: {ex.Message}");
            }

            return nextForeignKey;
        }
    }


}