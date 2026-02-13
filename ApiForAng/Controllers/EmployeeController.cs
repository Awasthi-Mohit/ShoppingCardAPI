using ApiForAng.ApplicationDbcontext;
using ApiForAng.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiForAng.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;

        }
        [HttpGet("Employee")]
        public async Task<IActionResult> GetEmployees()
        {
            var employees = _context.Employee.ToList();
                return Ok(employees);
        }
        [HttpPost("createEmployee")]
        public async Task<IActionResult> Create(Employee employee)
        {
            _context.Employee.Add(employee);
            _context.SaveChanges();
            return Ok(employee);

        }
        [HttpPut("UpdateEmployee")]
        public async Task<IActionResult> Update(Employee employee)
        {
            var existingEmployee = _context.Employee.FirstOrDefault(e => e.id == employee.id);
            if (existingEmployee == null)
            {
                return NotFound("Employee not found.");
            }
            existingEmployee.Name = employee.Name;
            existingEmployee.Salary = employee.Salary;
            _context.SaveChanges();
            return Ok(existingEmployee);
        }
        [HttpDelete("DeleteEmployee/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid employee ID.");
            }
            var employee = _context.Employee.FirstOrDefault(e => e.id == id);
            if (employee == null)
            {
                return NotFound("Employee not found.");
            }
            _context.Employee.Remove(employee);
            _context.SaveChanges();
            return Ok("Employee deleted successfully.");
        }
    }
}
