using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Api.Models
{
    [Table("departmentTbl")]
    public class Department
    {
        [Key]
        public int departmentId { get; set; }
        [Required,MaxLength(50)]
        public string departmentName { get; set; } = string.Empty;
        public bool isActive { get; set; }
    }
}
