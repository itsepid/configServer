using System;
using System.Threading.Tasks;
using ConfigServer.Domain.Entities;
using ConfigServer.Application.Interfaces;
using ConfigServer.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public AuthService(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<string> SignupAsync(string username, string password, string role)
    {
       
        var existingUser = await _userRepository.GetByUsernameAsync(username);
        if (existingUser != null)
            throw new Exception("User already exists.");

        var user = new User(username, password, role);
        await _userRepository.AddAsync(user);

        return _jwtService.GenerateJwt(user);
    }

    public async Task<(string Token, int UserId)> LoginAsync(string username, string password)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null || user.Password != password) 
            throw new Exception("Invalid credentials.");

        
        var token = _jwtService.GenerateJwt(user);

        return (token, user.Id);
}
}