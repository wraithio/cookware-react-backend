using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cookware_react_backend.Context;
using cookware_react_backend.Models;

namespace cookware_react_backend.Services
{
    public class DetailsServices
    {
        private readonly DataContext _dataContext;
        public DetailsServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<DetailsModel>> GetAllDetailsAsync() => await _dataContext.Details.ToListAsync();

        public async Task<DetailsModel?> GetDetailsByProductIdAsync(int id) => await _dataContext.Details
            .Where(d => d.ProductId == id)
            .OrderByDescending(d => d.CreatedDate)
            .FirstOrDefaultAsync();

        public async Task<bool> AddDetailsAsync(DetailsModel details)
        {
            details.CreatedDate = DateTime.UtcNow;
            details.ModifiedDate = DateTime.UtcNow;
            await _dataContext.Details.AddAsync(details);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> HardDeleteDetailsEntriesAsync(int id)
        {
            var details = await _dataContext.Details
        .Where(p => p.Id == id)
        .OrderByDescending(p => p.CreatedDate)
        .ToListAsync();
            if (!details.Any()) return false;

            _dataContext.Details.RemoveRange(details);
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
    

}