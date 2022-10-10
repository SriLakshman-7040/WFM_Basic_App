using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WFM_API.Models;
using WFM_Core.Abstraction;
using WFM_Domain.Models;

namespace WFM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly WfmDbContext _context;
        private readonly JWTSetting _jwtsetting;
        private readonly IRefreshTokenGenerator _refreshTokenGenerator;
        public UsersController(WfmDbContext context,IOptions<JWTSetting> jwtoptions, IRefreshTokenGenerator refreshTokenGenerator)
        {
            _context = context;
            _jwtsetting = jwtoptions.Value;
            _refreshTokenGenerator = refreshTokenGenerator;
        }
        [NonAction]
        public UserDetailsWithTokenResponceDto AuthenticateUser(string userName , Claim[] userClaims)
        {
            UserDetailsWithTokenResponceDto tokenResponce = new UserDetailsWithTokenResponceDto();
            var tokenSecurityKey = Encoding.UTF8.GetBytes(_jwtsetting.securitykey);
            var tokenHandler = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(tokenSecurityKey), SecurityAlgorithms.HmacSha256));
            tokenResponce.JWTToken = new JwtSecurityTokenHandler().WriteToken(tokenHandler);
            tokenResponce.RefreshToken = _refreshTokenGenerator.GenerateToken(userName);
            return tokenResponce;
        }
        [Route(template:"Authenticate")]
        [HttpPost]
        public ActionResult<UserDetailsWithTokenResponceDto> AuthenticateUser([FromBody] UserCredential UserCredential)
        {
            if(!ModelState.IsValid)
                return BadRequest("Not a Valid User Details");

            UserDetailsWithTokenResponceDto tokenResponce = new UserDetailsWithTokenResponceDto();
            var userVaild = _context.Users.FirstOrDefault(u => u.Username == UserCredential.UserName && u.Password == UserCredential.Password);
            if(userVaild == null) return Unauthorized();

            // if user is vaild , create a jwt token .
            var tokenhandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_jwtsetting.securitykey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.Name, userVaild.Username),
                    }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256),
            };
            var token = tokenhandler.CreateToken(tokenDescriptor);
            string finalToken = tokenhandler.WriteToken(token);

            tokenResponce.JWTToken = finalToken;
            tokenResponce.RefreshToken = _refreshTokenGenerator.GenerateToken(UserCredential.UserName);
            tokenResponce.Username = userVaild.Username;
            tokenResponce.Email = userVaild.Email;
            tokenResponce.Name = userVaild.Name;
            tokenResponce.Role = userVaild.Role;
            if(tokenResponce.Role.Equals("Manager"))
            {
                return StatusCode(StatusCodes.Status200OK, finalToken);
            }

            return StatusCode(StatusCodes.Status202Accepted, finalToken); 
        }

        [Route(template:"Refresh")]
        [HttpPost()]
        public IActionResult Refresh([FromBody] UserDetailsWithTokenResponceDto tokenResponce)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(tokenResponce.JWTToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtsetting.securitykey)),
                ValidateIssuer = false,
                ValidateAudience = false
            }, out securityToken);

            var _token = securityToken as JwtSecurityToken;
            if(_token != null && !_token.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
            {
                return Unauthorized();
            }
            var username = principal.Identity.Name;
            
            var _refTable = _context.TblRefreshToken.FirstOrDefault(u => u.UserName == username && u.RefreshToken == tokenResponce.RefreshToken);
            if(_refTable == null)
            {
                return Unauthorized();
            }
            UserDetailsWithTokenResponceDto _result = AuthenticateUser(username, principal.Claims.ToArray());
            return Ok(_result);
        }


    }
}
