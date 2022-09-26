using System.ComponentModel.DataAnnotations;

namespace RecipeBlazorApp.Models;

public class User
{
	[Required]
	public string Name { get; set; } = string.Empty;
	[Required]
	public string Password { get; set; } = string.Empty;
}
