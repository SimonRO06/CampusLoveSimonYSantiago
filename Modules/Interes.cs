namespace CampusLoveSimonYSantiago.Modules
{
    public class Interes
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public ICollection<PersonaInteres> PersonaIntereses { get; set; } = new List<PersonaInteres>();
    }
}