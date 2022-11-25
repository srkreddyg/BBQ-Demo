using BBQN.IdentityManagement.API.Common;
using BBQN.IdentityManagement.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BBQN.IdentityManagement.API.Controllers
{
    /// <summary>
    /// Controller For Authentication ,OTP Generation
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginController> _logger;
        private readonly ILoginService _loginService;
        public LoginController(IConfiguration configuration,ILogger<LoginController> logger, ILoginService loginService)
        {
            _configuration = configuration;
            _logger = logger;
            _loginService = loginService;
        }


        /// <summary>
        /// OTP Generation after Validating Employee ID
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("GenerateOTP")]
        public async Task<IActionResult> GenerateOTP(string employeeID, LoginType type)
        {
            //TO DO: Validate Employee ID ZIng HR API BBQN
            var otp = await _loginService.GenerateOTP(employeeID, type);
            if (otp == 0) return BadRequest(new { Status= "Fail" ,Message= "Incorrect details entered. Please try again " });
            return Ok(new { OTP = otp, Status = "Success" });
          
        }

        /// <summary>
        /// Authentication Peforming
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="otpProvided"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("Authenticate")]
        public async Task<IActionResult> Authenticate(string employeeID, int otpProvided, LoginType type,bool isAdminLogin= false)
        {
            var isUserAuthenticated = await _loginService.IsUserAuthenticated(employeeID, otpProvided,type);
            if (isUserAuthenticated)
            {
                var token = GenerateJwtToken(employeeID, isAdminLogin);
                return Ok(new { Token = token, Status = "Success" });
            }
            return BadRequest(new { Status = "Fail", Message = "Incorrect details entered. Please try again " });

        }

        /// <summary>
        /// Authentication Peforming based on Token
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("AuthenticateToken")]
        public IActionResult AuthenticateToken(int employeeID)
        {
            //Validate Employee ID It is from ZingHR or On Premise DB
            return  Ok(new { Status = "Success" });
        }

        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet(nameof(GetResult))]
        public IActionResult GetResult()
        {
            try
            {
                return Ok("API Validated");
            }
            catch (Exception ex) {

                throw new Exception(ex.ToString());
              
            }
        }
        #region private Methods
        /// <summary>
        /// Return Token After successful Login
        /// </summary>
        /// <param name="employeeID"></param>
        /// <param name="isAdminLogin= Browser Bases Login"></param>
        /// <returns></returns>
        private string GenerateJwtToken(string employeeID, bool isAdminLogin)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:key"]);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("EmployeeID", employeeID) }),
                Expires = isAdminLogin? DateTime.UtcNow.AddHours(6): DateTime.UtcNow.AddDays(45),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        #endregion

    }
}
