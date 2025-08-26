using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CampusLoveSimonYSantiago.Shared;
using CampusLoveSimonYSantiago.Modules.Persona;
using CampusLoveSimonYSantiago.Modules;
using System;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Cargar configuración
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            // Configurar DbContext
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseMySql(
                configuration.GetConnectionString("DefaultConnection"),
                new MySqlServerVersion(new Version(8, 0, 21))
            );

            using var context = new AppDbContext(optionsBuilder.Options);
            await context.Database.EnsureCreatedAsync();
            
            Console.WriteLine("✅ Conexión con la base de datos establecida correctamente");
            
            var personaService = new PersonaService(context);
            var matchService = new MatchService(context);
            
            await ShowMainMenu(personaService, matchService);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error al iniciar la aplicación: {ex.Message}");
            Console.WriteLine("Presione cualquier tecla para salir...");
            Console.ReadKey();
        }
    }

    static async Task ShowMainMenu(PersonaService personaService, MatchService matchService)
    {
        bool salir = false;
        
        while (!salir)
        {
            Console.Clear();
            Console.WriteLine("🎓 === CAMPUS LOVE - SISTEMA DE CITAS ===");
            Console.WriteLine("1. 👤 Crear nueva persona");
            Console.WriteLine("2. 💖 Dar like a persona");
            Console.WriteLine("3. 💕 Ver mis matches");
            Console.WriteLine("4. 👥 Ver personas disponibles");
            Console.WriteLine("5. 📋 Listar todas las personas");
            Console.WriteLine("6. 🚪 Salir");
            Console.WriteLine("===========================================");
            Console.Write("Seleccione una opción (1-6): ");
            
            var opcion = Console.ReadLine();
            
            switch (opcion)
            {
                case "1":
                    await ExecuteWithErrorHandling(async () => {
                        Console.Clear();
                        Console.WriteLine("👤 CREAR NUEVA PERSONA");
                        Console.WriteLine("======================");
                        personaService.CrearPersona();
                    });
                    break;
                    
                case "2":
                    await ExecuteWithErrorHandling(async () => {
                        await DarLikeMenu(matchService);
                    });
                    break;
                    
                case "3":
                    await ExecuteWithErrorHandling(async () => {
                        await VerMatchesMenu(matchService);
                    });
                    break;
                    
                case "4":
                    await ExecuteWithErrorHandling(async () => {
                        await VerPersonasDisponiblesMenu(matchService);
                    });
                    break;
                    
                case "5":
                    await ExecuteWithErrorHandling(async () => {
                        await ListarTodasLasPersonas(personaService);
                    });
                    break;
                    
                case "6":
                    salir = true;
                    Console.WriteLine("¡Hasta pronto! 👋");
                    break;
                    
                default:
                    Console.WriteLine("❌ Opción inválida. Por favor seleccione 1-6.");
                    break;
            }
            
            if (!salir && opcion != "6")
            {
                Console.WriteLine("\nPresione cualquier tecla para continuar...");
                Console.ReadKey();
            }
        }
    }

    static async Task DarLikeMenu(MatchService matchService)
    {
        Console.Clear();
        Console.WriteLine("💖 DAR LIKE A PERSONA");
        Console.WriteLine("=====================");
        
        Console.Write("Ingrese su ID de persona: ");
        if (!int.TryParse(Console.ReadLine(), out int personaQueDaLikeId))
        {
            Console.WriteLine("❌ ID inválido.");
            return;
        }
        await matchService.MostrarPersonasParaLike(personaQueDaLikeId);
        
        Console.Write("\nIngrese el ID de la persona a la que quiere dar like: ");
        if (!int.TryParse(Console.ReadLine(), out int personaQueRecibeLikeId))
        {
            Console.WriteLine("❌ ID inválido.");
            return;
        }

        await matchService.DarLike(personaQueDaLikeId, personaQueRecibeLikeId);
    }

    static async Task VerMatchesMenu(MatchService matchService)
    {
        Console.Clear();
        Console.WriteLine("💕 MIS MATCHES");
        Console.WriteLine("==============");
        
        Console.Write("Ingrese su ID de persona: ");
        if (!int.TryParse(Console.ReadLine(), out int personaId))
        {
            Console.WriteLine("❌ ID inválido.");
            return;
        }

        await matchService.MostrarMatches(personaId);
    }

    static async Task VerPersonasDisponiblesMenu(MatchService matchService)
    {
        Console.Clear();
        Console.WriteLine("👥 PERSONAS DISPONIBLES");
        Console.WriteLine("=======================");
        
        Console.Write("Ingrese su ID de persona: ");
        if (!int.TryParse(Console.ReadLine(), out int personaId))
        {
            Console.WriteLine("❌ ID inválido.");
            return;
        }

        await matchService.MostrarPersonasParaLike(personaId);
    }

    static Task ListarTodasLasPersonas(PersonaService personaService)
    {
        Console.Clear();
        Console.WriteLine("📋 LISTA DE TODAS LAS PERSONAS");
        Console.WriteLine("==============================");
        
        Console.WriteLine("⚠️  Función en desarrollo...");
        Console.WriteLine("Por ahora use la opción 4 para ver personas disponibles.");
        return Task.CompletedTask;
    }

    static async Task ExecuteWithErrorHandling(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
        }
    }
}