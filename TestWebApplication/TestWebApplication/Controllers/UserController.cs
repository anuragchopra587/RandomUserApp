using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text;
using TestDataAccess.Contracts;
using TestServiceModel;

namespace TestWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRandomUserDataService _randomUserDataService;

        public UserController(IRandomUserDataService randomUserDataService)
        {
            _randomUserDataService = randomUserDataService;
        }

        private const string Username = "admin";
        private const string Password = "password";

        [HttpGet("getUser")]
        public async Task<IActionResult> GetUser()
        {
            // Basic Authentication
            if (!HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                HttpContext.Response.Headers.Add("WWW-Authenticate", "Basic");
                return Unauthorized();
            }

            var authHeader = AuthenticationHeaderValue.Parse(HttpContext.Request.Headers["Authorization"]);
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
            var username = credentials[0];
            var password = credentials[1];

            if (username != Username || password != Password)
                return Unauthorized();

            try
            {
                RandomUserApiResponse userData = await _randomUserDataService.GetRandomUserData();
                return Ok(userData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

