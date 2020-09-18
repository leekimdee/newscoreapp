using System.ComponentModel.DataAnnotations;

namespace NewsCoreApp.Models.AccountViewModels
{
    public class ExternalLoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}