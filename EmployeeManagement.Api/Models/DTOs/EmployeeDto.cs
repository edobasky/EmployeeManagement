namespace EmployeeManagement.Api.Models.DTOs
{
    public class EmployeeDto
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; } = null!;
        public string ContactNo { get; set; } = null!;
        public string? AltContactNo { get; set; }
        public string Email { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string Pincode { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Role { get; set; } = string.Empty;

        public int DesignationId { get; set; }
        public string DesignationName { get; set; } = null!;
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
    }
}
