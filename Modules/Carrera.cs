namespace CampusLoveSimonYSantiago.Modules
{
    public class Carrera
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public ICollection<Persona> Personas { get; set; } = new List<Persona>();
    }
}