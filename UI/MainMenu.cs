using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampusLoveExamen.UI
{
    public class MainMenu
    {
        string menu = """
        ===========================
        |        BIENVENIDO       |
        ===========================
        | 1. Registrar Usuario    |
        | 2. Eliminar Usuario     |
        | 3. Iniciar Sesion       |
        | 3. Salir                |
        ===========================
        """;

        public void DisplayMenu()
        {
            Console.WriteLine(menu);
            Console.Write("Ingrese una opcion: ");
            string? option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    // Lógica para registrar usuario
                    Console.WriteLine("Registrar Usuario seleccionado.");
                    break;
                case "2":
                    // Lógica para eliminar usuario
                    Console.WriteLine("Eliminar Usuario seleccionado.");
                    break;
                case "3":
                    // Lógica para iniciar sesión
                    Console.WriteLine("Iniciar Sesion seleccionado.");
                    break;
                case "4":
                    Console.WriteLine("Saliendo...");
                    break;
                default:
                    Console.WriteLine("Opcion no valida. Intente de nuevo.");
                    DisplayMenu();
                    break;
            }
        }
    }
}