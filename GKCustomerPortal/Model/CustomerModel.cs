using System.ComponentModel.DataAnnotations;

namespace GKCustomerPortal.Model;

public class CustomerModel
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "First Name is required.")]
    [StringLength(50)]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required.")]
    [StringLength(50)]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email Address.")]
    public string? Email { get; set; }

    [Range(0, 120, ErrorMessage = "Age must be between 0 and 120.")]
    public int Age { get; set; }
}