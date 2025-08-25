using ApuntesCS.Modules.Entidad.Infrastructure.Repositories.PersonaRepository;
using CampusLoveExamen.Shared.Helpers;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static async Task Main(string[] args)
    {
        using (var db = DbContextFactory.Create())
        {
            db.Database.Migrate();
        }
    }
}