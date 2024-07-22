using ITI_Material.Data;
using ITI_Material.Models;

namespace ITI_Material.IRepository.Repository
{
    public class ServicesDepartements : IServicesDepartements<Department>
    {
        private readonly ItiMateiral _context;
        public ServicesDepartements(ItiMateiral context)
        {
            _context = context;
        }
        public bool Delete(int Id)
        {

            try
            {
                var result = FindBy(Id);
                _context.Remove(result);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public Department FindBy(int? Id)
        {
            try
            {
                return _context.Departments.FirstOrDefault(x => x.Id == Id);
            }
            catch (Exception)
            {

                return null;
            }
        }

        public Department FindBy(string Name)
        {
            try
            {
                return _context.Departments.FirstOrDefault(x => x.Name == Name.Trim());
            }
            catch (Exception)
            {

                return null;
            }
        }

        public List<Department> GetAll()
        {
            try
            {
                return _context.Departments.ToList();
            }
            catch (Exception)
            {

                return null;
            }
        }

        public bool Save(Department model)
        {
            try
            {
                var result = FindBy(model.Id);
                if(result == null) 
                {
                    var newDep = new Department()
                    {
                        Manger = model.Manger,
                        Name=model.Name,
                    };
                    _context.Departments.Add(newDep);
                    _context.SaveChanges();
                }
                else
                {
                    result.Name = model.Name;
                    result.Manger = model.Manger;
                    _context.Departments.Update(result);
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
