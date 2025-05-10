using System;
using System.Collections.Generic;
using System.Linq;

namespace Proyecto_Inventario
{
    internal class Reportes
    {
        public static void RunReportes()
        {
            Console.Clear();
            bool salir = false;

            while (!salir)
            {
                Console.WriteLine("----------------------");
                Console.WriteLine("    MENÚ DE STOCK     ");
                Console.WriteLine("----------------------");
                Console.WriteLine("1. PRODUCTOS EN STOCK");
                Console.WriteLine("2. PRODUCTOS CON BAJO STOCK");
                Console.WriteLine("3. PRODUCTOS POR VENCER");
                Console.WriteLine("4. VOLVER AL MENÚ PRINCIPAL");
                Console.WriteLine("**********************");
                Console.Write("Seleccione una opción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        MostrarProductosEnStock();
                        break;
                    case "2":
                        MostrarProductosBajoStock();
                        break;
                    case "3":
                        MostrarProductosPorVencer();
                        break;
                    case "4":
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

        private static void MostrarProductosEnStock()
        {
            Console.Clear();

            if (Produtos.Inventario.productos.Count == 0)
            {
                Console.WriteLine("No hay productos registrados en el inventario.");
                return;
            }

            // Filtrado LINQ: selecciona solo productos con cantidad > 0
            // y los ordena por cantidad (descendente)
            var productosEnStock = Produtos.Inventario.productos
                .Where(p => p.Cantidad > 0)
                .OrderByDescending(p => p.Cantidad)
                .ToList();

            if (productosEnStock.Count == 0)
            {
                Console.WriteLine("No hay productos con stock disponible.");
                return;
            }

            Console.WriteLine("PRODUCTOS EN STOCK");
            Console.WriteLine("=================================================================================");
            Console.WriteLine("ID\tNombre\t\tMarca\t\tCantidad\tPrecio");
            Console.WriteLine("=================================================================================");

            foreach (var p in productosEnStock)
            {
                Console.WriteLine($"{p.ID}\t{p.Nombre}\t\t{p.Marca}\t\t{p.Cantidad}\t\t{p.Precio}");
            }
            // Cálculos agregados usando LINQ
            Console.WriteLine($"\nTotal de productos en stock: {productosEnStock.Sum(p => p.Cantidad)}");
            Console.WriteLine($"Valor total del inventario: {productosEnStock.Sum(p => p.Cantidad * p.Precio)}");
        }

        private static void MostrarProductosBajoStock()
        {
            Console.Clear();

            if (Produtos.Inventario.productos.Count == 0)
            {
                Console.WriteLine("No hay productos registrados en el inventario.");
                return;
            }

            // Solicita un parámetro configurable para el límite de bajo stock
            Console.Write("Ingrese el límite de unidades para considerar bajo stock (recomendado: 5): ");
            int limite;
            if (!int.TryParse(Console.ReadLine(), out limite) || limite < 0)
            {
                Console.WriteLine("Valor inválido. Se usará el valor predeterminado: 5");
                limite = 5;
            }

            // Filtrado LINQ: productos con cantidad menor o igual al límite pero mayor a cero
            var productosBajoStock = Produtos.Inventario.productos
                .Where(p => p.Cantidad <= limite && p.Cantidad > 0)
                .OrderBy(p => p.Cantidad)
                .ToList();

            if (productosBajoStock.Count == 0)
            {
                Console.WriteLine($"No hay productos con stock menor o igual a {limite} unidades.");
                return;
            }

            // Muestra resultados
            Console.WriteLine($"PRODUCTOS CON BAJO STOCK (≤ {limite} unidades)");
            Console.WriteLine("=================================================================================");
            Console.WriteLine("ID\tNombre\t\tMarca\t\tCantidad\tPrecio");
            Console.WriteLine("=================================================================================");

            foreach (var p in productosBajoStock)
            {
                Console.WriteLine($"{p.ID}\t{p.Nombre}\t\t{p.Marca}\t\t{p.Cantidad}\t\t{p.Precio}");
            }
        }

        private static void MostrarProductosPorVencer()
        {
            Console.Clear();

            if (Produtos.Inventario.productos.Count == 0)
            {
                Console.WriteLine("No hay productos registrados en el inventario.");
                return;
            }

            // Solicita parámetro configurable para el número de días
            Console.Write("Ingrese el número de días para considerar productos próximos a vencer (recomendado: 30): ");
            int dias;
            if (!int.TryParse(Console.ReadLine(), out dias) || dias < 0)
            {
                Console.WriteLine("Valor inválido. Se usará el valor predeterminado: 30 días");
                dias = 30;
            }

            var fechaLimite = DateTime.Now.AddDays(dias);
            var productosPorVencer = new List<Produtos.Producto>();

            // Filtra productos por vencer analizando las fechas
            foreach (var p in Produtos.Inventario.productos)
            {
                if (p.Cantidad <= 0) continue;  // Ignora productos sin stock

                DateTime fechaVencimiento;
                if (DateTime.TryParse(p.FechaVencimiento, out fechaVencimiento))
                {
                    if (fechaVencimiento <= fechaLimite)
                    {
                        productosPorVencer.Add(p);
                    }
                }
            }

            if (productosPorVencer.Count == 0)
            {
                Console.WriteLine($"No hay productos que venzan en los próximos {dias} días.");
                return;
            }

            // Ordena los productos por fecha de vencimiento (más próximos primero)
            productosPorVencer = productosPorVencer.OrderBy(p => {
                DateTime fechaVencimiento;
                DateTime.TryParse(p.FechaVencimiento, out fechaVencimiento);
                return fechaVencimiento;
            }).ToList();

            // Muestra resultados
            Console.WriteLine($"PRODUCTOS POR VENCER EN LOS PRÓXIMOS {dias} DÍAS");
            Console.WriteLine("=================================================================================");
            Console.WriteLine("ID\tNombre\t\tMarca\t\tCantidad\tFecha Vencimiento");
            Console.WriteLine("=================================================================================");

            foreach (var p in productosPorVencer)
            {
                Console.WriteLine($"{p.ID}\t{p.Nombre}\t\t{p.Marca}\t\t{p.Cantidad}\t\t{p.FechaVencimiento}");
            }
        }
    }
}