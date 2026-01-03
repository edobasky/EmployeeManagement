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
    public class EmployeeMasterController(EmployeeDBContext context) : ControllerBase
    {
        // -------------------- GET ALL (NORMAL) --------------------
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees = await context.Employees
                .OrderBy(e => e.EmployeeId)
                .ToListAsync();

            return Ok(employees);
        }

        // -------------------- GET PAGINATED --------------------
        // api/EmployeeMaster/paged?page=1&pageSize=10
        //   [HttpGet("paged")]
        //   public async Task<IActionResult> GetPaged(
        //       int page = 1,
        //       int pageSize = 10)
        //   {
        //       if (page <= 0 || pageSize <= 0)
        //           return BadRequest("Invalid paging parameters");
        //
        //       var totalRecords = await context.Employees.CountAsync();
        //
        //       var data = await context.Employees
        //           .OrderBy(e => e.EmployeeId)
        //           .Skip((page - 1) * pageSize)
        //           .Take(pageSize)
        //           .ToListAsync();
        //
        //       return Ok(new
        //       {
        //           page,
        //           pageSize,
        //           totalRecords,
        //           totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
        //           data
        //       });
        //   }


        // api/EmployeeMaster/paged?page=1&pageSize=10&search=emma&sortBy=name&sortDir=asc
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(
            int page = 1,
            int pageSize = 10,
            string? search = null,
            string sortBy = "EmployeeId",
            string sortDir = "asc")
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest("Invalid paging parameters");

            IQueryable<Employee> query = context.Employees;

            // -------- FILTER --------
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(e =>
                    e.Name.Contains(search) ||
                    e.Email.Contains(search) ||
                    e.ContactNo.Contains(search));
            }

            // -------- SORT --------
            query = (sortBy.ToLower(), sortDir.ToLower()) switch
            {
                ("name", "desc") => query.OrderByDescending(e => e.Name),
                ("name", _) => query.OrderBy(e => e.Name),

                ("email", "desc") => query.OrderByDescending(e => e.Email),
                ("email", _) => query.OrderBy(e => e.Email),

                ("createddate", "desc") => query.OrderByDescending(e => e.CreatedDate),
                ("createddate", _) => query.OrderBy(e => e.CreatedDate),

                _ when sortDir.ToLower() == "desc"
                    => query.OrderByDescending(e => e.EmployeeId),

                _ => query.OrderBy(e => e.EmployeeId)
            };

            var totalRecords = await query.CountAsync();

            var data = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                page,
                pageSize,
                totalRecords,
                totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                data
            });
        }



        // -------------------- GET BY ID --------------------
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await context.Employees.FindAsync(id);

            if (employee == null)
                return NotFound("Employee not found");

            return Ok(employee);
        }

        // -------------------- CREATE --------------------
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Employee model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Uniqueness checks
            var exists = await context.Employees.AnyAsync(e =>
                e.ContactNo == model.ContactNo ||
                e.Email == model.Email);

            if (exists)
                return Conflict("Contact number or email already exists");

            model.CreatedDate = DateTime.UtcNow;
            model.ModifiedDate = DateTime.UtcNow;

            context.Employees.Add(model);
            await context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = model.EmployeeId },
                model);
        }

        // -------------------- UPDATE --------------------
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Employee model)
        {
            if (id != model.EmployeeId)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employee = await context.Employees.FindAsync(id);
            if (employee == null)
                return NotFound("Employee not found");

            // Uniqueness check (exclude current record)
            var duplicate = await context.Employees.AnyAsync(e =>
                e.EmployeeId != id &&
                (e.ContactNo == model.ContactNo ||
                 e.Email == model.Email));

            if (duplicate)
                return Conflict("Contact number or email already exists");

            employee.Name = model.Name;
            employee.ContactNo = model.ContactNo;
            employee.Email = model.Email;
            employee.City = model.City;
            employee.State = model.State;
            employee.Pincode = model.Pincode;
            employee.AltContactNo = model.AltContactNo;
            employee.Address = model.Address;
            employee.designationId = model.designationId;
            employee.ModifiedDate = DateTime.UtcNow;

            await context.SaveChangesAsync();

            return Ok(employee);
        }

        // -------------------- DELETE --------------------
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await context.Employees.FindAsync(id);

            if (employee == null)
                return NotFound("Employee not found");

            context.Employees.Remove(employee);
            await context.SaveChangesAsync();

            return NoContent();

        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await context.Employees
                .SingleOrDefaultAsync(x => x.Email == request.email && x.ContactNo == request.contactNo);

            if (user == null)
                return Unauthorized("Invalid Credentials");

           // var userDesig = await context.Designations.FirstOrDefaultAsync(x => x.designationId == user.designationId);

            // SUCCESS (JWT usually goes here)
            return Ok(new
            {
                message = "Login successful",
                Data = new
                {
                    user.EmployeeId,
                    user.Name,
                    user.Email,
                    user.ContactNo,
                    user.designationId,
                    user.Designation?.designationName,
                    user.Role
                }
            });
        }
    }
}

