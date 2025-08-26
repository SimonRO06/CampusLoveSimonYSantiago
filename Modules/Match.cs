using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusLoveSimonYSantiago.Modules
{
    public class Match
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int Persona1Id { get; set; }
        
        [Required]
        public int Persona2Id { get; set; }
        
        public DateTime FechaMatch { get; set; } = DateTime.Now;
        
        public bool EsMatchMutuo { get; set; }

        [ForeignKey("Persona1Id")]
        public virtual PersonaObject? Persona1 { get; set; }
        
        [ForeignKey("Persona2Id")]
        public virtual PersonaObject? Persona2 { get; set; }
    }
}