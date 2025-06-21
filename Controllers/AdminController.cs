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
    public class AdminController : ControllerBase
    {
        private readonly AdminServices _adminServices;

        public AdminController(AdminServices adminServices)
        {
            _adminServices = adminServices;
        }

        [HttpGet("GetAllAdmins")]
        public async Task<List<AdminModel>> GetAllAdmins() => await _adminServices.SeeAllAdmins();

        [HttpPost("CreateAdmin")]
        public async Task<IActionResult> CreateAdmin([FromBody] AdminDTO admin)
        {
            if (admin == null) return BadRequest(new { Message = "Invalid admin data" });

            var result = await _adminServices.CreateAdmin(admin);
            if (result) return Ok(new { Message = "Admin created successfully" });
            return BadRequest(new { Message = "Failed to create admin" });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] int passkey)
        {
            if (passkey <= 0) return BadRequest(new { Message = "Invalid passkey" });

            var token = await _adminServices.Login(passkey);
            if (token != null) return Ok(new { Token = token });
            return Unauthorized(new { Message = "Invalid credentials" });
        }

        [HttpDelete("DeleteAdmin/{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            if (id <= 0) return BadRequest(new { Message = "Invalid admin ID" });

            var result = await _adminServices.DeleteAdmin(id);
            if (result) return Ok(new { Message = "Admin deleted successfully" });
            return NotFound(new { Message = "Admin not found" });
        }

        [HttpPut("DeactivateAdmin/{id}")]
        public async Task<IActionResult> DeactivateAdmin(int id)
        {
            if (id <= 0) return BadRequest(new { Message = "Invalid admin ID" });

            var result = await _adminServices.DeactivateAdmin(id);
            if (result) return Ok(new { Message = "Admin deactivated successfully" });
            return NotFound(new { Message = "Admin not found" });
        }

        [HttpPut("UpdateLoginDate/{username}")]
        public async Task<IActionResult> UpdateLoginDate(string username)
        {
            if (string.IsNullOrEmpty(username)) return BadRequest(new { Message = "Invalid username" });

            var result = await _adminServices.UpdateLoginDate(username);
            if (result) return Ok(new { Message = "Login date updated successfully" });
            return NotFound(new { Message = "Admin not found" });
        }
    }
}