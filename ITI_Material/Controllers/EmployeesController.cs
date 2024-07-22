using ITI_Material.Data;
using ITI_Material.DTOs;
using ITI_Material.IRepository;
using ITI_Material.Models;
using ITI_Material.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITI_Material.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  //  [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly IServicesDepartements<Employee> _servicesEmployee;
        private readonly ItiMateiral _context;

        public EmployeesController(IServicesDepartements<Employee> servicesEmployee , ItiMateiral context )
        {
            _servicesEmployee = servicesEmployee;
            _context = context;
        }


        [HttpGet]
        public IActionResult GetAllEmployees() 
        {
            return Ok(_servicesEmployee.GetAll()); 
        }

        [HttpGet("GetEmployeeById/{id}")]
        public IActionResult GetEmployee(int id)
        {
            return Ok(_servicesEmployee.FindBy(id));
        }

        [HttpGet("GetEmployeeByName/{Name}")]
        public IActionResult GetEmployee(string Name)
        {
            return Ok(_servicesEmployee.FindBy(Name));
        }

        [HttpPost("Save")]
        public IActionResult SaveNewEmployee(EmployeeViewModel model)
        {
            if(ModelState.IsValid) 
            {
                var emp = new Employee()
                {
                     Name=model.EmployeeName,
                     Salary=model.EmployeeSalary,
                     DepartmentId=model.DepartmentId,
                     
                };
              
                _context.Employees.Add(emp);
                _context.SaveChanges();
                return Ok(emp);  
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut("Update/{id}")]
        public IActionResult UpdateEmployee(EmployeeViewModel model , int id)
        {
            if (ModelState.IsValid )
            {
                var emp =_context.Employees.Find(id);
                if (emp != null)
                {
                    emp.Salary = model.EmployeeSalary;
                    emp.Name = model.EmployeeName;
                    emp.DepartmentId = model.DepartmentId;  
                }
                _context.Employees.Update(emp);
                _context.SaveChanges();
                return Ok(emp);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        public IActionResult DeleteEmployee(int id) 
        {
            var result = _servicesEmployee.Delete(id);
            return Ok(result);
        }

        [HttpGet("EmpWithDepName/{id}")]
        public IActionResult EmpWithDepName(int id) 
        {
            var Emp = _context.Employees.Include(x=>x.Department).FirstOrDefault(x=>x.Id ==  id);   
            if (Emp != null) 
            {
                var EmpData = new EmployeeDataWithDepartmentNameDTO()
                {
                    EmployeeName= Emp.Name,
                    EmployeeSalary= Emp.Salary,
                    DepartmentName=Emp.Department.Name,
                    EmployeeId= Emp.Id,
                };
                return Ok(EmpData); 
            }
            else
            {
                return NotFound(Emp);
            }
        }
    }
}
