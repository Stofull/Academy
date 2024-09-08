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
    public class StudentsController : ControllerBase
    {
        private readonly AcademyContext _context;

        public StudentsController(AcademyContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public ActionResult<IEnumerable<Student>> GetAllStudents()
        {
            var students = _context.Students.ToList();
            return Ok(students);
        }

        [HttpPost("Add")]
        //[Authorize(Roles = "appadmin")]
        public IActionResult AddStudent([FromBody] Student student)
        {
            if (student == null)
            {
                return BadRequest("Student is null.");
            }

            _context.Students.Add(student);
            _context.SaveChanges();

            return Ok("Student added successfully.");
        }

        [HttpDelete("Delete/{id}")]
        //[Authorize(Roles = "appadmin")]
        public IActionResult DeleteStudent(int id)
        {
            var student = _context.Students.Find(id);
            if (student == null)
            {
                return NotFound("Student not found.");
            }

            _context.Students.Remove(student);
            _context.SaveChanges();

            return Ok("Student deleted successfully.");
        }

        [HttpPut("Edit")]
        //[Authorize(Roles = "appadmin")]
        public IActionResult EditStudent([FromBody] Student student)
        {
            if (student == null || student.Id == 0)
            {
                return BadRequest("Invalid student data.");
            }

            var existingStudent = _context.Students.Find(student.Id);
            if (existingStudent == null)
            {
                return NotFound("Student not found.");
            }

            existingStudent.Name = student.Name;
            existingStudent.Age = student.Age;

            _context.Students.Update(existingStudent);
            _context.SaveChanges();

            return Ok("Student updated successfully.");
        }
    }
}
