using System;
using System.Collections.Generic;
using System.Linq;

namespace Proyecto_Inventario
{
    public class Ventas
    {
        public class Venta
        {
            // Estructura de datos para almacenar información de ventas
            public int ID { get; set; }
            public int IDProducto { get; set; }
            public string NombreProducto { get; set; }
            public int Cantidad { get; set; }
            public int PrecioUnitario { get; set; }
            public int Total { get; set; }
            public DateTime Fecha { get; set; }
        }
        // Lista que almacena todas las ventas realizadas
        public static List<Venta> ventas = new List<Venta>();

        static Ventas()
        {
            // Cargar ventas desde Excel al iniciar
            ventas = ExcelHelper.CargarVentas();
        }

        public static void RunVentas()
        {
            Console.Clear();
            bool salir = false;

            while (!salir)
            {
                Console.WriteLine("----------------------");
                Console.WriteLine("    MENÚ DE VENTAS    ");
                Console.WriteLine("----------------------");
                Console.WriteLine("1. REALIZAR VENTA");
                Console.WriteLine("2. LISTAR VENTAS");
                Console.WriteLine("3. BUSCAR VENTA POR ID");
                Console.WriteLine("4. REPORTE DE VENTAS POR PRODUCTO");
                Console.WriteLine("5. REPORTE DE VENTAS MENSUALES");
                Console.WriteLine("6. VOLVER AL MENÚ PRINCIPAL");
                Console.WriteLine("**********************");
                Console.Write("Seleccione una opción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        RealizarVenta();
                        break;
                    case "2":
                        ListarVentas();
                        break;
                    case "3":
                        BuscarVenta();
                        break;
                    case "4":
                        ReporteVentasPorProducto();
                        break;
                    case "5":
                        ReporteVentasMensuales();
                        break;
                    case "6":
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

        private static void RealizarVenta()
        {
            Console.Clear();

            // Verificar si hay productos disponibles
            if (Produtos.Inventario.productos.Count == 0)
            {
                Console.WriteLine("No hay productos en el inventario para vender.");
                return;
            }

            // Mostrar productos disponibles
            Console.WriteLine("PRODUCTOS DISPONIBLES:");
            Console.WriteLine("=================================================================================");
            Console.WriteLine("ID\tNombre\t\tMarca\t\tCantidad\tPrecio");
            Console.WriteLine("=================================================================================");

            foreach (var p in Produtos.Inventario.productos)
            {
                Console.WriteLine($"{p.ID}\t{p.Nombre}\t\t{p.Marca}\t\t{p.Cantidad}\t\t{p.Precio}");
            }

            // Solicitar ID del producto a vender
            Console.Write("\nIngrese el ID del producto a vender: ");
            int idProducto;
            if (!int.TryParse(Console.ReadLine(), out idProducto))
            {
                Console.WriteLine("Error: ID inválido. Debe ingresar un número.");
                return;
            }

            // Buscar el producto
            var producto = Produtos.Inventario.productos.FirstOrDefault(p => p.ID == idProducto);
            if (producto == null)
            {
                Console.WriteLine("Producto no encontrado.");
                return;
            }

            // Verificar si hay stock disponible
            if (producto.Cantidad <= 0)
            {
                Console.WriteLine($"No hay stock disponible para el producto {producto.Nombre}.");
                return;
            }

            // Solicitar cantidad a vender
            Console.Write($"Ingrese la cantidad a vender (disponible: {producto.Cantidad}): ");
            int cantidad;
            if (!int.TryParse(Console.ReadLine(), out cantidad))
            {
                Console.WriteLine("Error: Cantidad inválida. Debe ingresar un número.");
                return;
            }

            // Validar cantidad
            if (cantidad <= 0)
            {
                Console.WriteLine("La cantidad debe ser mayor a cero.");
                return;
            }
            if (cantidad > producto.Cantidad)
            {
                Console.WriteLine($"No hay suficiente stock. Solo hay {producto.Cantidad} unidades disponibles.");
                return;
            }

            // Calcular total
            int total = cantidad * producto.Precio;

            // Crear registro de venta
            Venta nuevaVenta = new Venta
            {
                ID = ventas.Count > 0 ? ventas.Max(v => v.ID) + 1 : 1,
                IDProducto = producto.ID,
                NombreProducto = producto.Nombre,
                Cantidad = cantidad,
                PrecioUnitario = producto.Precio,
                Total = total,
                Fecha = DateTime.Now
            };

            // Restar del inventario
            producto.Cantidad -= cantidad;

            // Agregar venta a la lista
            ventas.Add(nuevaVenta);

            // Guardar cambios en Excel
            ExcelHelper.GuardarProductos(Produtos.Inventario.productos);
            ExcelHelper.GuardarVentas(ventas);

            Console.WriteLine("\nVenta realizada con éxito:");
            Console.WriteLine($"Producto: {producto.Nombre}");
            Console.WriteLine($"Cantidad: {cantidad}");
            Console.WriteLine($"Precio unitario: {producto.Precio}");
            Console.WriteLine($"Total: {total}");
            Console.WriteLine($"Fecha: {nuevaVenta.Fecha}");
        }

        private static void ListarVentas()
        {
            Console.Clear();

            if (ventas.Count == 0)
            {
                Console.WriteLine("No hay ventas registradas.");
                return;
            }

            Console.WriteLine("LISTADO DE VENTAS");
            Console.WriteLine("=================================================================================");
            Console.WriteLine("ID\tProducto\tCantidad\tPrecio Unit.\tTotal\tFecha");
            Console.WriteLine("=================================================================================");

            foreach (var v in ventas)
            {
                Console.WriteLine($"{v.ID}\t{v.NombreProducto}\t{v.Cantidad}\t\t{v.PrecioUnitario}\t\t{v.Total}\t{v.Fecha.ToShortDateString()}");
            }

            Console.WriteLine($"\nTotal de ventas: {ventas.Sum(v => v.Total)}");
        }

        private static void BuscarVenta()
        {
            Console.Clear();

            if (ventas.Count == 0)
            {
                Console.WriteLine("No hay ventas registradas.");
                return;
            }

            Console.Write("Ingrese el ID de la venta a buscar: ");
            int idBuscar;
            if (!int.TryParse(Console.ReadLine(), out idBuscar))
            {
                Console.WriteLine("Error: ID inválido. Debe ingresar un número.");
                return;
            }

            var venta = ventas.FirstOrDefault(v => v.ID == idBuscar);

            if (venta != null)
            {
                Console.WriteLine("\nVenta encontrada:");
                Console.WriteLine($"ID: {venta.ID}");
                Console.WriteLine($"Producto: {venta.NombreProducto}");
                Console.WriteLine($"Cantidad: {venta.Cantidad}");
                Console.WriteLine($"Precio unitario: {venta.PrecioUnitario}");
                Console.WriteLine($"Total: {venta.Total}");
                Console.WriteLine($"Fecha: {venta.Fecha}");
            }
            else
            {
                Console.WriteLine("Venta no encontrada.");
            }
        }

        private static void ReporteVentasPorProducto()
        {
            Console.Clear();

            if (ventas.Count == 0)
            {
                Console.WriteLine("No hay ventas registradas para generar el reporte.");
                return;
            }

            var ventasPorProducto = ventas
                .GroupBy(v => v.IDProducto)
                .Select(g => new
                {
                    ProductoID = g.Key,
                    NombreProducto = g.First().NombreProducto,
                    CantidadTotal = g.Sum(v => v.Cantidad),
                    MontoTotal = g.Sum(v => v.Total)
                })
                .OrderByDescending(x => x.MontoTotal)
                .ToList();

            Console.WriteLine("REPORTE DE VENTAS POR PRODUCTO");
            Console.WriteLine("=================================================================================");
            Console.WriteLine("ID\tProducto\tCantidad Total\tMonto Total");
            Console.WriteLine("=================================================================================");

            foreach (var item in ventasPorProducto)
            {
                Console.WriteLine($"{item.ProductoID}\t{item.NombreProducto}\t{item.CantidadTotal}\t\t{item.MontoTotal}");
            }

            Console.WriteLine($"\nTotal general: {ventasPorProducto.Sum(v => v.MontoTotal)}");
        }

        private static void ReporteVentasMensuales()
        {
            Console.Clear();

            if (ventas.Count == 0)
            {
                Console.WriteLine("No hay ventas registradas para generar el reporte.");
                return;
            }

            // Crear matriz 12 meses x 2 columnas (total ventas, cantidad ventas)
            int[,] ventasMensuales = new int[12, 2];

            // Procesar todas las ventas
            foreach (var venta in ventas)
            {
                int mes = venta.Fecha.Month - 1; // 0-11
                ventasMensuales[mes, 0] += venta.Total;    // Sumar total
                ventasMensuales[mes, 1] += venta.Cantidad; // Sumar cantidad
            }

            // Mostrar reporte
            Console.WriteLine("REPORTE DE VENTAS MENSUALES (MATRIZ BIDIMENSIONAL)");
            Console.WriteLine("==================================================");
            Console.WriteLine("Mes\t\tTotal Ventas\tCantidad Vendida");
            Console.WriteLine("==================================================");

            string[] nombresMeses = {"Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
                            "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"};

            for (int i = 0; i < 12; i++)
            {
                Console.WriteLine($"{nombresMeses[i]}\t{ventasMensuales[i, 0]}\t\t{ventasMensuales[i, 1]}");
            }
        }
    }
}