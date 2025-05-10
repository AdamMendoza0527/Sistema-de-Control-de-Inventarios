using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClosedXML.Excel; // Paquete NuGet ClosedXML

namespace Proyecto_Inventario
{
    public static class ExcelHelper
    {
        // Constantes que definen el nombre del archivo y las hojas de Excel
        private const string ExcelFile = "Inventario.xlsx";
        private const string SheetProductos = "Productos";
        private const string SheetVentas = "Ventas";

        public static void GuardarProductos(List<Produtos.Producto> productos)
        {
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add(SheetProductos);

                    // Define los encabezados de las columnas
                    worksheet.Cell(1, 1).Value = "ID";
                    worksheet.Cell(1, 2).Value = "Nombre";
                    worksheet.Cell(1, 3).Value = "Marca";
                    worksheet.Cell(1, 4).Value = "FechaVencimiento";
                    worksheet.Cell(1, 5).Value = "Cantidad";
                    worksheet.Cell(1, 6).Value = "Precio";
                    worksheet.Cell(1, 7).Value = "Proveedor";

                    // Guarda cada producto en una fila de la hoja de Excel
                    for (int i = 0; i < productos.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = productos[i].ID;
                        worksheet.Cell(i + 2, 2).Value = productos[i].Nombre;
                        worksheet.Cell(i + 2, 3).Value = productos[i].Marca;
                        worksheet.Cell(i + 2, 4).Value = productos[i].FechaVencimiento;
                        worksheet.Cell(i + 2, 5).Value = productos[i].Cantidad;
                        worksheet.Cell(i + 2, 6).Value = productos[i].Precio;
                        worksheet.Cell(i + 2, 7).Value = productos[i].Proveedor;
                    }

                    // Copiar hoja de ventas si existe
                    if (File.Exists(ExcelFile))
                    {
                        using (var existingWorkbook = new XLWorkbook(ExcelFile))
                        {
                            if (existingWorkbook.Worksheets.Contains(SheetVentas))
                            {
                                var existingVentasSheet = existingWorkbook.Worksheet(SheetVentas);
                                existingVentasSheet.CopyTo(workbook, SheetVentas);
                            }
                        }
                    }

                    workbook.SaveAs(ExcelFile);
                }

                Console.WriteLine("Datos guardados en Excel correctamente.");
            }
            catch (Exception ex)
            {
                // Captura errores específicos de operaciones de archivo
                Console.WriteLine($"Error al guardar datos en Excel: {ex.Message}");
            }
        }

        public static List<Produtos.Producto> CargarProductos()
        {
            var productos = new List<Produtos.Producto>();

            try
            {
                if (File.Exists(ExcelFile))
                {
                    using (var workbook = new XLWorkbook(ExcelFile))
                    {
                        if (workbook.Worksheets.Contains(SheetProductos))
                        {
                            var worksheet = workbook.Worksheet(SheetProductos);
                            var firstRow = true;

                            foreach (var row in worksheet.RowsUsed())
                            {
                                if (firstRow)
                                {
                                    firstRow = false;
                                    continue;
                                }

                                try
                                {
                                    // Convierte los datos de Excel a objetos Producto
                                    int id = int.Parse(row.Cell(1).Value.ToString());
                                    string nombre = row.Cell(2).Value.ToString();
                                    string marca = row.Cell(3).Value.ToString();
                                    string fechaVencimiento = row.Cell(4).Value.ToString();
                                    int cantidad = int.Parse(row.Cell(5).Value.ToString());
                                    int precio = int.Parse(row.Cell(6).Value.ToString());
                                    string proveedor = row.Cell(7).Value.ToString();

                                    var producto = new Produtos.Producto
                                    {
                                        ID = id,
                                        Nombre = nombre,
                                        Marca = marca,
                                        FechaVencimiento = fechaVencimiento,
                                        Cantidad = cantidad,
                                        Precio = precio,
                                        Proveedor = proveedor
                                    };

                                    productos.Add(producto);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error al procesar fila: {ex.Message}");
                                }
                            }
                        }
                    }

                    Console.WriteLine("Datos cargados desde Excel correctamente.");
                }
            }
            catch (Exception ex)
            {
                // Captura errores generales de lectura de archivo
                Console.WriteLine($"Error al cargar datos desde Excel: {ex.Message}");
            }

            return productos;
        }

        public static void GuardarVentas(List<Ventas.Venta> ventas)
        {
            try
            {
                using (var workbook = File.Exists(ExcelFile) ? new XLWorkbook(ExcelFile) : new XLWorkbook())
                {
                    if (workbook.Worksheets.Contains(SheetVentas))
                    {
                        workbook.Worksheet(SheetVentas).Delete();
                    }

                    var worksheet = workbook.Worksheets.Add(SheetVentas);

                    // Encabezados
                    worksheet.Cell(1, 1).Value = "ID";
                    worksheet.Cell(1, 2).Value = "IDProducto";
                    worksheet.Cell(1, 3).Value = "Producto";
                    worksheet.Cell(1, 4).Value = "Cantidad";
                    worksheet.Cell(1, 5).Value = "PrecioUnitario";
                    worksheet.Cell(1, 6).Value = "Total";
                    worksheet.Cell(1, 7).Value = "Fecha";

                    // Datos
                    for (int i = 0; i < ventas.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = ventas[i].ID;
                        worksheet.Cell(i + 2, 2).Value = ventas[i].IDProducto;
                        worksheet.Cell(i + 2, 3).Value = ventas[i].NombreProducto;
                        worksheet.Cell(i + 2, 4).Value = ventas[i].Cantidad;
                        worksheet.Cell(i + 2, 5).Value = ventas[i].PrecioUnitario;
                        worksheet.Cell(i + 2, 6).Value = ventas[i].Total;
                        worksheet.Cell(i + 2, 7).Value = ventas[i].Fecha.ToString("yyyy-MM-dd HH:mm:ss");
                    }

                    workbook.SaveAs(ExcelFile);
                }

                Console.WriteLine("Ventas guardadas en Excel correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar ventas en Excel: {ex.Message}");
            }
        }

        public static List<Ventas.Venta> CargarVentas()
        {
            var ventas = new List<Ventas.Venta>();

            try
            {
                if (File.Exists(ExcelFile))
                {
                    using (var workbook = new XLWorkbook(ExcelFile))
                    {
                        if (workbook.Worksheets.Contains(SheetVentas))
                        {
                            var worksheet = workbook.Worksheet(SheetVentas);
                            var firstRow = true;

                            foreach (var row in worksheet.RowsUsed())
                            {
                                if (firstRow)
                                {
                                    firstRow = false;
                                    continue;
                                }

                                try
                                {
                                    var venta = new Ventas.Venta
                                    {
                                        ID = int.Parse(row.Cell(1).Value.ToString()),
                                        IDProducto = int.Parse(row.Cell(2).Value.ToString()),
                                        NombreProducto = row.Cell(3).Value.ToString(),
                                        Cantidad = int.Parse(row.Cell(4).Value.ToString()),
                                        PrecioUnitario = int.Parse(row.Cell(5).Value.ToString()),
                                        Total = int.Parse(row.Cell(6).Value.ToString()),
                                        Fecha = DateTime.Parse(row.Cell(7).Value.ToString())
                                    };

                                    ventas.Add(venta);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error al procesar fila de venta: {ex.Message}");
                                }
                            }
                        }
                    }

                    Console.WriteLine("Ventas cargadas desde Excel correctamente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar ventas desde Excel: {ex.Message}");
            }

            return ventas;
        }
    }
}