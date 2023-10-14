namespace hulk
{
    public class Aplication
    {
        public static Scope scope = new();
        static Interpreter toInterpreter = new();
        public static void Initialize()
        {
            System.Console.ForegroundColor = ConsoleColor.DarkMagenta;
            System.Console.BackgroundColor = ConsoleColor.Black;
            System.Console.WriteLine("##Sea bienvenido a esta nueva versión de la aplicación Hulk");
            System.Console.WriteLine("##Si necesita ayuda para trabajar con el programa presione h");
            System.Console.WriteLine("##En caso de conocer el programa y si desea comenzar presione s");
            System.Console.Write("->");
            string option = Console.ReadLine()!.ToLower();
            if (option == "h")
            {
                Help();
            }
            else if (option == "s")
            {

                System.Console.WriteLine("##Por favor introduzca su código en la siguiente línea...");
                //Hasta que se presione Ctr + c continuara el ciclo
                do
                {
                    Start();
                } while (true);
            }
            else System.Console.WriteLine("##Esta no es una opción, intente otra vez");
        }

        //Imprime una lista de las operaciones que puede realizar con la aplicacion 
        private static void Help()
        {
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine("\t Con este programa usted podrá: \n \t Declarar funciones si sigue la siguiente estructura: \n \t --function id(<argumentos>) => <...> \n \t Establecer condiciones si sigue la siguiente estructura: \n \t --if(<condicion>) <...> else <...> \n \t Declarar variables si sigue la siguiente estructura: \n \t --let <argumentos> in <...> \n \t Imprimir valores específicos si sigue la siguiente estructura: \n \t --print(<...>) \n \t Resolver funciones matemáticas usando: \n \t --sin(<ángulo>) Devuelve seno de un ángulo en radianes \n \t --cos(<ángulo>) Devuelve coseno de un ángulo en radianes \n \t --sqrt(<valor>) Devuelve raíz cuadrada de un valor \n \t --exp(<valor>) Devuelve euler elevado a un valor \n \t --rand() Devuelve número aleatorio entre 0 y 1 \n \t --log(<base>, <valor>) Devuelve  logaritmo de un valor en la base dada \n \t Realizar operaciones de: \n \t --Suma '+'\n \t --Resta '-'\n \t --Multiplicación '*'\n \t --División '/'\n \t --Elevación '^'\n \t --Módulo '%'\n \t Realizar comparaciones \n \t --Mayor '>'\n \t --Menor '<'\n \t --Mayor que >=\n \t --Menor que <=\n \t --Igual a ==\n \t --Diferente de !=");
            System.Console.ForegroundColor = ConsoleColor.DarkMagenta;
            System.Console.WriteLine("Si desea volver a la pantalla de inicio presione i");
            string option = System.Console.ReadLine()!;
            if (option == "i") Initialize();
            System.Console.WriteLine("##Esta no es una opción, intente otra vez");
        }

        //Comienza el progama
        static void Start()
        {
            System.Console.Write("->");
            string line = System.Console.ReadLine()!;
            //Si la entrada del usuario es enter, espacio en blanco o tabulación imprimirá una línea en blanco
            if (line == "" || line.Length == 1 && (line == " " || line == "\t"))
                System.Console.WriteLine(" ");
            else
            {
                Parser parser = new(line);
                Expression expression = parser.Parse();
                //Devuelve la evaluación de la expresión si existe
                System.Console.WriteLine(toInterpreter.ToInterpret(expression!));
            }
        }

        //Imprime los errores en caso de existir
        public static void Error(string message)
        {
            System.Console.WriteLine(message);
            System.Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Environment.Exit(0);
        }

        public static void Main(string[] args)
        {
            Initialize();
        }
    }
}