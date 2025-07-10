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
    public class DetailsController : ControllerBase
    {
        private readonly DetailsServices _detailsServices;
        public DetailsController(DetailsServices detailsServices)
        {
            _detailsServices = detailsServices;
        }

        [HttpGet("GetAllDetails")]
        public async Task<IActionResult> GetAllDetails()
        {
            var details = await _detailsServices.GetAllDetailsAsync();
            if (details != null) return Ok(details);
            return BadRequest(new { Message = "No Details Found" });
        }

        [HttpGet("GetDetailsByProductId/{id}")]
        public async Task<IActionResult> GetDetailsByProductId(int id)
        {
            var details = await _detailsServices.GetDetailsByProductIdAsync(id);
            if (details != null) return Ok(details);
            return NotFound(new { Message = "Details not found for the specified product ID" });
        }

        [HttpPost("AddDetails")]
        public async Task<IActionResult> AddDetails([FromBody] DetailsModel details)
        {
            if (details == null) return BadRequest(new { Message = "Invalid details data" });

            var result = await _detailsServices.AddDetailsAsync(details);
            if (result) return Ok(new { Message = "Details added successfully" });
            return BadRequest(new { Message = "Failed to add details" });
        }

        [HttpDelete("HardDeleteDetails/{id}")]
        public async Task<IActionResult> HardDeleteDetails(int id)
        {
            var result = await _detailsServices.HardDeleteDetailsEntriesAsync(id);
            if (result) return Ok(new { Message = "Details deleted successfully" });
            return BadRequest(new { Message = "Failed to delete details" });
        }
    }
}