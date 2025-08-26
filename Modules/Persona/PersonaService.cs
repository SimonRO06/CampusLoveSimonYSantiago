using System;
using System.Linq;
using CampusLoveSimonYSantiago.Shared;

namespace CampusLoveSimonYSantiago.Modules.Persona
{
    public class PersonaService
    {
        private readonly AppDbContext _context;

        public PersonaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CrearPersona()
        {
            try
            {
                Console.Write("Ingrese el nombre: ");
                string nombre = Console.ReadLine()?.Trim() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(nombre))
                {
                    Console.WriteLine("❌ El nombre no puede estar vacío.");
                    return;
                }

                Console.Write("Ingrese la edad: ");
                if (!int.TryParse(Console.ReadLine(), out int edad) || edad < 18 || edad > 100)
                {
                    Console.WriteLine("❌ Edad inválida. Debe ser entre 18 y 100 años.");
                    return;
                }

                Console.Write("Ingrese el género (M/F): ");
                string genero = Console.ReadLine()?.Trim()?.ToUpper() ?? string.Empty;
                if (genero.ToUpper() != "M" && genero != "F")
                {
                    Console.WriteLine("❌ El género debe ser 'M' o 'F'.");
                    return;
                }

                Console.WriteLine("\n--- Carreras disponibles ---");
                var carreras = _context.Carreras.ToList();
                if (!carreras.Any())
                {
                    Console.WriteLine("❌ No hay carreras disponibles. Primero crea carreras.");
                    return;
                }

                foreach (var c in carreras)
                {
                    Console.WriteLine($"ID: {c.Id} - {c.Nombre}");
                }

                Console.Write("Ingrese el ID de la carrera: ");
                if (!int.TryParse(Console.ReadLine(), out int carreraId))
                {
                    Console.WriteLine("❌ ID de carrera inválido.");
                    return;
                }

                var carreraExistente = _context.Carreras.Find(carreraId);
                if (carreraExistente == null)
                {
                    Console.WriteLine("❌ La carrera seleccionada no existe.");
                    return;
                }

                var persona = new PersonaObject
                {
                    Nombre = nombre,
                    Edad = edad,
                    Genero = genero.ToUpper(),
                    CarreraId = carreraId
                };

                await _context.Personas.AddAsync(persona);
                await _context.SaveChangesAsync();
                Console.WriteLine($"✅ Persona '{nombre}' creada exitosamente (ID: {persona.Id})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al crear persona: {ex.Message}");
            }
        }
    }
}