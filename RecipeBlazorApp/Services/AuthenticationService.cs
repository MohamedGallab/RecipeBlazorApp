using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using RecipeBlazorApp.Models;
using System.ComponentModel.Design;
using static System.Net.WebRequestMethods;
using System.Net.Http.Headers;
using System;
using System.Net.Http.Json;

namespace RecipeBlazorApp.Services;

public interface IAuthenticationService
{
	JWToken JWT { get; }
	Task Initialize();
	Task Login(User user);
	Task Register(User user);
	Task Logout();
}

public class AuthenticationService : IAuthenticationService
{
	private readonly NavigationManager _navigationManager;
	private readonly IJSRuntime _jsRuntime;
	private readonly HttpClient _httpClient;

	public JWToken JWT { get; private set; } = new JWToken();

	public AuthenticationService(NavigationManager navigationManager, IJSRuntime jsRuntime, HttpClient httpClient)
	{
		_navigationManager = navigationManager;
		_jsRuntime = jsRuntime;
		_httpClient = httpClient;
	}

	public async Task Initialize()
	{
		JWT = await GetToken();
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWT.Token);
	}

	public async Task Login(User user)
	{
		var registerResponse = await _httpClient.PostAsJsonAsync<User>("/login", user, CancellationToken.None);
		if (registerResponse.IsSuccessStatusCode)
		{
			JWT = await registerResponse.Content.ReadFromJsonAsync<JWToken>();
			await SetToken(JWT);
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWT.Token);
		}
	}

	public async Task Register(User user)
	{
		var registerResponse = await _httpClient.PostAsJsonAsync<User>("/register", user, CancellationToken.None);
		if (registerResponse.IsSuccessStatusCode)
		{
			JWT = await registerResponse.Content.ReadFromJsonAsync<JWToken>();
			await SetToken(JWT);
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", JWT.Token);
		}
	}

	public async Task Logout()
	{
		JWT.Token = null;
		JWT.RefreshToken = null;
		await RemoveToken();
		_navigationManager.NavigateTo("login");
	}

	public async Task SetToken(JWToken jWToken)
	{
		await _jsRuntime.InvokeAsync<string>("localStorage.setItem", "token", jWToken.Token);
		await _jsRuntime.InvokeAsync<string>("localStorage.setItem", "refreshToken", jWToken.RefreshToken);
	}

	public async Task<JWToken> GetToken()
	{
		JWToken jwt = new()
		{
			Token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "token"),
			RefreshToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "refreshToken")
		};
		return jwt;
	}

	public async Task RemoveToken()
	{
		await _jsRuntime.InvokeAsync<string>("localStorage.removeItem", "token");
		await _jsRuntime.InvokeAsync<string>("localStorage.removeItem", "refreshToken");
	}
}
