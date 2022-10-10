using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WFM_Core.Abstraction;
using WFM_Domain.Models;

namespace WFM_Service.Services
{
    public class RefreshTokenGenerator : IRefreshTokenGenerator
    {
        private readonly WfmDbContext _context;
        public RefreshTokenGenerator(WfmDbContext context)
        {
            _context = context;
        }
        public string GenerateToken(string userName)
        {
            var randomNumber = new byte[32];
            using(var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(randomNumber);
                string refreshToken = Convert.ToBase64String(randomNumber);

                var _userExists = _context.TblRefreshToken.FirstOrDefault(u => u.UserName == userName);
                TblRefreshToken tblRefreshToken = new()
                {                    
                    UserName = userName,
                    RefreshToken = refreshToken,                    
                    IsActive = true
                };
                if (_userExists != null)
                {
                    _userExists.RefreshToken = refreshToken;
                    _context.SaveChanges();                   
                }
                else
                {
                    tblRefreshToken.TokenId = new Random().Next().ToString();
                    _context.TblRefreshToken.Add(tblRefreshToken);
                    _context.SaveChanges();
                }
                return refreshToken;
            }
        }
    }
}
