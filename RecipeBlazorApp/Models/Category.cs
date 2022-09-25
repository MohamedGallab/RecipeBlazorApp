using System.ComponentModel.DataAnnotations;

namespace RecipeBlazorApp.Models;

public class Category
{
	[Required]
	public string Name { get; set; } = string.Empty;
}
