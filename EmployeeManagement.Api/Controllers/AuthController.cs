using EmployeeManagement.Api.Data;
using EmployeeManagement.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly EmployeeDBContext _context;
        private readonly PasswordHasher<Employee> _passwordHasher;

        public AuthController(EmployeeDBContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<Employee>();
        }

        // POST: api/auth/login
       /* [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.Employees
                .SingleOrDefaultAsync(x => x.Email == request.email);

            if (user == null)
                return Unauthorized("Invalid email or password");

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.password);

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid email or password");

            // SUCCESS (JWT usually goes here)
            return Ok(new
            {
                message = "Login successful",
                userId = user.EmployeeId,
                user.Email,
                user.Name
            });
        }*/
    }
}

/*MapWhenExtensions creating account=====>
var hasher = new PasswordHasher<Employee>();

employee.PasswordHash = hasher.HashPassword(employee, "PlainTextPassword");*/
