
using API.Data;
using API.Dtos;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class UserController(
        ITokenService _tokenService,
        UserManager<AppUser> _userManager,
        SignInManager<AppUser> _signInManager,
        TenantDbcontext _context
    ) : BaseApiController
    {
        protected async Task<AppUser> GetauthenticatedUserAsync()
        {
            var email = User.GetEmail();

            if (string.IsNullOrEmpty(email)) return null;

            return await _userManager.FindByEmailAsync(email);
        }

        [HttpGet("emailexists")]
        [Authorize(Roles = "Admin, UserAdmin")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {

            if (string.IsNullOrEmpty(email)) return BadRequest(new { Errors = new[] { "Email is required" } });

            var tenantHeader = Request.Headers["tenant"].ToString();

            if (string.IsNullOrEmpty(tenantHeader)) return BadRequest(new { Errors = new[] { "Tenant Id is required" } });

            var tenantExists = await _context.Tenants.AnyAsync(t => t.Id == tenantHeader);

            if (!tenantExists) return BadRequest(new { Error = new[] { $"Tenant {tenantHeader} does not exist" } });

            var userExists = await _userManager.Users
                .AsNoTracking()
                .AnyAsync(u => u.TenantId == tenantHeader && u.Email == email);

            return await _userManager.FindByEmailAsync(email) != null;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var tenantHeader = Request.Headers["tenant"].ToString();

                var tenantExists = await _context.Tenants
                .AsNoTracking()
                .AnyAsync(t => t.Id == tenantHeader);

                if (!tenantExists)
                {
                    return BadRequest(new { Errors = new[] { $"Tenant '{tenantHeader}' does not exist" } });
                }

                if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
                {
                    return BadRequest(new { Errors = new[] { "Email address is in use: ", registerDto.Email } });
                }

                var isFirstUser = !await _userManager.Users.AnyAsync(u => u.TenantId == tenantHeader);

                var appUser = new AppUser
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                    TenantId = tenantHeader
                };

                var result = await _userManager.CreateAsync(appUser, registerDto.Password);

                if (!result.Succeeded) return BadRequest(new { Errors = new[] { "Problem creating user" } });


                var role = isFirstUser ? "ADMIN" : "MEMBER";

                var roleAddResult = await _userManager.AddToRoleAsync(appUser, role);

                if (!roleAddResult.Succeeded) return BadRequest(new { Errors = new[] { "Failed to add to role" } });

                return new UserDto
                {
                    UserName = appUser.UserName,
                    Email = appUser.Email,
                    Token = await _tokenService.CreateTokenAsync(appUser),
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var tenantHeader = Request.Headers["tenant"].ToString();

                var tenantExists = await _context.Tenants
                .AsNoTracking()
                .AnyAsync(t => t.Id == tenantHeader);

                if (!tenantExists)
                {
                    return BadRequest(new { Errors = new[] { $"Tenant '{tenantHeader}' does not exist" } });
                }

                var user = await _userManager.FindByEmailAsync(loginDto.Email);

                if (user == null | !await _userManager.CheckPasswordAsync(user, loginDto.Password)) return Unauthorized(new { Error = new[] { "User Unauthorized" } });

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
                if (!result.Succeeded) return Unauthorized(new { Error = new[] { "User Unauthorized" } });

                return new UserDto()
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Token = await _tokenService.CreateTokenAsync(user)
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("allusers")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            try
            {
                var tenantHeader = Request.Headers["tenant"].ToString();

                var tenantExists = await _context.Tenants
                .AsNoTracking()
                .AnyAsync(t => t.Id == tenantHeader);

                if (!tenantExists)
                {
                    return BadRequest(new { Errors = new[] { $"Tenant '{tenantHeader}' does not exist" } });
                }

                var users = await _userManager.Users.Where(u => u.TenantId == tenantHeader).ToListAsync();

                if (users == null || users.Count == 0) return NotFound(new { Errors = new[] { "No users found for this tenant" } });

                var userDtos = users.Select(user => new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email
                }).ToList();

                return Ok(userDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Errors = new[] { "An unexpected error occurred", ex.Message } });
            }
        }

        [HttpDelete("{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDto>> DeleteUser(string userId)
        {
            try
            {
                var tenantHeader = Request.Headers["tenant"].ToString();

                if (string.IsNullOrEmpty(tenantHeader))
                {
                    return BadRequest(new { Errors = new[] { "Tenant Id is required in the header" } });
                }

                var tenantExists = await _context.Tenants
                    .AsNoTracking()
                    .AnyAsync(t => t.Id == tenantHeader);

                if (!tenantExists)
                {
                    return BadRequest(new { Errors = new[] { $"Tenant '{tenantHeader}' does not exist" } });
                }

                var user = await _userManager.Users
                    .Where(u => u.TenantId == tenantHeader && u.Id == userId)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return NotFound(new { Errors = new[] { $"User with Id '{userId}' not found in tenant '{tenantHeader}'" } });
                }

                var result = await _userManager.DeleteAsync(user);

                if (!result.Succeeded)
                {
                    return BadRequest(new { Errors = new[] { "Failed to delete user" } });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Errors = new[] { "An unexpected error occurred", ex.Message } });
            }
        }
    }
}