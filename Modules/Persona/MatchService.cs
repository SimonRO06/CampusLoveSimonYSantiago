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

            var likeExistente = await _context.Likes
                .FirstOrDefaultAsync(l => l.PersonaQueDaLikeId == personaQueDaLikeId && 
                                         l.PersonaQueRecibeLikeId == personaQueRecibeLikeId);

            if (likeExistente != null)
            {
                Console.WriteLine("❌ Ya le has dado like a esta persona.");
                return false;
            }

            var like = new Like
            {
                PersonaQueDaLikeId = personaQueDaLikeId,
                PersonaQueRecibeLikeId = personaQueRecibeLikeId
            };

            _context.Likes.Add(like);
            await _context.SaveChangesAsync();

            Console.WriteLine($"✅ Like dado a la persona ID: {personaQueRecibeLikeId}");

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
                Console.WriteLine($"💕 {persona1!.Nombre} y {persona2!.Nombre} han hecho match!");
                Console.WriteLine("¡Pueden comenzar a chatear!");
            }
        }

        public async Task MostrarPersonasParaLike(int personaActualId)
        {
            var likesDados = await _context.Likes
                .Where(l => l.PersonaQueDaLikeId == personaActualId)
                .Select(l => l.PersonaQueRecibeLikeId)
                .ToListAsync();

            var matches = await _context.Matches
                .Where(m => m.Persona1Id == personaActualId || m.Persona2Id == personaActualId)
                .Select(m => m.Persona1Id == personaActualId ? m.Persona2Id : m.Persona1Id)
                .ToListAsync();

            var idsExcluidos = likesDados.Union(matches).Append(personaActualId).ToList();

            var personas = await _context.Personas
                .Where(p => !idsExcluidos.Contains(p.Id))
                .Include(p => p.Carrera)
                .OrderBy(p => p.Nombre)
                .ToListAsync();

            if (!personas.Any())
            {
                Console.WriteLine("No hay personas disponibles para dar like.");
                return;
            }

            Console.WriteLine("\n👥 PERSONAS DISPONIBLES:");
            Console.WriteLine("==============================================");
            
            foreach (var persona in personas)
            {
                Console.WriteLine($"ID: {persona.Id} | {persona.Nombre} | {persona.Edad} años | {persona.Genero} | {persona.Carrera?.Nombre ?? "Sin carrera"}");
            }
            
            Console.WriteLine("==============================================");
            Console.WriteLine($"Total: {personas.Count} personas disponibles");
        }

        public async Task MostrarPersonasParaLikePaginado(int personaActualId, int pagina = 1, int porPagina = 10)
        {
            var likesDados = await _context.Likes
                .Where(l => l.PersonaQueDaLikeId == personaActualId)
                .Select(l => l.PersonaQueRecibeLikeId)
                .ToListAsync();

            var matches = await _context.Matches
                .Where(m => m.Persona1Id == personaActualId || m.Persona2Id == personaActualId)
                .Select(m => m.Persona1Id == personaActualId ? m.Persona2Id : m.Persona1Id)
                .ToListAsync();

            var idsExcluidos = likesDados.Union(matches).Append(personaActualId).ToList();

            var query = _context.Personas
                .Where(p => !idsExcluidos.Contains(p.Id))
                .Include(p => p.Carrera)
                .OrderBy(p => p.Nombre);

            var totalPersonas = await query.CountAsync();
            var personas = await query
                .Skip((pagina - 1) * porPagina)
                .Take(porPagina)
                .ToListAsync();

            if (!personas.Any())
            {
                Console.WriteLine("No hay personas disponibles para dar like.");
                return;
            }

            Console.WriteLine($"\n👥 PERSONAS DISPONIBLES (Página {pagina}):");
            Console.WriteLine("==============================================");
            
            foreach (var persona in personas)
            {
                Console.WriteLine($"ID: {persona.Id} | {persona.Nombre} | {persona.Edad} años | {persona.Genero} | {persona.Carrera?.Nombre ?? "Sin carrera"}");
            }
            
            Console.WriteLine("==============================================");
            Console.WriteLine($"Mostrando {personas.Count} de {totalPersonas} personas disponibles");
            Console.WriteLine($"Usa 'siguiente' para ver más o 'anterior' para volver");
        }

        public async Task MostrarMatches(int personaId)
        {
            var matches = await _context.Matches
                .Include(m => m.Persona1)
                .Include(m => m.Persona2)
                .Include(m => m.Persona1!.Carrera)
                .Include(m => m.Persona2!.Carrera)
                .Where(m => m.Persona1Id == personaId || m.Persona2Id == personaId)
                .OrderByDescending(m => m.FechaMatch)
                .ToListAsync();

            if (!matches.Any())
            {
                Console.WriteLine("No tienes matches aún. 😢");
                return;
            }

            Console.WriteLine($"\n💖 TUS MATCHES ({matches.Count}):");
            Console.WriteLine("==============================================");
            
            foreach (var match in matches)
            {
                var otraPersona = match.Persona1Id == personaId ? match.Persona2 : match.Persona1;
                Console.WriteLine($"• {otraPersona!.Nombre} ({otraPersona.Edad} años, {otraPersona.Genero})");
                Console.WriteLine($"  Carrera: {otraPersona.Carrera?.Nombre ?? "No especificada"}");
                Console.WriteLine($"  Match desde: {match.FechaMatch:dd/MM/yyyy HH:mm}");
                Console.WriteLine("----------------------------------------------");
            }
        }
    }
}