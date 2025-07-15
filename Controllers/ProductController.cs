using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cookware_react_backend.Models;
using cookware_react_backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace cookware_react_backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductServices _productServices;
        public ProductController(ProductServices productServices)
        {
            _productServices = productServices;
        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            var posts = await _productServices.GetAllProductsAsync();
            if (posts != null) return Ok(posts);
            return BadRequest(new { Message = "No Products" });
        }

        [HttpGet("GetLiveProducts")]
        public async Task<IActionResult> GetLiveProducts()
        {
            var posts = await _productServices.GetLiveProductsAsync();
            if (posts != null) return Ok(posts);
            return BadRequest(new { Message = "No Live Products" });
        }

        [HttpGet("GetProductById/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var post = await _productServices.GetProductByIdAsync(id);
            if (post != null) return Ok(post);
            return NotFound(new { Message = "Product not found" });
        }

        [HttpGet("GetProductsByCategory/{category}")]
        public async Task<IActionResult> GetProductsByCategory(string category)
        {
            var posts = await _productServices.GetProductsbyCategoryAsync(category);
            if (posts != null) return Ok(posts);
            return NotFound(new { Message = "No products found in this category" });
        }

        [HttpGet("GetProductByName/{name}")]
        public async Task<IActionResult> GetProductByName(string name)
        {
            var post = await _productServices.GetProductByProductNameAsync(name);
            if (post != null) return Ok(post);
            return NotFound(new { Message = "Product not found" });
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody] ProductModel product)
        {
            if (product == null) return BadRequest(new { Message = "Invalid product data" });

            var result = await _productServices.AddProductAsync(product);
            if (result) return Ok(new { Message = "Product added successfully" });
            return BadRequest(new { Message = "Failed to add product" });
        }

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductModel product)
        {
            if (product == null) return BadRequest(new { Message = "Invalid product data" });

            var result = await _productServices.UpdateProductEntryAsync(product);
            if (result) return Ok(new { Message = "Product updated successfully" });
            return BadRequest(new { Message = "Failed to update product" });
        }

        [HttpDelete("HardDeleteProduct/{id}")]
        public async Task<IActionResult> HardDeleteProduct(int id)
        {
            var result = await _productServices.HardDeleteProductEntriesAsync(id);
            if (result) return Ok(new { Message = "Product deleted successfully" });
            return BadRequest(new { Message = "Failed to delete product" });
        }

        [HttpGet("GetArchivedProducts")]
        public async Task<IActionResult> GetArchivedProducts()
        {
            var posts = await _productServices.GetArchivedProductsAsync();
            if (posts != null) return Ok(posts);
            return BadRequest(new { Message = "No Archived Products" });
        }

    }
}