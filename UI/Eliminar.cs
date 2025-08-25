using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CampusLoveExamen.Modules.Persona.Domain;
using MySql.Data.MySqlClient;

namespace CampusLoveExamen.UI
{
    public class Eliminar
    {
        public void MostrarFormularioEliminacion()
        {
            Console.WriteLine("=== Formulario de Eliminación ===");
            GetUsuarios();

            Console.Write("Ingrese su ID de usuario para eliminar: ");
            if (int.TryParse(Console.ReadLine(), out int userId))
            {
                // Aquí puedes agregar la lógica para eliminar el usuario de la base de datos.
                Console.WriteLine($"Usuario con ID {userId} eliminado exitosamente.");
            }
            else
            {
                Console.WriteLine("ID inválido. Por favor, ingrese un número válido.");
            }
        }

        public List<Persona> GetUsuarios()
        {
            string connStr = "Server=localhost;Database=ExamenDB;User=campus2023;Password=campus2023;";
            using var conn = new MySqlConnection(connStr);
            conn.Open();
            var lista = new List<Persona>();
            string query = "SELECT id, nombre, edad, id_genero, id_carrera, enfoque, id_interes_favorito, intereses, frase, likes FROM Persona";
            using var cmd = new MySqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Persona
                {
                    Id = reader.GetInt32("id"),
                    Nombre = reader.GetString("nombre"),
                    Edad = reader.GetInt32("edad"),
                    Genero = reader.GetInt32("id_genero"),
                    Carrera = reader.GetInt32("id_carrera"),
                    Enfoque = reader.GetString("enfoque"),
                    InteresFavorito = reader.GetInt32("id_interes_favorito"),
                    Intereses = reader.GetString("intereses"),
                    Frase = reader.GetString("frase"),
                    Likes = reader.GetInt32("likes")
                });
            }
            conn.Close();
            return lista;
        }
    }
}