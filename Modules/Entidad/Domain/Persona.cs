using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CampusLoveExamen.Modules.Persona.Domain
{
    [Table("Persona")]
    public class Persona
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public int Edad { get; set; }
        public int Genero { get; set; }
        public int Carrera { get; set; }
        public string? Enfoque { get; set; }
        public int InteresFavorito { get; set; }
        public string? Intereses { get; set; }
        public string? Frase { get; set; }
        public int Likes { get; set; } = 0;
    }

    
}