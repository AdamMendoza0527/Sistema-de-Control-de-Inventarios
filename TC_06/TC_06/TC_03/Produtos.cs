using System;
using System.Collections.Generic;
using System.Linq;

namespace Proyecto_Inventario
{
    public class Produtos
    {
        public class Producto
        {
            // Clase que define la estructura de datos para los productos
            public int ID { get; set; }
            public string Nombre { get; set; }
            public string Marca { get; set; }
            public string FechaVencimiento { get; set; }
            public int Cantidad { get; set; }
            public int Precio { get; set; }
            public string Proveedor { get; set; }
        }

        public static class Inventario
        {
            // Colección principal que almacena todos los productos
            public static List<Producto> productos = new List<Producto>();

            static Inventario()
            {
                // Cargar productos desde Excel al iniciar la aplicación
                productos = ExcelHelper.CargarProductos();
            }

            public static void AgregarProducto()
            {
                Console.Clear();
                Console.Write("¿Cuántos productos desea registrar? ");
                int n;
                if (!int.TryParse(Console.ReadLine(), out n) || n <= 0)
                {
                    // Validación de entrada: asegura que se ingrese un número válido
                    Console.WriteLine("Error: Debe ingresar un número válido mayor a cero.");
                    return;
                }

                for (int i = 0; i < n; i++)
                {
                    // Genera un ID automático incrementando en 1 el ID más alto existente
                    Producto nuevo = new Producto();
                    nuevo.ID = productos.Count > 0 ? productos.Max(p => p.ID) + 1 : 1;

                    Console.WriteLine($"\nProducto {i + 1}:");

                    Console.Write("Nombre: ");
                    string nombre = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(nombre))
                    {
                        Console.WriteLine("Error: El nombre no puede estar vacío.");
                        i--; // Repetir este producto
                        continue;
                    }
                    nuevo.Nombre = nombre;

                    Console.Write("Marca: ");
                    string marca = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(marca))
                    {
                        Console.WriteLine("Error: La marca no puede estar vacía.");
                        i--; // Repetir este producto
                        continue;
                    }
                    nuevo.Marca = marca;

                    Console.Write("Fecha de vencimiento (DD/MM/AAAA): ");
                    string fechaVencimiento = Console.ReadLine();
                    DateTime fecha;
                    if (string.IsNullOrWhiteSpace(fechaVencimiento) || !DateTime.TryParse(fechaVencimiento, out fecha))
                    {
                        Console.WriteLine("Error: Debe ingresar una fecha válida en formato DD/MM/AAAA.");
                        i--; // Repetir este producto
                        continue;
                    }
                    nuevo.FechaVencimiento = fecha.ToShortDateString();

                    Console.Write("Cantidad: ");
                    int cantidad;
                    if (!int.TryParse(Console.ReadLine(), out cantidad) || cantidad < 0)
                    {
                        Console.WriteLine("Error: Debe ingresar un número entero mayor o igual a cero.");
                        i--; // Repetir este producto
                        continue;
                    }
                    nuevo.Cantidad = cantidad;

                    Console.Write("Precio: ");
                    int precio;
                    if (!int.TryParse(Console.ReadLine(), out precio) || precio <= 0)
                    {
                        Console.WriteLine("Error: Debe ingresar un número entero mayor a cero.");
                        i--; // Repetir este producto
                        continue;
                    }
                    nuevo.Precio = precio;

                    Console.Write("Proveedor: ");
                    string proveedor = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(proveedor))
                    {
                        Console.WriteLine("Error: El proveedor no puede estar vacío.");
                        i--; // Repetir este producto
                        continue;
                    }
                    nuevo.Proveedor = proveedor;

                    productos.Add(nuevo);
                }

