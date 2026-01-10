namespace EmployeeManagement.Api.Models.DTOs
{
    public class DesignationDto
    {
        public int DesignationId { get; set; }
        public string DesignationName { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
    }
}
