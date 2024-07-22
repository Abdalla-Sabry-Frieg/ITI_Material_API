using ITI_Material.Data;
using ITI_Material.DTOs;
using ITI_Material.IRepository;
using ITI_Material.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ITI_Material.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class DepartmentsController : ControllerBase
    {
        private readonly IServicesDepartements<Department> _servicesDepartements;
        private readonly ItiMateiral _context;

        public DepartmentsController(IServicesDepartements<Department> servicesDepartements , ItiMateiral context)
        {
            _servicesDepartements = servicesDepartements;
            _context= context;
        }


        [HttpGet]
        public IActionResult GetDepatments()
        {
            var result = _context.Departments.ToList();
            return Ok(result);
        }
        [HttpGet("GetById/{Id}")]  
        public IActionResult GetDepatmentById(int Id)
        {
            var result = _servicesDepartements.FindBy(Id);
            return Ok(result);
        }
        [HttpGet("GetByName/{name}")]
        public IActionResult GetDepatmentByName(string name)
        {
            var result = _servicesDepartements.FindBy(name);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Save(Department departments)
        {

            if (ModelState.IsValid)
            {
                   

                return Ok(_servicesDepartements.Save(departments));
            }
            else
            {
                return NotFound("Can't Save");
            }

            
      
        }

        [HttpPut]
        public IActionResult Update(Department department) 
        {
            //var oldDep = _servicesDepartements.FindBy(department.Id);
            if (ModelState.IsValid) 
            {
                var result = _servicesDepartements.Save(department);
                return Ok(result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id) 
        {
            return Ok(_servicesDepartements.Delete(id));
        }

        [HttpGet("DepDataWithEmpName/{id}")]
        public IActionResult DepartmentDataWithEmpName(int id)
        {
            var Dep = _context.Departments.Include(x=>x.Employees).FirstOrDefault(x=>x.Id == id);
            if (Dep != null)
            {
                var DepData = new DepartmentDetailsWithEmployeeName()
                {
                    DepartmentId = id,
                    DepartmentName = Dep.Name,

                };
                foreach (var item in Dep.Employees)
                {
                    DepData.EmployeesName.Add(item.Name);
                    DepData.EmployeesSalary.Add(item.Salary.ToString());
                }
               
                return Ok(DepData);
            }
            else
            {
                return NotFound(Dep);
            }
        }
    }
}
