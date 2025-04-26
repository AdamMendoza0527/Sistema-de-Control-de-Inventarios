using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using static ejemplis.FileName;

namespace ejemplis
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool valido = true;

            SISTEMA sistema = new SISTEMA();
            sistema.Bienvenida(); ;

            while (valido)
            {
                Console.WriteLine("----------------------");
                Console.WriteLine("    MENU PRINCIPAL    ");
                Console.WriteLine("----------------------");
                Console.WriteLine("1. AGREGAR PRODUCTO");
                Console.WriteLine("2. MOSTRAR PRODUCTO");
                Console.WriteLine("3. BUSCAR PRODUCTO");
                Console.WriteLine("4. MODIFICAR PRODUCTO");
                Console.WriteLine("5. ELIMINAR PRODUCTO");
                Console.WriteLine("6. SALIR");
                Console.WriteLine("**********************");

                int opcion = Convert.ToInt32(Console.ReadLine());

                switch (opcion)
                {
                    case 1:
                        Console.WriteLine("USTED HA INGRESADO EN LA OPCION 1");
                        string nombre = Console.ReadLine();
                        Console.WriteLine("Ingrese el nombre del producto");
                        string producto = Console.ReadLine();
                        Console.WriteLine("Ingrese el precio del producto");
                        int precio = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Ingrese cantidad del producto");
                        int cantidad = Convert.ToInt32(Console.ReadLine());


                        Console.ReadKey();
                        break;
                    case 2:
                        Console.WriteLine("USTED HA INGRESADO EN LA OPCION 2");
                        Console.ReadKey();
                        break;
                    case 3:
                        Console.WriteLine("USTED HA INGRESADO EN LA OPCION 3");
                        Console.ReadKey();
                        break;
                    case 4:
                        Console.WriteLine("USTED HA INGRESADO EN LA OPCION 4");
                        Console.ReadKey();
                        break;
                    case 5:
                        Console.WriteLine("USTED HA INGRESADO EN LA OPCION 5");
                        Console.ReadKey();
                        break;
                    case 6:
                        Console.WriteLine("¿ESTA SEGURO QUE DESEA SALIR?");
                        Console.WriteLine("S/N");
                        string desicion = Console.ReadLine();
                        if (desicion == "S")
                        {
                            valido = false;
                        }
                        Console.ReadKey();
                        break;
                    default:
                        Console.WriteLine("USTED INGRESO A UNA OPCION NO VALIDA.");
                        Console.ReadKey();
                        break;
                }
            } 
        }
    }
}
