namespace ApiLogin.Interfaces
{
    public interface IJwtMiddleware
    {
        Task Invoke(HttpContext context);
    }
}