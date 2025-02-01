namespace ConfigServer.Application.Interfaces
{
    using ConfigServer.Domain.Entities;

    public interface IAuthService
    {
        Task<(string Token, int UserId)> LoginAsync(string username, string password);
        Task<string> SignupAsync(string username, string password, string role);
    }
}