                // Guardar en Excel después de agregar productos
                ExcelHelper.GuardarProductos(productos);
                Console.WriteLine("\nProductos agregados correctamente.");
            }

            public static void MostrarProductos()
            {
                Console.Clear();

                if (productos.Count == 0)
                {
                    Console.WriteLine("No hay productos registrados.");
                    return;
                }

                Console.WriteLine("LISTADO DE PRODUCTOS");
                Console.WriteLine("=================================================================================");
                Console.WriteLine("ID\tNombre\t\tMarca\t\tFecha Venc.\tCantidad\tPrecio\tProveedor");
                Console.WriteLine("=================================================================================");

                foreach (var p in productos)
                {
                    Console.WriteLine($"{p.ID}\t{p.Nombre}\t\t{p.Marca}\t\t{p.FechaVencimiento}\t{p.Cantidad}\t\t{p.Precio}\t{p.Proveedor}");
                }
            }

            public static void BuscarProducto()
            {
                Console.Clear();
                Console.Write("Ingrese el ID del producto a buscar: ");
                int idBuscar;
                if (!int.TryParse(Console.ReadLine(), out idBuscar))
                {
                    Console.WriteLine("Error: ID inválido. Debe ingresar un número.");
                    return;
                }

                var producto = productos.FirstOrDefault(p => p.ID == idBuscar);

                if (producto != null)
                {
                    Console.WriteLine("\nProducto encontrado:");
                    Console.WriteLine($"ID: {producto.ID}");
                    Console.WriteLine($"Nombre: {producto.Nombre}");
                    Console.WriteLine($"Marca: {producto.Marca}");
                    Console.WriteLine($"Fecha de Vencimiento: {producto.FechaVencimiento}");
                    Console.WriteLine($"Cantidad: {producto.Cantidad}");
                    Console.WriteLine($"Precio: {producto.Precio}");
                    Console.WriteLine($"Proveedor: {producto.Proveedor}");
                }
                else
                {
                    Console.WriteLine("Producto no encontrado.");
                }
            }

            public static void ModificarProducto()
            {
                Console.Clear();
                Console.Write("Ingrese el ID del producto a modificar: ");
                int idBuscar;
                if (!int.TryParse(Console.ReadLine(), out idBuscar))
                {
                    Console.WriteLine("Error: ID inválido. Debe ingresar un número.");
                    return;
                }

                var producto = productos.FirstOrDefault(p => p.ID == idBuscar);

                if (producto != null)
                {
                    Console.WriteLine("Deje en blanco si no desea modificar un campo.");

                    Console.Write($"Nuevo nombre (actual: {producto.Nombre}): ");
                    string nuevoNombre = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(nuevoNombre)) producto.Nombre = nuevoNombre;

                    Console.Write($"Nueva marca (actual: {producto.Marca}): ");
                    string nuevaMarca = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(nuevaMarca)) producto.Marca = nuevaMarca;

                    Console.Write($"Nueva fecha de vencimiento (actual: {producto.FechaVencimiento}): ");
                    string nuevaFecha = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(nuevaFecha))
                    {
                        DateTime fecha;
                        if (DateTime.TryParse(nuevaFecha, out fecha))
                        {
                            producto.FechaVencimiento = fecha.ToShortDateString();
                        }
                        else
                        {
                            Console.WriteLine("Fecha inválida. No se modificará este campo.");
                        }
                    }

                    Console.Write($"Nueva cantidad (actual: {producto.Cantidad}): ");
                    string nuevaCantidad = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(nuevaCantidad))
                    {
                        int cant;
                        if (int.TryParse(nuevaCantidad, out cant) && cant >= 0)
                        {
                            producto.Cantidad = cant;
                        }
                        else
                        {
                            Console.WriteLine("Cantidad inválida. No se modificará este campo.");
                        }
                    }

                    Console.Write($"Nuevo precio (actual: {producto.Precio}): ");
                    string nuevoPrecio = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(nuevoPrecio))
                    {
                        int precio;
                        if (int.TryParse(nuevoPrecio, out precio) && precio > 0)
                        {
                            producto.Precio = precio;
                        }
                        else
                        {
                            Console.WriteLine("Precio inválido. No se modificará este campo.");
                        }
                    }

                    Console.Write($"Nuevo proveedor (actual: {producto.Proveedor}): ");
                    string nuevoProveedor = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(nuevoProveedor)) producto.Proveedor = nuevoProveedor;

                    // Guardar cambios en Excel
                    ExcelHelper.GuardarProductos(productos);
                    Console.WriteLine("Producto modificado correctamente.");
                }
                else
                {
                    Console.WriteLine("Producto no encontrado.");
                }
            }

            public static void EliminarProducto()
            {
                Console.Clear();
                Console.Write("Ingrese el ID del producto a eliminar: ");
                int idEliminar;
                if (!int.TryParse(Console.ReadLine(), out idEliminar))
                {
                    Console.WriteLine("Error: ID inválido. Debe ingresar un número.");
                    return;
                }

                var producto = productos.FirstOrDefault(p => p.ID == idEliminar);

                if (producto != null)
                {
                    Console.WriteLine($"\nProducto a eliminar: {producto.Nombre} ({producto.Marca})");
                    Console.Write("¿Está seguro que desea eliminar este producto? (S/N): ");
                    string confirmacion = Console.ReadLine().Trim().ToUpper();

                    if (confirmacion == "S")
                    {
                        productos.Remove(producto);
                        // Guardar cambios en Excel
                        ExcelHelper.GuardarProductos(productos);
                        Console.WriteLine("Producto eliminado correctamente.");
                    }
                    else
                    {
                        Console.WriteLine("Operación cancelada.");
                    }
                }
                else
                {
                    Console.WriteLine("Producto no encontrado.");
                }
            }
            public static void OrdenarProductosPorNombre()
            {
                // Implementación del algoritmo de burbuja
                for (int i = 0; i < productos.Count - 1; i++)
                {
                    for (int j = 0; j < productos.Count - i - 1; j++)
                    {
                        if (string.Compare(productos[j].Nombre, productos[j + 1].Nombre) > 0)
                        {
                            // Intercambiar productos
                            Producto temp = productos[j];
                            productos[j] = productos[j + 1];
                            productos[j + 1] = temp;
                        }
                    }
                }

                // Guardar cambios en Excel
                ExcelHelper.GuardarProductos(productos);
                Console.WriteLine("Productos ordenados por nombre (algoritmo burbuja)");
            }
        }
    }
}