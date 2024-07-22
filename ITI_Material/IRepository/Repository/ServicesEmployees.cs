using ITI_Material.Data;
using ITI_Material.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace ITI_Material.IRepository.Repository
{
    public class ServicesEmployees : IServicesDepartements<Employee>
    {
        private readonly ItiMateiral _context;
        public ServicesEmployees(ItiMateiral context)
        {
            _context = context;
        }
        public bool Delete(int Id)
        {
            try
            {
                var Emp = FindBy(Id);
                _context.Employees.Remove(Emp);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public Employee FindBy(int? Id)
        {
            try
            {
                return _context.Employees.SingleOrDefault(x => x.Id == Id);
            }
            catch (Exception)
            {

                return null;
            }
        }

        public Employee FindBy(string Name)
        {
            try
            {
                return _context.Employees.SingleOrDefault(x => x.Name == Name.Trim());
            }
            catch (Exception)
            {

                return null;
            }
        }

        public List<Employee> GetAll()
        {
            try
            {
                return _context.Employees.ToList();
            }
            catch (Exception)
            {

                return null;
            }
        }

        public bool Save(Employee model)
        {
            try
            {
                var Emp = FindBy(model.Id);
                if(Emp == null)
                {
                    var newEmp = new Employee()
                    {
                        Name = model.Name,
                        Salary=model.Salary,
                    };
                    _context.Employees.Add(newEmp);
                    _context.SaveChanges();
                }
                else
                {
                    Emp.Name = model.Name;
                    Emp.Salary = model.Salary;
                    _context.Employees.Update(Emp);
                    _context.SaveChanges();
                }
               
                return true;    
            }
            catch (Exception)
            {

                return false;
            } 
        }
    }
}
