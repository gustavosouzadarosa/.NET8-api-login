namespace ApiLogin.Interfaces
{
    public interface IDataSeeder
    {
        Task SeedRolesAsync(IServiceProvider serviceProvider);
    }
}