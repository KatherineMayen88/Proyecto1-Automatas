using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace pruebasssssssssss
{

    internal class Program
    {
        static string no_estados;
        static string estado_inicial;
        static string[] estado_final;
        static string[,] transiciones;
        static string cadena; 


        //LECTURA ARCHIVO Y ASIGNACION DE VALOR A VARIABLES GLOBALES
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
                        ArchivoTXT(filePathCorrecto, fileExtension);
                        break;
                    case ".csv":
                        ArchivoCSV(filePathCorrecto, fileExtension);
                        break;
                    case ".json":
                        ArchivoJSON(filePathCorrecto, fileExtension);
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

        static void ArchivoTXT(string filePath, string fileExtension)
        {
            Console.WriteLine("\nArchivo de extensión " + fileExtension +"\n");

            // Leer todas las líneas del archivo
            string[] lines = File.ReadAllLines(filePath);

            // Asegurarse de que el archivo tiene al menos 4 líneas
            if (lines.Length >= 4)
            {
                // Asignar contenido a las variables globales
                no_estados = lines[0];
                estado_inicial = lines[1];
                estado_final = lines[2].Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // Crear una matriz para las líneas restantes
                // Asumiendo que cada línea tiene exactamente tres números separados por comas
                int filas = lines.Length - 3;
                transiciones = new string[filas, 3];

                // Procesar las líneas restantes y almacenarlas en la matriz
                for (int i = 3; i < lines.Length; i++)
                {
                    string[] partes = lines[i].Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    transiciones[i - 3, 0] = partes[0];
                    transiciones[i - 3, 1] = partes[1];
                    transiciones[i - 3, 2] = partes[2];
                }

                //imprimir datos
                datosAutomata(no_estados, estado_inicial, estado_final, transiciones, filas);
            }
            else
            {
                Console.WriteLine("El archivo no tiene suficientes líneas.");
            }

            ConsultarCadena();
        }

        static void ArchivoCSV(string filePath, string fileExtension)
        {
            Console.WriteLine("\nArchivo de extensión " + fileExtension + "\n");

            // Leer todas las líneas del archivo
            string[] lines = File.ReadAllLines(filePath);

            // Asegurarse de que el archivo tiene al menos 4 líneas
            if (lines.Length >= 4)
            {
                // Asignar contenido a las variables globales
                no_estados = lines[0].Trim().Replace(",", "");
                estado_inicial = lines[1].Trim().Replace(",", ""); ;
                estado_final = lines[2].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();

                // Crear una matriz para las líneas restantes
                int filas = lines.Length - 3;
                transiciones = new string[filas, 3];

                // Procesar las líneas restantes y almacenarlas en la matriz
                for (int i = 3; i < lines.Length; i++)
                {
                    string[] partes = lines[i].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (partes.Length == 3) // Asegurarse de que hay exactamente 3 partes
                    {
                        for (int j = 0; j < partes.Length; j++)
                        {
                            transiciones[i - 3, j] = partes[j].Trim();
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Línea {i + 1} no tiene 3 elementos: {lines[i]}");
                    }
                }

                //imprimir datos
                datosAutomata(no_estados, estado_inicial, estado_final, transiciones, filas);
            }
            else
            {
                Console.WriteLine("El archivo no tiene suficientes líneas.");
            }

            ConsultarCadena(); 
        }
    
        static void ArchivoJSON(string filePath, string fileExtension)
        {
            Console.WriteLine("\nArchivo de extensión " + fileExtension + "\n");

            // Lee todo el contenido del archivo
            string jsonString = File.ReadAllText(filePath);

            // Analiza la cadena JSON en un objeto JObject
            JObject jsonObject = JObject.Parse(jsonString);

            // Accede a los valores directamente
            no_estados = jsonObject["no_estados"].Value<string>();
            estado_inicial = jsonObject["estado_inicial"].Value<string>();
            estado_final = jsonObject["estado_final"].ToObject<string[]>();

            // Accede al array de datos
            var datosArray = jsonObject["transiciones"].Value<JArray>();

            // Crear una matriz para los datos
            int filas = datosArray.Count;
            transiciones = new string[filas, 3];

            // Llenar la matriz con los datos del array
            for (int i = 0; i < filas; i++)
            {
                JArray filaDatos = (JArray)datosArray[i];
                transiciones[i, 0] = (string)filaDatos[0];
                transiciones[i, 1] = (string)filaDatos[1];
                transiciones[i, 2] = (string)filaDatos[2];
            }

            datosAutomata(no_estados, estado_inicial, estado_final, transiciones, filas);
            ConsultarCadena();
        }


        //Imprimir datos del txt
        static void datosAutomata(string cantEst, string estInicial, string[] estFinal, string[,] transc, int cantFilas)
        {
            Console.WriteLine("DESCRIPCIÓN DEL AUTÓMATA:");
            Console.WriteLine($"No. de estados: {cantEst}");
            Console.WriteLine($"Estado inicial: {estInicial}");
            Console.Write($"Estado final: ");
            foreach (var estado in estFinal)
            {
                Console.Write(estado + "  ");
            }
            
            Console.WriteLine("\nTransiciones: ");
            for (int i = 0; i < cantFilas; i++)
            {
                Console.WriteLine($"{transiciones[i, 0]}, {transiciones[i, 1]}, {transiciones[i, 2]}");
            }

        }


      

        //DIEGOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO
        static void ConsultarCadena()
        {
            Console.WriteLine("\n\nCONSULTA DE CADENA: ");
            Console.WriteLine("Ingrese su cadena de caracteres:");
            cadena = Console.ReadLine();

            //verifica que la cadena no esté vacía
            if (!string.IsNullOrEmpty(cadena))
            {
                Console.WriteLine("\nRecorrido:");
                RecorrerAF(estado_inicial, cadena, 0);
            }
            else
            {
                Console.WriteLine("El string está vacío.");
            }
        }

        static void RecorrerAF(string estActual, string cadena, int contador)
        {
            string[] caracteres = cadena.ToCharArray().Select(c => c.ToString()).ToArray(); //separa la cadena ingresada en un arreglo string

            if (contador == caracteres.Length)
            {
                VerifEstFinal(estActual);
            }
            else
            {
                string sigEstado = SigEstado(caracteres[contador], estActual);
                ImprimirPaso(estActual, caracteres[contador], sigEstado, contador, cadena);
            }
        }

        static string SigEstado(string caracter, string estado)
        {
            for (int i = 0; i < transiciones.GetLength(0); i++)//recorre toda la matriz
            {
                if (estado.Equals(transiciones[i, 0]))//se compara estado recibido con la primera posicion de cada fila(estado actual)
                {
                    if (caracter.Equals(transiciones[i, 1]))//Si coincide se compara con la segunda posición el cual es "la letra"
                    {
                        return transiciones[i, 2];//Si ambas coinciden se retorna el siguiente estado
                    }
                }
            }

            if (estado.ToUpper() == "E")
            {
                return "E";
            }
            else
            {
                return ("no se encontró el estado " + estado);

            }
        }

        static void ImprimirPaso(string estActual, string caracter, string sigEstado, int contador, string cadena)
        {
            int longitudCad = cadena.Length;

            if (contador < longitudCad)
            {
                Console.WriteLine(estActual + " -> " + caracter + " -> " + sigEstado);
                contador++;//aumenta el contador para seguir recorriendo la cadena
                estActual = sigEstado;//siguiente estado pasa a ser el actual para volver a llamar RecorrerAF

                RecorrerAF(estActual, cadena, contador);
            }
        }

        static void VerifEstFinal(string estActual)
        {
            bool esEstadoFinal = estado_final.Contains(estActual);

            if (esEstadoFinal)
            {
                Console.WriteLine("El recorrido dirige al estado '" + estActual + "' que si es un estado final, la palabra SI es aceptable.");
            }
            else if (estActual == "E")
            {
                Console.WriteLine("El recorrido dirige a Epsilon. \nEl estado '" + estActual + "' no está permitido, la palabra NO es aceptable.");
            }
            else
            {
                Console.WriteLine("El recorrido dirige al estado '" + estActual + "' que no es un estado final, la palabra NO es aceptable.");
            }
        }



        static void Main(string[] args)
        {
            Console.WriteLine("Ingrese el path del archivo que describa al automata");
            string filePath = Console.ReadLine();
            ReadFileAndAssign(filePath);

            Console.ReadKey();
        }
    }
}
