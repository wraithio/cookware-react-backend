using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cookware_react_backend.Context;
using cookware_react_backend.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace cookware_react_backend.Services
{
    public class AdminServices
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _config;
        public AdminServices(DataContext dataContext, IConfiguration config)
        {
            _dataContext = dataContext;
            _config = config;
        }

        public async Task<List<AdminModel>> SeeAllAdmins() => await _dataContext.Admins.ToListAsync();

        public async Task<bool> CreateAdmin(AdminDTO newUser)
        {
            if (newUser.Username != null && await DoesUserExist(newUser.Username)) return false;
            PasswordDTO hashedPassword = HashPassword(newUser.Passkey.ToString());
            AdminModel userToAdd = new()
            {
                Username = newUser.Username,
                Salt = hashedPassword.Salt,
                Hash = hashedPassword.Hash,
                CreatedDate = DateTime.Now,
                LastLogin = DateTime.Now
            };
            await _dataContext.Admins.AddAsync(userToAdd);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<string> Login(int passkey)
        {
            AdminModel? foundUser = await _dataContext.Admins.SingleOrDefaultAsync();

            if (foundUser == null) return null;
            if (!VerifyPassword(passkey.ToString(), foundUser.Salt, foundUser.Hash)) return null;
            foundUser.LastLogin = DateTime.UtcNow;
            _dataContext.Admins.Update(foundUser);
            await _dataContext.SaveChangesAsync();
            return GenerateJWTToken(new List<Claim>());
        }

        public async Task<bool> DeactivateAdmin(int id)
        {
            AdminModel? userToDeactivate = await _dataContext.Admins.FindAsync(id);
            if (userToDeactivate == null) return false;
            userToDeactivate.IsActive = false;
            _dataContext.Admins.Update(userToDeactivate);
            return await _dataContext.SaveChangesAsync() != 0;
        }


        private string GenerateJWTToken(List<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));

            //Now to encrypt our secret key
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: "https://cookwareinterface-drgnfkhdevbvd6gw.westus-01.azurewebsites.net/",
                audience: "https://cookwareinterface-drgnfkhdevbvd6gw.westus-01.azurewebsites.net/",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signingCredentials
            );

            //token anatomy:
            //asdfernn3435.asdfnwrn224b345h.ihihfw3fb
            //Header: asdfernn3435
            //Payload: asdfnwrn224b345h this will have info about the token, including the expiration date
            //Signature: ihihfw3fb encrypt and combine headder and payload using secret key
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public async Task<bool> DeleteAdmin(int id)
        {
            AdminModel? userToDelete = await _dataContext.Admins.FindAsync(id);
            if (userToDelete == null) return false;
            _dataContext.Admins.Remove(userToDelete);
            return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<AdminModel> GetAdminByUsername(string username) => await _dataContext.Admins.SingleOrDefaultAsync(user => user.Username == username);

        private static bool VerifyPassword(string password, string salt, string hash)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            string newHash;

            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 310000, HashAlgorithmName.SHA256))
            {
                newHash = Convert.ToBase64String(deriveBytes.GetBytes(32));
            }
            return hash == newHash;
        }

        private async Task<bool> DoesUserExist(string username) => await _dataContext.Admins.SingleOrDefaultAsync(users => users.Username == username) != null;
        // SingleorDefault finds first or default instance of whatever is in parameters


        private static PasswordDTO HashPassword(string password)
        {
            byte[] saltBytes = RandomNumberGenerator.GetBytes(64);

            string salt = Convert.ToBase64String(saltBytes);

            string hash;

            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 310000, HashAlgorithmName.SHA256))
            {
                hash = Convert.ToBase64String(deriveBytes.GetBytes(32));
            }

            PasswordDTO hashedPassword = new();
            hashedPassword.Salt = salt;
            hashedPassword.Hash = hash;
            return hashedPassword;
        }
    }
}