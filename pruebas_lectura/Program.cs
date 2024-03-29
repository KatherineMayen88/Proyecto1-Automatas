using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace pruebas_lectura
{
    
    internal class Program
    {
        static string primeraLinea;
        static string segundaLinea;
        static string terceraLinea;
        static string globalVar4;


        static void ReadFileAndAssign(string path)
        {
            string filePathCorrecto = path.Trim('"'); //se guarda el path sin las comillas, si se guarda con comillas no funciona.
            string fileExtension = Path.GetExtension(filePathCorrecto); //obtiene la extension del path
       
            // Verificar que el archivo exista
            if (File.Exists(filePathCorrecto))
            {
                // Leer todas las líneas del archivo
                string[] lines = File.ReadAllLines(filePathCorrecto);

                // Asegurarse de que el archivo tiene al menos 4 líneas
                if (lines.Length >= 0)
                {
                    // Asignar contenido a las variables globales
                    primeraLinea = lines[0];
                    segundaLinea = lines[1];
                    terceraLinea = lines[2];

                    // Crear una matriz para las líneas restantes
                    // Asumiendo que cada línea tiene exactamente tres números separados por comas
                    int filas = lines.Length - 3;
                    int[,] datos = new int[filas, 3];

                    // Procesar las líneas restantes y almacenarlas en la matriz
                    for (int i = 3; i < lines.Length; i++)
                    {
                        string[] partes = lines[i].Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        datos[i - 3, 0] = int.Parse(partes[0]);
                        datos[i - 3, 1] = int.Parse(partes[1]);
                        datos[i - 3, 2] = int.Parse(partes[2]);
                    }

                    // Imprimir las variables y la matriz para verificación
                    Console.WriteLine($"Primera línea: {primeraLinea}");
                    Console.WriteLine($"Segunda línea: {segundaLinea}");
                    Console.WriteLine($"Tercera línea: {terceraLinea}");

                    for (int i = 0; i < filas; i++)
                    {
                        Console.WriteLine($"{datos[i, 0]}, {datos[i, 1]}, {datos[i, 2]}");
                    }

                }
                else
                {
                    Console.WriteLine("El archivo no tiene suficientes líneas.");
                }
            }
            else
            {
                Console.WriteLine($"El archivo en la ruta {filePathCorrecto} no existe.");
            }

        }




        static void Main(string[] args)
        {
            
            Console.WriteLine("Ingrese el path del archivo que describa al automata");
            string filePath = Console.ReadLine();

            /*
            string filePathSinComillas = filePath.Trim('"');
            string fileExtension = Path.GetExtension(filePathSinComillas);
            Console.WriteLine(fileExtension);
            */

            ReadFileAndAssign(filePath);
          
            Console.ReadKey();
        }
    }


   

}
