using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cookware_react_backend.Context;
using cookware_react_backend.Models;

namespace cookware_react_backend.Services
{
    public class ReviewServices
    {
        private readonly DataContext _dataContext;
        public ReviewServices(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<ReviewModel>> SeeAllReviewsAsync() => await _dataContext.Reviews.ToListAsync();

        public async Task<List<ReviewModel>> GetReviewsByProductIdAsync(int productId)
        {
            return await _dataContext.Reviews
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.CreatedDate)
                .ToListAsync();
        }

        public async Task<bool> AddReviewAsync(ReviewModel review)
        {
            review.CreatedDate = DateTime.UtcNow;
            await _dataContext.Reviews.AddAsync(review);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> AdminDeleteReviewAsync(int id)
        {
            var review = await _dataContext.Reviews.FindAsync(id);
            if (review == null) return false;

            _dataContext.Reviews.Remove(review);
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}