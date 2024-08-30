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
    public class FacultiesController : ControllerBase
    {
        private readonly AcademyContext _context;

        public FacultiesController(AcademyContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public ActionResult<IEnumerable<Faculty>> GetAllFaculties()
        {
            var faculties = _context.Faculties.ToList();
            return Ok(faculties);
        }

        [HttpPost("Add")]
        [Authorize(Roles = "appadmin")]
        public IActionResult AddFaculty([FromBody] Faculty faculty)
        {
            if (faculty == null)
            {
                return BadRequest("Faculty is null.");
            }

            _context.Faculties.Add(faculty);
            _context.SaveChanges();

            return Ok("Faculty added successfully.");
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "appadmin")]
        public IActionResult DeleteFaculty(int id)
        {
            var faculty = _context.Faculties.Find(id);
            if (faculty == null)
            {
                return NotFound("Faculty not found.");
            }

            _context.Faculties.Remove(faculty);
            _context.SaveChanges();

            return Ok("Faculty deleted successfully.");
        }

        [HttpPut("Edit")]
        [Authorize(Roles = "appadmin")]
        public IActionResult EditFaculty([FromBody] Faculty faculty)
        {
            if (faculty == null || faculty.Id == 0)
            {
                return BadRequest("Invalid faculty data.");
            }

            var existingFaculty = _context.Faculties.Find(faculty.Id);
            if (existingFaculty == null)
            {
                return NotFound("Faculty not found.");
            }

            existingFaculty.Name = faculty.Name;

            _context.Faculties.Update(existingFaculty);
            _context.SaveChanges();

            return Ok("Faculty updated successfully.");
        }

        [HttpPost("AddGroup/{facultyId}/{groupId}")]
        [Authorize(Roles = "appadmin")]
        public IActionResult AddGroupToFaculty(int facultyId, int groupId)
        {
            var faculty = _context.Faculties.Find(facultyId);
            if (faculty == null)
            {
                return NotFound("Faculty not found.");
            }

            var group = _context.Groups.Find(groupId);
            if (group == null)
            {
                return NotFound("Group not found.");
            }

            faculty.Groups.Add(group);
            _context.SaveChanges();

            return Ok("Group added to faculty successfully.");
        }

        [HttpDelete("RemoveGroup/{facultyId}/{groupId}")]
        [Authorize(Roles = "appadmin")]
        public IActionResult RemoveGroupFromFaculty(int facultyId, int groupId)
        {
            var faculty = _context.Faculties.Find(facultyId);
            if (faculty == null)
            {
                return NotFound("Faculty not found.");
            }

            var group = faculty.Groups.FirstOrDefault(g => g.Id == groupId);
            if (group == null)
            {
                return NotFound("Group not found in this faculty.");
            }

            faculty.Groups.Remove(group);
            _context.SaveChanges();

            return Ok("Group removed from faculty successfully.");
        }
    }
}
