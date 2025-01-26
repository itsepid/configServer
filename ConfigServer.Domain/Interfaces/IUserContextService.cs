namespace ConfigServer.Domain.Interfaces
{
    public interface IUserContextService
    {
        Guid GetCurrentUserId();
    }
}