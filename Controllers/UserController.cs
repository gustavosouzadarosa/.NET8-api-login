using ApiLogin.Data;
using ApiLogin.Model;
using ApiLogin.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using ApiLogin.Interfaces;

namespace ApiLogin.Controllers
{
    [ApiController]
    [Route("Login")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticationService _authenticationService;

        public UsersController(UserManager<User> userManager,
                               IAuthenticationService authenticationService)
        {
            _userManager = userManager;
            _authenticationService = authenticationService;
        }

        [HttpPost("GenerateToken")]
        public async Task<IActionResult> GenerateToken([FromBody] LoginViewModel model)
        {
            var token = await _authenticationService.GenerateToken(model.UserName, model.Password);

            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(new { token });
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                CampoAdicional = model.CampoAdicional
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (model.IsAdmin)
                {
                    await _userManager.AddToRoleAsync(user, "Administrator");
                }
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return BadRequest(ModelState);
            }

            return Ok("User created successfully!");
        }

        [HttpGet("GetUser/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("User ID is required.");
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var retorno = new ReturnGetUserViewModel
            {
                UserName = user.UserName,
                CampoAdicional = user.CampoAdicional
            };

            return Ok(retorno);
        }

        [HttpPut("EditUser")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditUser([FromBody] EditUserViewModel model)
        {
            if (string.IsNullOrEmpty(model.UserId))
            {
                return BadRequest("User ID is required.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.CampoAdicional = model.CampoAdicional ?? user.CampoAdicional;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);

                }

                return BadRequest(ModelState);
            }

            return Ok("User updated successfully!");
        }
    }
}