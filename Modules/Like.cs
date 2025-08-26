using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusLoveSimonYSantiago.Modules
{
    public class Like
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int PersonaQueDaLikeId { get; set; }
        
        [Required]
        public int PersonaQueRecibeLikeId { get; set; }
        
        public DateTime FechaLike { get; set; } = DateTime.Now;

        [ForeignKey("PersonaQueDaLikeId")]
        public virtual PersonaObject? PersonaQueDaLike { get; set; }
        
        [ForeignKey("PersonaQueRecibeLikeId")]
        public virtual PersonaObject? PersonaQueRecibeLike { get; set; }
    }
}