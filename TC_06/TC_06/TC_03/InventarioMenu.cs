using System;
using static Proyecto_Inventario.Produtos;

namespace Proyecto_Inventario
{
    // Clase que maneja la interfaz de usuario del inventario
    // Proporciona un menú interactivo para gestionar productos
    class InventarioMenu
    {
        // Método principal que ejecuta el menú de inventario
        // Muestra las opciones y dirige el flujo según la selección del usuario
        public static void RunInventarioMenu()
        {
            Console.Clear();
            bool salir = false;

            // Bucle principal que mantiene el menú activo hasta que el usuario decida salir
            while (!salir)
            {
                // Sección de visualización del menú
                // Muestra todas las opciones disponibles para el usuario
                Console.WriteLine("----------------------");
                Console.WriteLine("          MENÚ        ");
                Console.WriteLine("----------------------");
                Console.WriteLine("1. AGREGAR PRODUCTO");
                Console.WriteLine("2. MOSTRAR PRODUCTO");
                Console.WriteLine("3. BUSCAR PRODUCTO");
                Console.WriteLine("4. MODIFICAR PRODUCTO");
                Console.WriteLine("5. ELIMINAR PRODUCTO");
                Console.WriteLine("6. ORDENAR PRODUCTOR POR NOMBRE");
                Console.WriteLine("7. VOLVER AL MENÚ PRINCIPAL");
                Console.WriteLine("**********************");
                Console.Write("Seleccione una opción: ");

                // Captura la opción elegida por el usuario
                string opcion = Console.ReadLine();

                // Estructura de control para manejar la selección del usuario
                // Cada case ejecuta una función específica del sistema de inventario
                switch (opcion)
                {
                    case "1":
                        Inventario.AgregarProducto();
                        break;
                    case "2":
                        Inventario.MostrarProductos();
                        break;
                    case "3":
                        Inventario.BuscarProducto();
                        break;
                    case "4":
                        Inventario.ModificarProducto();
                        break;
                    case "5":
                        Inventario.EliminarProducto();
                        break;
                    case "6":
                        Inventario.OrdenarProductosPorNombre();
                        Inventario.MostrarProductos();
                        break;
                    case "7":
                        salir = true;
                        break;
                    default:
                        Console.WriteLine("Opción no válida. Intente nuevamente.");
                        break;
                }

                if (!salir)
                {
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
    }

}