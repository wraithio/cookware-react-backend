using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cookware_react_backend.Context;
using cookware_react_backend.Models;
using cookware_react_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace cookware_react_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewServices _reviewServices;
        public ReviewController(ReviewServices reviewServices)
        {
            _reviewServices = reviewServices;
        }

        [HttpGet("GetAllReviews")]
        public async Task<IActionResult> GetAllReviews()
        {
            var reviews = await _reviewServices.SeeAllReviewsAsync();
            if (reviews != null && reviews.Count > 0)
                return Ok(reviews);
            return NotFound(new { Message = "No reviews found" });
        }

        [HttpGet("GetReviewsByProductId/{productId}")]
        public async Task<IActionResult> GetReviewsByProductId(int productId)
        {
            var reviews = await _reviewServices.GetReviewsByForeignKeyAsync(productId);
            if (reviews != null && reviews.Count > 0)
                return Ok(reviews);
            return NotFound(new { Message = "No reviews found for this product" });
        }

        [HttpPost("AddReview")]
        public async Task<IActionResult> AddReview([FromBody] ReviewModel review)
        {
            if (review == null) return BadRequest(new { Message = "Invalid review data" });

            var result = await _reviewServices.AddReviewAsync(review);
            if (result) return Ok(new { Message = "Review added successfully" });
            return BadRequest(new { Message = "Failed to add review" });
        }

        [HttpDelete("AdminDeleteReview/{id}")]
        public async Task<IActionResult> AdminDeleteReview(int id)
        {
            if (id <= 0) return BadRequest(new { Message = "Invalid review ID" });

            var result = await _reviewServices.AdminDeleteReviewAsync(id);
            if (result) return Ok(new { Message = "Review deleted successfully" });
            return NotFound(new { Message = "Review not found" });
        }


    }
}