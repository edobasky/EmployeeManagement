using EmployeeManagement.Api.Data;
using EmployeeManagement.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentMasterController(EmployeeDBContext context) : ControllerBase
    {

        [HttpGet("GetAllDepartments")]
        public IActionResult GetAllDepartment()
        {
            var depList = context.Departments.ToList();
            return Ok(depList);
        }

        [HttpPost("AddDepartment")]
        public IActionResult AddDepartment([FromBody] Department dept)
        {
            if (string.IsNullOrWhiteSpace(dept.departmentName))
                return BadRequest("Department name cannot be empty");

            var existingDept = context.Departments.FirstOrDefault(x => x.departmentName == dept.departmentName);
            if (existingDept is not null)
                return BadRequest("Department name already exists");
            dept.departmentName.ToUpper();
            context.Departments.Add(dept);
            context.SaveChanges();
            return Created("Department Added Successfully",dept);
        }

        [HttpPut("UpdateDepartment")]
        public IActionResult UpdateDepartment([FromBody] Department dept)
        {
            var checkDept = context.Departments.FirstOrDefault(x => x.departmentId == dept.departmentId);
            if (checkDept is null) return Ok(new { Message = "No Department found" });

            checkDept.departmentName = dept.departmentName;
            checkDept.isActive = dept.isActive;
            context.SaveChanges();
            return Ok( new { Message = "Department Updated Successfully" });
        }


        [HttpDelete("RemoveDepartment/{id}")]
        public IActionResult RemoveDepartment(int id)
        {
            var Dept = context.Departments.FirstOrDefault(x => x.departmentId == id);
            if (Dept is null) return Ok(new { Message = "No Department found" });

            context.Remove(Dept);
            context.SaveChanges();
            return Ok(new { Message = "Department Deleted Successfully" });
        }
    }

}
