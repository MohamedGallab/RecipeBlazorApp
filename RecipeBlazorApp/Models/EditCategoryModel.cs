using System.ComponentModel.DataAnnotations;

namespace RecipeBlazorApp.Models;

public class EditCategoryModel
{
	[Required]
	public string OldCategory { get; set; } = string.Empty;
	[Required]
	public string NewCategory { get; set; } = string.Empty;
}
