using PMS.Domain.Enumeration;
using System.ComponentModel.DataAnnotations;

namespace PMS.Application.DTOs.Policies
{
    public class PolicyDto : BaseDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        public string? Description { get; set; }
        public PolicyType PolicyType { get; set; }
        public decimal Price { get; set; }
    }
}
