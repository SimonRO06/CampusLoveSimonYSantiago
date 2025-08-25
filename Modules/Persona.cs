using CampusLoveSimonYSantiago.Shared;

namespace CampusLoveSimonYSantiago.Modules
{
    public class Persona(AppDbContext context)
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Edad { get; set; }
        public string Genero { get; set; } = string.Empty;
        public string Frase { get; set; } = string.Empty;
        public int CarreraId { get; set; }
        public Carrera? Carrera { get; set; }
        public ICollection<PersonaInteres> PersonaIntereses { get; set; } = new List<PersonaInteres>();
    }
    
    private readonly AppDbContext _context = context;

        public void CrearPersona()
        {
            // Pedir datos al usuario
            Console.Write("Ingrese el nombre: ");
            string nombre = Console.ReadLine() ?? string.Empty;

            Console.Write("Ingrese la edad: ");
            int edad = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Ingrese el género: ");
            string genero = Console.ReadLine() ?? string.Empty;

            Console.Write("Ingrese la carrera: ");
            if (int.TryParse(Console.ReadLine(), out Carrera carrera))
            {
                Console.WriteLine($"Tu numero es {carrera}");
            }
            else
            {
                Console.WriteLine("Numero inválida.");
            }

            Console.Write("Ingrese los intereses: ");
            Interes intereses = Console.ReadLine() ?? string.Empty;

            // Crear objeto Persona
            var persona = new Persona
            {
                Nombre = nombre,
                Edad = edad,
                Genero = genero,
                Carrera = carrera,
                Intereses = intereses
            };

            _context.Personas.Add(persona);
            _context.SaveChanges();

            Console.WriteLine("✅ Persona creada y guardada en la base de datos.");
        }
}