using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using asp_project.Models;
using asp_project.Data;

namespace asp_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : ControllerBase
    {
        private readonly AcademyContext _context;

        public DepartmentsController(AcademyContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public ActionResult<IEnumerable<Department>> GetAllDepartments()
        {
            var departments = _context.Departments.ToList();
            return Ok(departments);
        }

        [HttpPost("Add")]
        [Authorize(Roles = "appadmin")]
        public IActionResult AddDepartment([FromBody] Department department)
        {
            if (department == null)
            {
                return BadRequest("Department is null.");
            }

            _context.Departments.Add(department);
            _context.SaveChanges();

            return Ok("Department added successfully.");
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "appadmin")]
        public IActionResult DeleteDepartment(int id)
        {
            var department = _context.Departments.Find(id);
            if (department == null)
            {
                return NotFound("Department not found.");
            }

            _context.Departments.Remove(department);
            _context.SaveChanges();

            return Ok("Department deleted successfully.");
        }

        [HttpPut("Edit")]
        [Authorize(Roles = "appadmin")]
        public IActionResult EditDepartment([FromBody] Department department)
        {
            if (department == null || department.Id == 0)
            {
                return BadRequest("Invalid department data.");
            }

            var existingDepartment = _context.Departments.Find(department.Id);
            if (existingDepartment == null)
            {
                return NotFound("Department not found.");
            }

            existingDepartment.Name = department.Name;
            existingDepartment.Head = department.Head;

            _context.Departments.Update(existingDepartment);
            _context.SaveChanges();

            return Ok("Department updated successfully.");
        }

        [HttpPost("AddTeacher/{departmentId}/{teacherId}")]
        [Authorize(Roles = "appadmin")]
        public IActionResult AddTeacherToDepartment(int departmentId, int teacherId)
        {
            var department = _context.Departments.Find(departmentId);
            if (department == null)
            {
                return NotFound("Department not found.");
            }

            var teacher = _context.Teachers.Find(teacherId);
            if (teacher == null)
            {
                return NotFound("Teacher not found.");
            }

            department.Teachers.Add(teacher);
            _context.SaveChanges();

            return Ok("Teacher added to department successfully.");
        }

        [HttpDelete("RemoveTeacher/{departmentId}/{teacherId}")]
        [Authorize(Roles = "appadmin")]
        public IActionResult RemoveTeacherFromDepartment(int departmentId, int teacherId)
        {
            var department = _context.Departments.Find(departmentId);
            if (department == null)
            {
                return NotFound("Department not found.");
            }

            var teacher = department.Teachers.FirstOrDefault(t => t.Id == teacherId);
            if (teacher == null)
            {
                return NotFound("Teacher not found in this department.");
            }

            department.Teachers.Remove(teacher);
            _context.SaveChanges();

            return Ok("Teacher removed from department successfully.");
        }
    }
}
