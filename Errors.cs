namespace hulk
{
    public class Error : Exception
    {
        public class Syntax_Error : Error
        {
            public string message { get; set; }
            public Syntax_Error(object message1, char missed)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                this.message = missed switch
                {
                    '(' => "Olvidó poner un \"(\" después de " + message1.ToString(),
                    ')' => "Olvidó poner un \")\" después de " + message1.ToString(),
                    ',' => "Después de " + message1.ToString() + " debería de ir \",\"",
                    ';' => "Debería haber terminado la expresión con \";\"",
                    '=' => "Después de " + message1.ToString() + " debería de ir \"=\"",
                    'a' => "Se esperaba \"=>\" en lugar de " + message1.ToString() + " despues de declarar la función",
                    'b' => "No es posible utilizar " + message1.ToString() + " con más de una condición",
                    'c' => "Se esperaba una asignación de variable en lugar de " + message1.ToString(),
                    'd' => "Se esperaba \"in\" en lugar de " + message1.ToString(),
                    'e' => "No es posible utilizar " + message1.ToString() + " porque es una constante del lenguaje a la que no se le puede asignar un valor",
                    'f' => "Después de \"function\" debería ir la declaración de una función en lugar de " + message1.ToString(),
                    'g' => "La función " + "\"" + message1.ToString() + "\"" + " no ha sido declarada",
                    'h' => "No es posible realizar esta operación pues la variable " + "\"" + message1.ToString() + "\"" + " no se encuentra en el contexto actual",
                    'i' => "Se esperaba la declaración de un argumento para la función en lugar de " + message1.ToString(),
                    'j' => "No es posible realizar la operación debido a que \"if\" no acepta condiciones vacías",
                    'k' => "No se encontró condición después de \"else\"",
                    'l' => "Después de \"let\" debería ir variable en lugar de " + message1.ToString(),
                    'o' => "No puede emplear la palabra " + "\"" + message1.ToString() + "\"" + " para declarar una función debido a que ya existe una función con este nombre y la misma cantidad de argumentos",
                    'n' => "No es posible evaluar esta expresión",
                    'p' => "Se esperaba \"else\" en lugar de " + message1.ToString(),
                    'r' => "No puede emplear la palabra " + "\"" + message1.ToString() + "\"" + " para declarar una función debido a que es una palabra reservada",
                    's' => "No se encontró condición después de \"if\"",
                    'v' => "La variable " + message1.ToString() + " es usada pero no se ha inicializado",
                    _ => "",
                };
                Aplication.Error("ERROR DE SINTAXIS: " + message);
            }
        }

        public class Semantic_Error : Error
        {
            public string message { get; set; }

            public Semantic_Error(object message1, object message2, Token oper, char operation)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                this.message = operation switch
                {
                    'b' => "No es posible con " + message1.ToString() + " y " + message2.ToString() + " realizar la operación " + oper.Value.ToString(),
                    'c' => "No es posible convertir " + message1.ToString() + " a bool",
                    'n' => "No es posible evaluar esta expresión",
                    's' => "Ha ocurrido un Stack Overflow al llamar a la función " + message1.ToString(),
                    'u' => "No es posible realizar la operación " + oper.Value.ToString() + " con la expresión " + message1.ToString(),
                    _ => "",
                };
                Aplication.Error("ERROR SEMÁNTICO: " + message);
            }
        }

        public class Lexical_Error : Error
        {
            public string message { get; set; }
            public Lexical_Error(string position)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                this.message = "No es posible realizar esta operación, por favor revise su entrada en la posición " + position;
                Aplication.Error("ERROR LÉXICO: " + message);

            }
        }
    }
}
