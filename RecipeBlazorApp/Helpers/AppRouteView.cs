using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components;
using System.Net;
using Microsoft.JSInterop;
using RecipeBlazorApp.Services;

namespace RecipeBlazorApp.Helpers;

public class AppRouteView : RouteView
{
	[Inject]
	public NavigationManager NavigationManager { get; set; }

	[Inject]
	public IAuthenticationService AuthenticationService { get; set; }

	protected override void Render(RenderTreeBuilder builder)
	{
		var anonymous = Attribute.GetCustomAttribute(RouteData.PageType, typeof(AllowAnonymousAttribute)) != null;
		if (AuthenticationService.JWT.Token == null && !anonymous)
		{
			NavigationManager.NavigateTo($"login");
		}
		else
		{
			base.Render(builder);
		}
	}
}