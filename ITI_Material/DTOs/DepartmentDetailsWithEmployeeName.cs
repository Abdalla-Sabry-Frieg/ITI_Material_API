namespace ITI_Material.DTOs
{
    public class DepartmentDetailsWithEmployeeName
    {
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public List<string>? EmployeesName { get; set; } = new List<string>();
        public List<string>? EmployeesSalary { get; set; } = new List<string>();
    }
}
