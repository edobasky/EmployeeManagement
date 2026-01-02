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
            context.Departments.Add(dept);
            context.SaveChanges();
            return Ok("Department Added Successfully");
        }

        [HttpPost("UpdateDepartment")]
        public IActionResult UpdateDepartment([FromBody] Department dept)
        {
            var checkDept = context.Departments.FirstOrDefault(x => x.departmentId == dept.departmentId);
            if (checkDept is null) return Ok("No Department found");

            checkDept.departmentName = dept.departmentName;
            checkDept.isActive = dept.isActive;
            context.SaveChanges();
            return Ok("Department Updated Successfully");
        }


        [HttpPost("RemoveDepartment/{id}")]
        public IActionResult RemoveDepartment(int id)
        {
            var Dept = context.Departments.FirstOrDefault(x => x.departmentId == id);
            if (Dept is null) return Ok("No Department found");

            context.Remove(Dept);
            context.SaveChanges();
            return Ok("Department Deleted Successfully");
        }
    }

}
