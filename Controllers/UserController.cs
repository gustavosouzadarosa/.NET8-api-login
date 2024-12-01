using ApiLogin.Data;
using ApiLogin.Model;
using ApiLogin.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using ApiLogin.Interfaces;
using System.Security.Claims;
using ApiLogin.Implementations;

namespace ApiLogin.Controllers
{
    [ApiController]
    [Route("Login")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticationService _authenticationService;
        private readonly IBasicValidations _basicValidations;

        public UsersController(UserManager<User> userManager,
                               IAuthenticationService authenticationService,
                               IBasicValidations basicValidations)
        {
            _userManager = userManager;
            _authenticationService = authenticationService;
            _basicValidations = basicValidations;
        }

        [HttpPost("GenerateToken")]
        [AllowAnonymous]
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
            // This method does not require authentication and authorization for development and testing environments only.
            // In a production environment, we should implement validation methods here.
            // var validationResult = _basicValidations.IsAdministratorUser();

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
        public async Task<IActionResult> GetUser(string id)
        {
            var validationResult = _basicValidations.IsAuthenticatedUser();

            if (!validationResult)
            {
                return BadRequest("User does not have permission to access this resource.");
            }

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
        public async Task<IActionResult> EditUser([FromBody] EditUserViewModel model)
        {
            var validationResult = _basicValidations.IsAdministratorUser();

            if (!validationResult.Authenticated || !validationResult.Administrator)
            {
                return BadRequest("User does not have permission to access this resource.");
            }

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