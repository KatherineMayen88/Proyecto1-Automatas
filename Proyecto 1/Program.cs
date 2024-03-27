using System;

class Program
{

    string cantEstados = "4";
    string estInicial = "0";
    public string cadena;
    //int[] estFinales = new int[2]; se usa cuando tengamos varios estados finales
    string estFinal = "0";
    string[,] transiciones = { { "0", "1", "1" }, { "0", "0", "2" }, { "1", "1", "0" }, {"1","0","3" }, { "2", "1", "3" }, { "2", "0", "0" }, { "3", "1", "2" }, { "3","0","1"} };

    static void Main(string[] args)
    {
        var Program = new Program();    
        Console.WriteLine("Ingrese su cadena de caracteres:");
        Program.cadena = Console.ReadLine();
        
        //verifica que la cadena no esté vacía
        if (!string.IsNullOrEmpty(Program.cadena))
        {
            Console.WriteLine("Recorrido:");
            RecorrerAF(Program.estInicial,Program.cadena,0);
        }
        else
        {
            Console.WriteLine("El string está vacío.");
        }
    }

    static void RecorrerAF(string estActual,string cadena, int contador)
    {
        var Program = new Program();
        string[] caracteres = cadena.ToCharArray().Select(c => c.ToString()).ToArray(); //separa la cadena ingresada en un arreglo string

        if (contador == caracteres.Length)
        {
            VerifEstFinal(estActual);
        }
        else
        {
            string sigEstado = Program.SigEstado(caracteres[contador], estActual);
            ImprimirPaso(estActual, caracteres[contador], sigEstado, contador, cadena);
        }
    }

    string SigEstado(string caracter, string estado)
    {
        for (int i = 0; i < transiciones.GetLength(0); i++)//recorre toda la matriz
        {
            if (estado.Equals(transiciones[i,0]) )//se compara estado recibido con la primera posicion de cada fila(estado actual)
            {
                if (caracter.Equals(transiciones[i, 1]))//Si coincide se compara con la segunda posición el cual es "la letra"
                {
                   return transiciones[i, 2];//Si ambas coinciden se retorna el siguiente estado
                }
            }
        }
        return "no se encontró";
        
    }

    static void ImprimirPaso(string estActual,string caracter, string sigEstado,int contador,string cadena)
    {
        var Program = new Program();
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
        var Program = new Program();

        if (estActual.Equals(Program.estFinal))
        {
            Console.WriteLine("Palabra aceptada :D");
        }
        else
        {
            Console.WriteLine("Palabra no aceptada D:");
        }
    }

}
