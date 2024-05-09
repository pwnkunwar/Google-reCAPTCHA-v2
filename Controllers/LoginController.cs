using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Google_reCaptcha_v2.Models;
using Google_reCaptcha_v2.Services;

namespace Google_reCaptcha_v2.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Login login)
        {
            if (login == null)
            {
                return BadRequest("Invalid login data.");
            }

          
            if (string.IsNullOrWhiteSpace(login.RecaptchaToken))
            {
                return BadRequest("reCAPTCHA token is required.");
            }

            
            bool success = await VerifyReCaptcha(login.RecaptchaToken);
            if (!success)
            {
                return BadRequest("reCAPTCHA verification failed.");
            }

            
            return Ok("LoginSuccess");
        }

        private async Task<bool> VerifyReCaptcha(string recaptchaToken)
        {
            try
            {
                
                string secretKey = _configuration["GoogleReCaptcha:SecretKey"];

                
                return await ReCaptchaService.verifyReCaptchaV2(recaptchaToken, secretKey);
            }
            catch (Exception ex)
            {
               
                Console.WriteLine("reCAPTCHA verification error: " + ex.Message);
                return false;
            }
        }
    }
}
