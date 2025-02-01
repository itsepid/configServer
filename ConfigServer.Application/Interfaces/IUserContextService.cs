namespace ConfigServer.Application.Interfaces
{
    public interface IUserContextService
    {
        Guid GetCurrentUserId();
    }
}