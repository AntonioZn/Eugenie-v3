namespace Eugenie.Server.Api.Models.Account
{
    using System.ComponentModel.DataAnnotations;

    using Common.Constants;

    public class RegisterBindingModel
    {
        [Required]
        [Display(Name = "Username")]
        [StringLength(ValidationConstants.UsernameMaxLength, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = ValidationConstants.UsernameMinLength)]
        public string Username { get; set; }

        [Required]
        [Display(Name = "FirstName")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required]
        [StringLength(ValidationConstants.PasswordMaxLength, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = ValidationConstants.PasswordMinLength)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }
    }
}