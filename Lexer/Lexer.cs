namespace hulk
{
    public class Lexer
    {
        public string codeline;
        public Lexer(string codeline)
        {
            this.codeline = codeline;
        }
        public class Words
        {
            public int finalPosition;
            public string words = "";
        }
        public class Numbers
        {
            public int finalPosition;
            public string number = "";
        }

        public List<Token> Scan()
        {
            List<Token> tokenized = new();
            string temp = "";

            //Recorre cada carácter 
            for (int i = 0; i < codeline.Length; i++)
            {
                //En caso de ser comillas obterner el comentario
                if (codeline[i] == '\"')
                {
                    try
                    {
                        Words comment = GetString(codeline, i + 1);
                        //Añade el comentario como un único token a la lista tokenized
                        tokenized.Add(new Token(Token.TypesOfToken.Chain, comment.words));
                        //Mueve la siguiente posición que se va a verificar hacia el último lugar del comentario
                        i = comment.finalPosition;
                        continue;
                    }
                    catch (System.Exception)
                    {
                        throw new Error.Lexical_Error((i + 1).ToString());
                    }
                }

                //Si es un espacio en blanco o una tabulación continuará a la próxima posición de existir
                if (codeline[i] == ' ' || codeline[i] == '\t') continue;

                //Si es un ';' lo añade a la lista y continúa
                if (codeline[i] == ';')
                {
                    tokenized.Add(new Token(Token.TypesOfToken.EndLine_Token, codeline[i].ToString()));
                    continue;
                }

                //Si es un número lo añade a la lista y se mueve a la siguiente posición
                if (char.IsDigit(codeline[i]))
                {
                    try
                    {
                        Numbers number = GetNumber(codeline, i);
                        tokenized.Add(new Token(Token.TypesOfToken.Number, number.number));
                        i = number.finalPosition;
                        continue;
                    }
                    catch (System.Exception)
                    {
                        throw new Error.Lexical_Error((i + 1).ToString());
                    }
                }

                //Si es un palabra lo añade a la lista y se mueve a la siguiente posición
                if (char.IsLetter(codeline[i]) || codeline[i] == '_')
                {
                    try
                    {
                        Words terms = Variables(codeline, i);
                        temp = terms.words;
                        tokenized.Add(GetToken(temp));
                        i = terms.finalPosition;
                        temp = "";
                        continue;
                    }
                    catch (System.Exception)
                    {
                        throw new Error.Lexical_Error((i + 1).ToString());
                    }
                }

                //Si es un operador lo añade a la lista y se mueve a la siguinte posición
                if (!char.IsLetterOrDigit(codeline[i]))
                {
                    try
                    {
                        Token token = Operators(codeline, i);
                        tokenized.Add(token);
                        if (token.Value.Length == 2) i += 1;
                        continue;
                    }
                    catch (System.Exception)
                    {
                        throw new Error.Lexical_Error((i + 1).ToString());
                    }
                }
            }
            return tokenized;
        }

        //Devuelve el string que se encuetra entre comillas
        private static Words GetString(string codeline, int position)
        {
            Words result = new();
            string temp = "";

            //Hasta que encuentre un " para el cierre del comentario si no hay " de cierre es un error
            while (codeline[position] != '\"')
            {
                //Si esta \" es pq se quiere poner entre comillas la palabra
                if (codeline[position] == '\\' && codeline[position + 1] == '\"')
                {
                    position++;
                }
                temp += codeline[position];
                position++;
                if (position == codeline.Count()) throw new Error.Lexical_Error(position.ToString());
            }
            result.words = temp;
            //Guarda la posición en que terminó el ciclo para así continuar revisando la entrada del usuario
            result.finalPosition = position;
            return result;
        }

        //Devuelve un número
        private static Numbers GetNumber(string codeline, int position)
        {
            Numbers result = new();
            string temp = codeline[position].ToString();
            position++;
            bool Is_dot = false;

            for (int i = position; i < codeline.Length; i++)
            {
                //Si es una letra lanza un error 
                if (char.IsLetter(codeline[i])) throw new Error.Lexical_Error(i.ToString());
                //Si ya tenía un punto y se agregó otro lanza un error
                if (codeline[i] == '.' && Is_dot == true) throw new Error.Lexical_Error(i.ToString());
                //Si es decimal se declara verdadero Is_dot
                if (codeline[i] == '.') Is_dot = true;
                //Si no es un número o un punto sale del ciclo 
                if (!char.IsDigit(codeline[i]) && codeline[i] != '.') break;
                temp += codeline[i];
                position++;
            }
            result.number = temp;
            //Guarda la posición en que terminó el ciclo para así continuar revisando la entrada del usuario
            result.finalPosition = position - 1;
            return result;
        }

        //Devuelve el tipo de token que es si se encuentra guardado en el diccionario, en caso de no encontrarlo se asumirá que es una variable 
        private static Token GetToken(string codeline)
        {
            if (!Token.Tokens.ContainsKey(codeline)) return new Token(Token.TypesOfToken.ID, codeline);
            return Token.Tokens[codeline];
        }

        //Devuelve el tipo de operador que es
        private static Token Operators(string codeline, int position)
        {
            string temp = codeline[position].ToString();
            //Se buscará si es un operador con más de un caracter
            if (position + 1 != codeline.Length && !char.IsLetterOrDigit(codeline[position + 1]))
            {
                if (codeline[position] == '=' && (codeline[position + 1] == '=' || codeline[position + 1] == '>')) temp += codeline[position + 1];
                if (codeline[position] == '!' && codeline[position + 1] == '=') temp += codeline[position + 1];
                if (codeline[position] == '<' && codeline[position + 1] == '=') temp += codeline[position + 1];
                if (codeline[position] == '>' && codeline[position + 1] == '=') temp += codeline[position + 1];
            }
            //Si el caracter no esta contenido en el diccionario de token lanza un error 
            if (!Token.Tokens.ContainsKey(temp)) throw new Error.Lexical_Error((position + 1).ToString());
            return GetToken(temp);
        }

        //Devuelve el nombre de las variables
        private static Words Variables(string codeline, int position)
        {
            Words result = new();
            string temp = "";
            try
            {
                while (char.IsLetterOrDigit(codeline[position]) || codeline[position] == '_')
                {
                    temp += codeline[position];
                    position++;
                }
            }
            catch (System.Exception)
            {
                throw new Error.Lexical_Error(position.ToString());
            }
            result.words = temp;
            //Guarda la posición en que terminó el ciclo para así continuar revisando la entrada del usuario 
            result.finalPosition = position - 1;
            return result;
        }
    }
}
