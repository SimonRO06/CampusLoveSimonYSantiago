using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public void CrearPersona()
        {
            Console.Write("Ingrese el nombre: ");
            string nombre = Console.ReadLine() ?? string.Empty;

            Console.Write("Ingrese la edad: ");
            int edad = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Ingrese el género: ");
            string genero = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("\n--- Carreras disponibles ---");
            var carreras = _context.Carreras.ToList();
            foreach (var c in carreras)
            {
                Console.WriteLine($"ID: {c.Id} - {c.Nombre}");
            }

            Console.Write("Ingrese el ID de la carrera: ");
            int carreraId = int.Parse(Console.ReadLine() ?? "0");

            var persona = new PersonaObject
            {
                Nombre = nombre,
                Edad = edad,
                Genero = genero,
                CarreraId = carreraId
            };

            _context.Personas.Add(persona);
            _context.SaveChanges();

            Console.WriteLine("✅ Persona creada y guardada en la base de datos.");
        }
    }
}