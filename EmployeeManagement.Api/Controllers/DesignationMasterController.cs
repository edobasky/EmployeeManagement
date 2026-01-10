using EmployeeManagement.Api.Data;
using EmployeeManagement.Api.Models;
using EmployeeManagement.Api.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignationMasterController(EmployeeDBContext context) : ControllerBase
    {

        // GET: api/DesignationMaster
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //  var data = await context.Designations.ToListAsync();
            var data = await (from 
                d in context.Designations join
                dep in context.Departments on 
                d.departmentId equals dep.departmentId
                select new DesignationDto
                {
                    DesignationId = d.designationId,
                    DepartmentId = dep.departmentId,
                    DepartmentName = dep.departmentName,
                    DesignationName = d.designationName
                }).ToListAsync();
            return Ok(data);
        }

        // GET: api/DesignationMaster/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var designation = await context.Designations.FindAsync(id);

            if (designation == null)
                return NotFound("Designation not found");

            return Ok(designation);
        }

        // POST: api/DesignationMaster
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Designation model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            context.Designations.Add(model);
            await context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = model.designationId },
                model);
        }

        // PUT: api/DesignationMaster/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Designation model)
        {
            if (id != model.designationId)
                return BadRequest("ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var exists = await context.Designations.FirstOrDefaultAsync(x => x.designationId == id);

            if (exists == null)
                return NotFound("Designation not found");

            exists.designationName = model.designationName;
            exists.designationId = model.designationId;
            exists.departmentId = model.departmentId;
            await context.SaveChangesAsync();

            return Ok(model);
        }

        // DELETE: api/DesignationMaster/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var designation = await context.Designations.FindAsync(id);

            if (designation == null)
                return NotFound("Designation not found");

            context.Designations.Remove(designation);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}

