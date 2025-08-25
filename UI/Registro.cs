using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampusLoveExamen.UI
{
    public class Registro
    {
        public void MostrarFormularioRegistro()
        {
            Console.WriteLine("=== Formulario de Registro ===");
            Console.Write("Ingrese su nombre: ");
            string? nombre = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(nombre))
            {
                Console.WriteLine("El nombre no puede estar vacío.");
                return;
            }
            else
            {
                Console.Write("Ingrese su edad: ");
                if (int.TryParse(Console.ReadLine(), out int numero))
                {
                    Console.Write("Genero (1: Masculino, 2: Femenino): ");
                    if (int.TryParse(Console.ReadLine(), out int genero))
                    {
                        Console.Write("Seleccione la categoría de su Carrera : ");
                        if (int.TryParse(Console.ReadLine(), out int carrera))
                        {
                            Console.Write("Enfoque (Académico, Deportivo, Cultural, etc.): ");
                            string? enfoque = Console.ReadLine();
                            if (string.IsNullOrWhiteSpace(enfoque))
                            {
                                Console.WriteLine("El enfoque no puede estar vacío.");
                                return;
                            }
                            else
                            {
                                Console.Write("Seleccione su Interes Favorito: ");
                                if (int.TryParse(Console.ReadLine(), out int interesFavorito))
                                {
                                    Console.Write("Ingrese sus Intereses (separados por comas): ");
                                    string? intereses = Console.ReadLine();
                                    if (string.IsNullOrWhiteSpace(intereses))
                                    {
                                        Console.WriteLine("Los intereses no pueden estar vacíos.");
                                        return;
                                    }
                                    else
                                    {
                                        Console.Write("Ingrese una frase que lo describa: ");
                                        string? frase = Console.ReadLine();
                                        if (string.IsNullOrWhiteSpace(frase))
                                        {
                                            Console.WriteLine("La frase no puede estar vacía.");
                                            return;
                                        }
                                        else
                                        {
                                            // Aca tenemos para guardar el usuario en la base de datos
                                            Console.WriteLine("Registro exitoso. ¡Bienvenido, " + nombre + "!");
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Numero inválida.");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Numero inválida.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Numero inválida.");
                    }
                }
                else
                {
                    Console.WriteLine("Numero inválida.");
                }
            }
        }
    }
}