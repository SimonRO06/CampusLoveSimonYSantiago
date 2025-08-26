using CampusLoveSimonYSantiago.Shared;

namespace CampusLoveSimonYSantiago.Modules
{
    public class PersonaObject
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
}