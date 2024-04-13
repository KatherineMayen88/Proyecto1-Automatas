using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using pruebasssssssssss.Entities;

namespace pruebasssssssssss
{

    internal class Program
    {
        static string cadena; //Variable global para almacenar la cadena ingresada por el usuaio


        // LECTURA ARCHIVO E INVOCACIÓN DE FUNCIONES
        public static void LeerArchivos()
        {
            const int maxIntentos = 2; //Intentos maximos para ingresar el path
            int intentos = 0; //Contador de intentos
            bool archivoAceptado = false; //Se establece en true cuando se procesa con éxito un archivo.
                                          //Se utiliza para salir del loop una vez que se ha procesado un archivo.

            //Bucle para solicitar el path del archivo y procesarlo
            while (intentos <= maxIntentos && !archivoAceptado)
            {
                Console.WriteLine("Ingrese el path del archivo: ");
                string path = Console.ReadLine();
                string trimmedPath = path.Trim('"'); //Se guarda el path sin las comillas, si se guarda con comillas no funciona.

                //Valida el path ingresado
                if (string.IsNullOrEmpty(trimmedPath))
                {
                    Console.WriteLine("Error: El path no puede esta vacío.");
                    intentos++;
                    continue;
                }

                if (!File.Exists(trimmedPath))
                {
                    Console.WriteLine($"Error: El archivo {trimmedPath} no existe.");
                    intentos++;
                    continue;
                }

                //Obtiene la extension del path
                string extension = Path.GetExtension(trimmedPath);

                switch (extension.ToLower())
                {
                    case ".txt":
                        LeerArchivoTxt(trimmedPath, extension);
                        archivoAceptado = true;
                        break;

                    case ".csv":
                        LeerArchivoCsv(trimmedPath, extension);
                        archivoAceptado = true;
                        break;

                    case ".json":
                        LeerArchivoJson(trimmedPath, extension);
                        archivoAceptado = true;
                        break;

                    default:
                        Console.WriteLine("La aplcación no soporta la extensión");
                        intentos++;
                        break;

                }
            }

            if (!archivoAceptado)
            {
                Console.WriteLine("Número de intentos exedido.");
            }

            Console.WriteLine("\nPresione cualquier tecla para regresar al menú...");
            Console.ReadKey();
            Console.Clear();
        }

        // Funcion para leer archivos .txt
        static AutomataEntity LeerArchivoTxt(string filePath, string fileExtension)
        {
            try
            {
                Console.WriteLine("\nArchivo de extensión " + fileExtension + "\n");

                //Lee todas las líneas del archivo
                List<string> lineasTxt = File.ReadAllLines(filePath).ToList();

                //Asegurar de que el archivo tiene al menos 4 líneas
                if (lineasTxt.Count < 3)
                {
                    Console.WriteLine($"Error: El archivo {filePath} no tiene el formato esperado.");
                    return null;
                }


                //Crea una nueva instancia de AutomataEntity
                AutomataEntity Automata = new AutomataEntity();
                {
                    Automata.Estados = lineasTxt[0];
                    Automata.EstadoInicial = lineasTxt[1].Split(',');
                    Automata.EstadosFinales = lineasTxt[2].Split(',');
                    Automata.Transiciones = new List<TransicionEntity>();
                };

                //Asignar las transiciones de la instacia del automata
                for (int i = 3; i < lineasTxt.Count; i++)
                {
                    string[] transicionDatos = lineasTxt[i].Split(',');

                    if (transicionDatos.Length != 3)
                    {
                        Console.WriteLine($"Advertencia: El formato de la transición en la línea {i + 1} del archivo {filePath} no es válido.");
                    }
                    Automata.Transiciones.Add(new TransicionEntity
                    {
                        EstadoOrigen = transicionDatos[0].Trim(),
                        Simbolo = transicionDatos[1].Trim(),
                        EstadoDestino = transicionDatos[2].Trim()
                    });
                }

                ImprimirAutomata(Automata);
                ConsultarCadena(Automata);
                return Automata;
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error: No se puede leer el archivo {filePath}. {ex.Message}");
                return null;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Error: Formato de los datos en el archivo {filePath} no es válido. {ex.Message}");
                return null;
            }

        }

