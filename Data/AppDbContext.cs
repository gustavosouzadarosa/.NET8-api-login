using ApiLogin.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

#pragma warning disable S125 // Sections of code should not be commented out

namespace ApiLogin.Data
{
    // IdentityDbContext == Projetado especificamente para gerenciar informações relacionadas à identidade do usuário,
    // como usuários, roles, claims e logins. Fornece uma série de funcionalidades prontas para uso.
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }

    // DbContext == Permite criar um modelo de dados personalizado, adaptando-o às necessidades específicas da sua aplicação.
    //public class AppDbContext : DbContext
    //{
    //    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    //    {
    //    }

    //    public DbSet<User> Users { get; set; }
    //}
}

#pragma warning restore S2344