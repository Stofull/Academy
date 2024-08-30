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
    public class GroupsController : ControllerBase
    {
        private readonly AcademyContext _context;

        public GroupsController(AcademyContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public ActionResult<IEnumerable<Group>> GetAllGroups()
        {
            var groups = _context.Groups.ToList();
            return Ok(groups);
        }

        [HttpPost("Add")]
        [Authorize(Roles = "appadmin")]
        public IActionResult AddGroup([FromBody] Group group)
        {
            if (group == null)
            {
                return BadRequest("Group is null.");
            }

            _context.Groups.Add(group);
            _context.SaveChanges();

            return Ok("Group added successfully.");
        }

        [HttpDelete("Delete/{id}")]
        [Authorize(Roles = "appadmin")]
        public IActionResult DeleteGroup(int id)
        {
            var group = _context.Groups.Find(id);
            if (group == null)
            {
                return NotFound("Group not found.");
            }

            _context.Groups.Remove(group);
            _context.SaveChanges();

            return Ok("Group deleted successfully.");
        }

        [HttpPut("Edit")]
        [Authorize(Roles = "appadmin")]
        public IActionResult EditGroup([FromBody] Group group)
        {
            if (group == null || group.Id == 0)
            {
                return BadRequest("Invalid group data.");
            }

            var existingGroup = _context.Groups.Find(group.Id);
            if (existingGroup == null)
            {
                return NotFound("Group not found.");
            }

            existingGroup.Name = group.Name;
            existingGroup.Teacher = group.Teacher;

            _context.Groups.Update(existingGroup);
            _context.SaveChanges();

            return Ok("Group updated successfully.");
        }

        [HttpPost("AddStudent/{groupId}/{studentId}")]
        [Authorize(Roles = "appadmin")]
        public IActionResult AddStudentToGroup(int groupId, int studentId)
        {
            var group = _context.Groups.Find(groupId);
            if (group == null)
            {
                return NotFound("Group not found.");
            }

            var student = _context.Students.Find(studentId);
            if (student == null)
            {
                return NotFound("Student not found.");
            }

            group.Students.Add(student);
            _context.SaveChanges();

            return Ok("Student added to group successfully.");
        }

        [HttpDelete("RemoveStudent/{groupId}/{studentId}")]
        [Authorize(Roles = "appadmin")]
        public IActionResult RemoveStudentFromGroup(int groupId, int studentId)
        {
            var group = _context.Groups.Find(groupId);
            if (group == null)
            {
                return NotFound("Group not found.");
            }

            var student = group.Students.FirstOrDefault(s => s.Id == studentId);
            if (student == null)
            {
                return NotFound("Student not found in this group.");
            }

            group.Students.Remove(student);
            _context.SaveChanges();

            return Ok("Student removed from group successfully.");
        }
    }
}