        //Funcion para leer archivos .csv
        public static AutomataEntity LeerArchivoCsv(string filePath, string fileExtension)
        {
            try
            {
                Console.WriteLine("\nArchivo de extensión " + fileExtension + "\n");

                //Lee todas las líneas del archivo
                List<string> lineasCsv = File.ReadAllLines(filePath).ToList();

                //Valida que el archivo tenga al menos las primeras 3 líneas necesarias
                if (lineasCsv.Count < 3)
                {
                    Console.WriteLine($"Error: El archivo {filePath} no tiene el formato esperado.");
                    return null;
                }

                AutomataEntity Automata = new AutomataEntity();
                {
                    Automata.Estados = lineasCsv[0].Trim('"');
                    Automata.EstadoInicial = lineasCsv[1].Trim('"').Split(',').Select(est => est.Trim()).ToArray();
                    Automata.EstadosFinales = lineasCsv[2].Trim('"').Split(',').Select(est => est.Trim()).ToArray();
                    Automata.Transiciones = new List<TransicionEntity>();
                };

                //Asigna las transiciones a la instancia del autómata
                for (int i = 3; i < lineasCsv.Count; i++)
                {
                    string[] transicionDatos = lineasCsv[i].Split(',').Select(dato => dato.Trim()).ToArray();

                    if (transicionDatos.Length != 3)
                    {
                        Console.WriteLine($"Error: El formato de la transición en la línea {i + 1} del archivo {filePath} no es válido.");
                        continue;
                    }

                    Automata.Transiciones.Add(new TransicionEntity
                    {
                        EstadoOrigen = transicionDatos[0].Trim('"'),
                        Simbolo = transicionDatos[1].Trim('"'),
                        EstadoDestino = transicionDatos[2].Trim('"')
                    });
                }
                ImprimirAutomata(Automata);
                ConsultarCadena(Automata);
                Console.Clear();
                return Automata;
            }
            //Valida la lectura del archivo
            catch (IOException ex)
            {
                Console.WriteLine($"Error: No se puede leer el archivo {filePath}. {ex.Message}");
                return null;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Error: Formato de los datos en el archivo {filePath} no es válido. {ex.Message}");
                return null;
            }
        }

