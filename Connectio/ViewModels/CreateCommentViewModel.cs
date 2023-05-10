using Connectio.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Connectio.ViewModels
{
    public class CreateCommentViewModel
    {        
        [Required]
        [StringLength(256, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Text { get; set; } = string.Empty;
    }
}
