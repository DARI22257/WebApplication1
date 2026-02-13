using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AvaloniaApplication2.Models;

namespace AvaloniaApplication2.Services;

public class ApiService
{
     private readonly HttpClient _http;
    private readonly AuthService _auth;

    public event Action? OnUnauthorized;

    public ApiService(AuthService auth)
    {
        _auth = auth;
        _http = new HttpClient
        {
            BaseAddress = new Uri("Нужен Apiiiiii") 
        };
    }

    private async Task<HttpResponseMessage> SendAsync(Func<Task<HttpResponseMessage>> action)
    {
        var token = await _auth.GetTokenAsync();
        _http.DefaultRequestHeaders.Authorization = null;
        if (!string.IsNullOrWhiteSpace(token))
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var resp = await action();

        if (resp.StatusCode == HttpStatusCode.Unauthorized)
            OnUnauthorized?.Invoke();

        return resp;
    }

  
    public async Task<TokenResponse?> LoginAsync(LoginRequest req)
    {
        var resp = await SendAsync(() => _http.PostAsJsonAsync("api/auth/login", req));
        if (!resp.IsSuccessStatusCode) return null;
        return await resp.Content.ReadFromJsonAsync<TokenResponse>();
    }

    public async Task<EmployeeWithRoleDto?> GetProfileAsync()
    {
        var resp = await SendAsync(() => _http.GetAsync("api/auth/profile"));
        if (!resp.IsSuccessStatusCode) return null;
        return await resp.Content.ReadFromJsonAsync<EmployeeWithRoleDto>();
    }

    public async Task<List<EmployeeDto>> GetEmployeesAsync()
    {
        var resp = await SendAsync(() => _http.GetAsync("api/employees"));
        if (!resp.IsSuccessStatusCode) return new List<EmployeeDto>();
        return (await resp.Content.ReadFromJsonAsync<List<EmployeeDto>>()) ?? new();
    }

    public async Task<EmployeeDto?> CreateEmployeeAsync(EmployeeDto dto)
    {
        var resp = await SendAsync(() => _http.PostAsJsonAsync("api/employees", dto));
        if (!resp.IsSuccessStatusCode) return null;
        return await resp.Content.ReadFromJsonAsync<EmployeeDto>();
    }

    public async Task<bool> UpdateEmployeeAsync(int id, EmployeeDto dto)
    {
        var resp = await SendAsync(() => _http.PutAsJsonAsync($"api/employees/{id}", dto));
        return resp.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteEmployeeAsync(int id)
    {
        var resp = await SendAsync(() => _http.DeleteAsync($"api/employees/{id}"));
        return resp.IsSuccessStatusCode;
    }

    public async Task<List<ShiftDto>> GetShiftsAsync()
    {
        var resp = await SendAsync(() => _http.GetAsync("api/shifts"));
        if (!resp.IsSuccessStatusCode) return new List<ShiftDto>();
        return (await resp.Content.ReadFromJsonAsync<List<ShiftDto>>()) ?? new();
    }

    public async Task<List<ShiftDto>> GetShiftsByEmployeeAsync(int employeeId)
    {
        var resp = await SendAsync(() => _http.GetAsync($"api/shifts/employee/{employeeId}"));
        if (!resp.IsSuccessStatusCode) return new List<ShiftDto>();
        return (await resp.Content.ReadFromJsonAsync<List<ShiftDto>>()) ?? new();
    }

    public async Task<ShiftDto?> CreateShiftAsync(ShiftDto dto)
    {
        var resp = await SendAsync(() => _http.PostAsJsonAsync("api/shifts", dto));
        if (!resp.IsSuccessStatusCode) return null;
        return await resp.Content.ReadFromJsonAsync<ShiftDto>();
    }

    public async Task<bool> UpdateShiftAsync(int id, ShiftDto dto)
    {
        var resp = await SendAsync(() => _http.PutAsJsonAsync($"api/shifts/{id}", dto));
        return resp.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteShiftAsync(int id)
    {
        var resp = await SendAsync(() => _http.DeleteAsync($"api/shifts/{id}"));
        return resp.IsSuccessStatusCode;
    }
}