        //Funcion para lleer archivo .json
        public static AutomataEntity LeerArchivoJson(string trimmedPath, string flieExtension)
        {
            try
            {
                Console.WriteLine("\nArchivo de extensión " + flieExtension + "\n");

                string jsonData;
                AutomataEntity Automata = null;

                //Lee el archivo JSON
                using (StreamReader leerJson = new StreamReader(trimmedPath))
                {
                    jsonData = leerJson.ReadToEnd();

                    //Intentar deserializar el JSON a una instancia de AutomataEntity
                    try
                    {
                        Automata = JsonConvert.DeserializeObject<AutomataEntity>(jsonData);
                    }
                    catch (JsonException)
                    {
                        Automata = new AutomataEntity();
                        Automata = JsonConvert.DeserializeObject<AutomataEntity>(jsonData, new JsonSerializerSettings
                        {
                            MissingMemberHandling = MissingMemberHandling.Error
                        });
                    }
                    //Imprimir el autómata y consultar la cadena
                    if (Automata != null)
                    {
                        ImprimirAutomata(Automata);
                        ConsultarCadena(Automata);
                        return Automata;
                    }
                    else
                    {
                        Console.WriteLine($"Error: No se pudo deserializar el archivo {trimmedPath}");
                    }
                }
            }

            //Valida la lectura del archivo
            catch (IOException ex)
            {
                Console.WriteLine($"Error: No se puede leer el archivo {trimmedPath}. {ex.Message}");
            }

            //Valida el formato json
            catch (JsonException ex)
            {
                Console.WriteLine($"Error: El archivo {trimmedPath} contiene un .json inválido.");
                MostrarEstructuraJson();
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        //Funcion para mostrar la estructura JSON esperada
        static void MostrarEstructuraJson()
        {
            string jsonString = @"
        {
            ""estados"": 6,
            ""estadoInicial"": [""0""],
            ""estadosFinales"": [""4""],
            ""transiciones"": [
                {
                    ""estadoOrigen"": ""0"",
                    ""simbolo"": ""0"",
                    ""estadoDestino"": ""E""
                },
                {
                    ""estadoOrigen"": ""0"",
                    ""simbolo"": ""1"",
                    ""estadoDestino"": ""1""
                },
                {
                    ""estadoOrigen"": ""1"",
                    ""simbolo"": ""0"",
                    ""estadoDestino"": ""2""
                }
            ]
        }";
            Console.WriteLine("El .json debe seguir la siguiente estructura:");
            Console.WriteLine(jsonString);
        }

        //Funcion para imprimir los detalles del autómata
        static void ImprimirAutomata(AutomataEntity automata)
        {
            try
            {
                //Comprueba la lectura de los archivos 
                Console.WriteLine($"Automata: ");
                Console.WriteLine($"Estados: {automata.Estados}");
                Console.WriteLine($"Estados iniciales: {string.Join(", ", automata.EstadoInicial)}");
                Console.WriteLine($"Estados finales: {string.Join(", ", automata.EstadosFinales)}");

                foreach (var transicion in automata.Transiciones)
                {
                    Console.WriteLine($"{transicion.EstadoOrigen}, {transicion.Simbolo}, {transicion.EstadoDestino}");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al imprimir automata: {ex.Message}");
            }
        }

        //Funcion para consultar y validar una cadena en el autómata
        static void ConsultarCadena(AutomataEntity automata)
        {
            try
            {
                Console.WriteLine("\n\nCONSULTA DE CADENA: ");
                Console.WriteLine("Ingrese su cadena de caracteres:");
                cadena = Console.ReadLine();

                //Verifica que la cadena no esté vacía
                if (!string.IsNullOrEmpty(cadena))
                {
                    Console.WriteLine("\nRecorrido:");
                    RecorrerAF(automata.EstadoInicial[0], cadena, 0, automata);
                }
                else
                {
                    Console.WriteLine("El string está vacío.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al mostrar recorrido: {ex.Message}");
            }
        }

        //Funcion recursiva para recorrer el autómata con una cadena
        static void RecorrerAF(string estActual, string cadena, int contador, AutomataEntity automata)
        {
            string[] caracteres = cadena.ToCharArray().Select(c => c.ToString()).ToArray(); //separa la cadena ingresada en un arreglo string

            if (contador == caracteres.Length)
            {
                VerifEstFinal(estActual, automata.EstadosFinales, automata);
            }
            else
            {
                string sigEstado = SigEstado(caracteres[contador], estActual, automata);
                ImprimirPaso(estActual, caracteres[contador], sigEstado, contador, cadena, automata);
            }
        }

        //Funcion para obtener el siguiente estado en el autómata
        static string SigEstado(string caracter, string estado, AutomataEntity automata)
        {
            foreach (var transicion in automata.Transiciones)//recorre toda la matriz
            {
                if (estado.Equals(transicion.EstadoOrigen))//se compara estado recibido con la primera posicion de cada fila(estado actual)
                {
                    if (caracter.Equals(transicion.Simbolo))//Si coincide se compara con la segunda posición el cual es "la letra"
                    {
                        return transicion.EstadoDestino;//Si ambas coinciden se retorna el siguiente estado
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

        //Funcion para imprimir el paso actual del recorrido
        static void ImprimirPaso(string estActual, string caracter, string sigEstado, int contador, string cadena, AutomataEntity automata)
        {
            int longitudCad = cadena.Length;
            if (contador < longitudCad)
            {
                Console.WriteLine(estActual + " -> " + caracter + " -> " + sigEstado);
                contador++;//aumenta el contador para seguir recorriendo la cadena
                estActual = sigEstado;//siguiente estado pasa a ser el actual para volver a llamar RecorrerAF

                RecorrerAF(estActual, cadena, contador, automata);
            }
        }

        //Funcion para verificar si el estado actual es final o no
        static void VerifEstFinal(string estActual, string[] EstadosFinales, AutomataEntity automata)
        {
            //Limpiar el estado actual
            string estAcualLimpio = estActual.Trim().ToUpper();

            //Limpiar los estados finales
            string[] estadosFinalesLimpios = EstadosFinales.Select(ef => ef.Trim().ToUpper()).ToArray();

            bool esEstadoFinal = estadosFinalesLimpios.Contains(estAcualLimpio);

            if (esEstadoFinal)
            {
                Console.WriteLine("El recorrido dirige al estado '" + estActual + "' que si es un estado final, la palabra SI es aceptable.");
                ContinuarValidando(automata);
            }
            else if (estActual == "E")
            {
                Console.WriteLine("El recorrido dirige a Epsilon. \nEl estado '" + estActual + "' no está permitido, la palabra NO es aceptable.");
                ContinuarValidando(automata);
            }
            else
            {
                Console.WriteLine("El recorrido dirige al estado '" + estActual + "' que no es un estado final, la palabra NO es aceptable.");
                ContinuarValidando(automata);
            }
        }

        //Funcion para continuar validando cadenas o volver al menu
        static void ContinuarValidando(AutomataEntity automata)
        {
            string respuesta;

            do
            {
                Console.WriteLine($"\nDesea seguir validando cadenas? (SI o NO):");
                respuesta = Console.ReadLine();

                if (respuesta.ToUpper() == "SI")
                {
                    ConsultarCadena(automata);
                    break;
                }
                else if (respuesta.ToUpper() == "NO")
                {
                    Console.Clear();
                    MostrarMenu();
                    break;
                }
                else
                {
                    Console.Write($"\nEntrada no válida. Por favor ingrese 'SI' O ''NO'");
                }

            } while (respuesta != "SI" && respuesta != "NO");
        }

        static void MostrarMenu()
        {
            try
            {
                bool op = true;

                while (op == true)
                {
                    Console.Clear();

                    Console.WriteLine("1. Automata finito determinista");
                    Console.WriteLine("2. Automata finito no determinista");
                    Console.WriteLine("3. Salir");
                    Console.WriteLine("Digite su opcción: ");

                    string opcion = Console.ReadLine();

                    switch (opcion)
                    {
                        case "1":
                            Console.Clear();
                            MostrarOpcion1();
                            break;

                        case "2":
                            MostrarOpcion2();
                            break;

                        case "3":
                            Environment.Exit(0); //Sale del programa
                            break;

                        default:
                            MostrarOpcionInvalida();
                            break;

                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error con el menú: {ex.Message}");
            }
        }

        static void MostrarOpcion1()
        {
            bool archivoLeido = false;
            try
            {
                if (!archivoLeido)
                {
                    LeerArchivos();
                    archivoLeido = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al leer archivo: {ex.Message}");
            }
            finally
            {
                MostrarMenu();
            }
        }

        static void MostrarOpcion2()
        {
            Console.WriteLine("!Estamos trabajando para agregar esta función! ;)");
            MostrarRegresarMenu();
        }

        static void MostrarOpcionInvalida()
        {
            Console.WriteLine("Opción inválida.");
            MostrarRegresarMenu();
        }

        static void MostrarRegresarMenu()
        {
            Console.WriteLine("\nPresione cualquier tecla para regresar al menú...");
            Console.ReadKey();
            MostrarMenu();
        }

        static void MostrarBienvenida()
        {
            Console.WriteLine("******************************************");
            Console.WriteLine("*   BIENVENIDO/A AL PROGRAMA QUE LEE Y   *");
            Console.WriteLine("*        VALIDA AUTÓMATAS FINITOS        *");
            Console.WriteLine("*                                        *");
            Console.WriteLine("*   Este programa realiza operaciones    *");
            Console.WriteLine("*   relacionadas con autómatas finitos   *");
            Console.WriteLine("*        y cadenas de caracteres.        *");
            Console.WriteLine("******************************************");
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            MostrarBienvenida();
            MostrarMenu();
        }
    }
}
