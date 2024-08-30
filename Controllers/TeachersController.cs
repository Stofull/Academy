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
    public class TeachersController : ControllerBase
    {
        private readonly AcademyContext _context;

        public TeachersController(AcademyContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public ActionResult<IEnumerable<Teacher>> GetAllTeachers()
        {
            var teachers = _context.Teachers.ToList();
            return Ok(teachers);
        }

        [HttpPost("Add")]
        [Authorize(Roles = "appadmin")]
        public IActionResult AddTeacher([FromBody] Teacher teacher)
        {
            if (teacher == null)
            {
                return BadRequest("Teacher is null.");
            }

            _context.Teachers.Add(teacher);
            _context.SaveChanges();

            return Ok("Teacher added successfully.");
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "appadmin")]
        public IActionResult DeleteTeacher(int id)
        {
            var teacher = _context.Teachers.Find(id);
            if (teacher == null)
            {
                return NotFound("Teacher not found.");
            }

            _context.Teachers.Remove(teacher);
            _context.SaveChanges();

            return Ok("Teacher deleted successfully.");
        }

        [HttpPut("Edit")]
        [Authorize(Roles = "appadmin")]
        public IActionResult EditTeacher([FromBody] Teacher teacher)
        {
            if (teacher == null || teacher.Id == 0)
            {
                return BadRequest("Invalid teacher data.");
            }

            var existingTeacher = _context.Teachers.Find(teacher.Id);
            if (existingTeacher == null)
            {
                return NotFound("Teacher not found.");
            }

            existingTeacher.Name = teacher.Name;
            existingTeacher.Subject = teacher.Subject;

            _context.Teachers.Update(existingTeacher);
            _context.SaveChanges();

            return Ok("Teacher updated successfully.");
        }
    }
}
