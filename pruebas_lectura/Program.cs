using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace pruebas_lectura
{

    internal class Program
    {
        static string no_estados;
        static string estado_inicial;
        static string[] estado_final;

        static void ReadFileAndAssign(string path)
        {
            string filePathCorrecto = path.Trim('"'); //se guarda el path sin las comillas, si se guarda con comillas no funciona.
            string fileExtension = Path.GetExtension(filePathCorrecto); //obtiene la extension del path

            // Verificar que el archivo exista
            if (File.Exists(filePathCorrecto))
            {
                switch (fileExtension.ToLower())
                {
                    case ".txt":
                    case ".csv":
                        Console.WriteLine("Archivo de extensión " + fileExtension);
                        ArchivoTXTyCSV(filePathCorrecto);
                        break;
                    case ".json":
                        Console.WriteLine("Archivo de extensión " + fileExtension);
                        ArchivoJSON(filePathCorrecto);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Console.WriteLine($"El archivo en la ruta {filePathCorrecto} no existe.");

            }
        }

        static void ArchivoTXTyCSV(string filePath)
        {
            // Leer todas las líneas del archivo
            string[] lines = File.ReadAllLines(filePath);

            // Asegurarse de que el archivo tiene al menos 4 líneas
            if (lines.Length >= 4)
            {
                // Asignar contenido a las variables globales
                no_estados = lines[0];
                estado_inicial = lines[1];
                estado_final = lines[2].Split(new char[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries);

                // Crear una matriz para las líneas restantes
                // Asumiendo que cada línea tiene exactamente tres números separados por comas
                int filas = lines.Length - 3;
                string[,] datos = new string[filas, 3];

                // Procesar las líneas restantes y almacenarlas en la matriz
                for (int i = 3; i < lines.Length; i++)
                {
                    string[] partes = lines[i].Split(new char[] {',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    datos[i - 3, 0] = partes[0];
                    datos[i - 3, 1] = partes[1];
                    datos[i - 3, 2] = partes[2];
                }

                // Imprimir las variables y la matriz para verificación
                Console.WriteLine($"No. de estados: {no_estados}");
                Console.WriteLine($"Estado inicial: {estado_inicial}");
                Console.WriteLine($"Estado final: ");
                foreach (var estado in estado_final)
                {
                    Console.WriteLine(estado);
                }

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

        static void ArchivoJSON(string filePath)
        {
            // Lee todo el contenido del archivo
            string jsonString = File.ReadAllText(filePath);

            // Analiza la cadena JSON en un objeto JObject
            JObject jsonObject = JObject.Parse(jsonString);

            // Accede a los valores directamente
            no_estados = jsonObject["no_estados"].Value<string>();
            estado_inicial = jsonObject["estado_inicial"].Value<string>();
            estado_final = jsonObject["estado_final"].ToObject<string[]>();

            // Accede al array de datos
            var datosArray = jsonObject["datos"].Value<JArray>();

            // Crear una matriz para los datos
            int filas = datosArray.Count;
            string[,] datos = new string[filas, 3];

            // Llenar la matriz con los datos del array
            for (int i = 0; i < filas; i++)
            {
                JArray filaDatos = (JArray)datosArray[i];
                datos[i, 0] = (string)filaDatos[0];
                datos[i, 1] = (string)filaDatos[1];
                datos[i, 2] = (string)filaDatos[2];
            }

            // Imprimir las variables y la matriz para verificación
            Console.WriteLine($"No. de estados: {no_estados}");
            Console.WriteLine($"Estado inicial: {estado_inicial}");
            Console.WriteLine($"Estado final: ");
            foreach (var estado in estado_final)
            {
                Console.WriteLine(estado);
            }

            for (int i = 0; i < filas; i++)
            {
                Console.WriteLine($"{datos[i, 0]}, {datos[i, 1]}, {datos[i, 2]}");
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
