using System;

namespace Proyecto_Inventario
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool salir = false;

            while (!salir) // Bucle principal que mantiene la aplicación en ejecución
            {
                Console.Clear();
                Console.WriteLine("==================");
                Console.WriteLine("   INVENTARIO");
                Console.WriteLine("==================");
                Console.WriteLine("1. INGRESAR PRODUCTOS");
                Console.WriteLine("2. VENTAS DEL PRODUCTO");
                Console.WriteLine("3. STOCK");
                Console.WriteLine("4. SALIR");
                Console.Write("\nSeleccione una opcion: ");

                string opcion = Console.ReadLine();

                try // Bloque try-catch para manejar excepciones inesperadas
                {
                    switch (opcion)
                    {
                        case "1":
                            InventarioMenu.RunInventarioMenu(); // Navega al menú de gestión de inventario
                            break;
                        case "2":
                            Ventas.RunVentas(); // Navega al menú de ventas
                            break;
                        case "3":
                            Reportes.RunReportes(); // Navega al menú de reportes
                            break;
                        case "4":
                            salir = true; // Finaliza la aplicación
                            break;
                        default:
                            Console.WriteLine("Opción no válida");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    // Captura y muestra cualquier error para evitar que la aplicación se cierre inesperadamente
                    Console.WriteLine($"Error en la aplicación: {ex.Message}");
                    Console.WriteLine("Presione cualquier tecla para continuar...");
                    Console.ReadKey();
                }

                if (!salir)
                {
                    Console.WriteLine("\nPresione cualquier tecla para volver al menú...");
                    Console.ReadKey();
                }
            }
        }
    }
}