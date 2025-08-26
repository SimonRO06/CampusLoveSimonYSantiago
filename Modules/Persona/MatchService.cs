using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CampusLoveSimonYSantiago.Shared;
using Microsoft.EntityFrameworkCore;

namespace CampusLoveSimonYSantiago.Modules.Persona
{
    public class MatchService
    {
        private readonly AppDbContext _context;

        public MatchService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DarLike(int personaQueDaLikeId, int personaQueRecibeLikeId)
        {
            if (personaQueDaLikeId == personaQueRecibeLikeId)
            {
                Console.WriteLine("❌ No puedes darte like a ti mismo.");
                return false;
            }

            // Verificar si ya existe el like
            var likeExistente = await _context.Likes
                .FirstOrDefaultAsync(l => l.PersonaQueDaLikeId == personaQueDaLikeId && 
                                         l.PersonaQueRecibeLikeId == personaQueRecibeLikeId);

            if (likeExistente != null)
            {
                Console.WriteLine("❌ Ya le has dado like a esta persona.");
                return false;
            }

            // Crear nuevo like
            var like = new Like
            {
                PersonaQueDaLikeId = personaQueDaLikeId,
                PersonaQueRecibeLikeId = personaQueRecibeLikeId
            };

            _context.Likes.Add(like);
            await _context.SaveChangesAsync();

            Console.WriteLine($"✅ Like dado a la persona ID: {personaQueRecibeLikeId}");

            // Verificar si es match mutuo
            var likeInverso = await _context.Likes
                .FirstOrDefaultAsync(l => l.PersonaQueDaLikeId == personaQueRecibeLikeId && 
                                         l.PersonaQueRecibeLikeId == personaQueDaLikeId);

            if (likeInverso != null)
            {
                await CrearMatch(personaQueDaLikeId, personaQueRecibeLikeId);
                return true;
            }

            return true;
        }

        private async Task CrearMatch(int persona1Id, int persona2Id)
        {
            // Ordenar IDs para evitar duplicados
            var idMenor = Math.Min(persona1Id, persona2Id);
            var idMayor = Math.Max(persona1Id, persona2Id);

            var matchExistente = await _context.Matches
                .FirstOrDefaultAsync(m => m.Persona1Id == idMenor && m.Persona2Id == idMayor);

            if (matchExistente == null)
            {
                var match = new Match
                {
                    Persona1Id = idMenor,
                    Persona2Id = idMayor,
                    EsMatchMutuo = true
                };

                _context.Matches.Add(match);
                await _context.SaveChangesAsync();

                var persona1 = await _context.Personas.FindAsync(persona1Id);
                var persona2 = await _context.Personas.FindAsync(persona2Id);

                Console.WriteLine("\n🎉 ¡MATCH!");
                Console.WriteLine($"💕 {persona1.Nombre} y {persona2.Nombre} han hecho match!");
                Console.WriteLine("¡Pueden comenzar a chatear!");
            }
        }

        public async Task MostrarMatches(int personaId)
        {
            var matches = await _context.Matches
                .Include(m => m.Persona1)
                .Include(m => m.Persona2)
                .Where(m => m.Persona1Id == personaId || m.Persona2Id == personaId)
                .ToListAsync();

            if (!matches.Any())
            {
                Console.WriteLine("No tienes matches aún. 😢");
                return;
            }

            Console.WriteLine("\n💖 TUS MATCHES:");
            foreach (var match in matches)
            {
                var otraPersona = match.Persona1Id == personaId ? match.Persona2 : match.Persona1;
                Console.WriteLine($"• {otraPersona.Nombre} (ID: {otraPersona.Id}) - {match.FechaMatch:dd/MM/yyyy}");
            }
        }

        public async Task MostrarPersonasParaLike(int personaActualId)
        {
            var personas = await _context.Personas
                .Where(p => p.Id != personaActualId)
                .Include(p => p.Carrera)
                .ToListAsync();

            if (!personas.Any())
            {
                Console.WriteLine("No hay personas disponibles para dar like.");
                return;
            }

            Console.WriteLine("\n👥 PERSONAS DISPONIBLES:");
            foreach (var persona in personas)
            {
                Console.WriteLine($"ID: {persona.Id} | {persona.Nombre} | {persona.Edad} años | {persona.Genero} | {persona.Carrera?.Nombre}");
            }
        }
    }
}