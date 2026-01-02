using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Api.Models
{
    [Table("employeeTbl")]
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required,MaxLength(50)]
        public string Name { get; set; } = null!;
        [Required,MaxLength (11)]
        public string ContactNo { get; set; } = null!;
        [Required,RegularExpression("")]
        public string Email { get; set; } = null!;

        public string City { get; set; } = null!;

        public string State { get; set; } = null!;

        public string Pincode { get; set; } = null!;

        public string? AltContactNo { get; set; }

        public string Address { get; set; } = null!;

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
        public int designationId { get; set; }
    }
}
