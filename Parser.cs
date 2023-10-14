namespace hulk
{
    class Parser
    {
        public List<Token> tokens;
        int position;
        bool Is_let_variable = false;

        public Parser(string codeline)
        {
            Lexer toLexer = new(codeline);
            //Devuelve la lista de tokens ya escaneadas (poseen el tipo de token y su valor)
            this.tokens = toLexer.Scan(); ;
        }

        //Devuelve el token que se encuentra en la posición siguiente
        private Token GetNextToken()
        {
            var current = tokens[position];
            position++;
            return current;
        }

        //Devuelve el token que se encuentra en la posición anterior
        private Token GetPreviuosToken()
        {
            var previous = tokens[position - 1];
            return previous;
        }
        //Devuelve los argumentos de la función 
        private List<Token> GetArguments()
        {
            List<Token> arguments = new();
            //Si no posee argumentos igual se devuelven
            if (tokens[position].Types == Token.TypesOfToken.CloseParenthesis_Token) return arguments;

            //Mientras no exista ',' seguirá añadiendo valores siempre y cuando sea una ecuación que se pueda evaluar, parará siempre si hay un ')'
            do
            {
                if (tokens[position].Types == Token.TypesOfToken.ID)
                {
                    arguments.Add(tokens[position]);
                    position++;
                }
                else throw new Error.Syntax_Error(tokens[position + 1].Value, 'i');
            } while (Match(Token.TypesOfToken.Separator_Token));
            return arguments;
        }

        //Verifica que el token sea del tipo requirido para realizar la operación y devuelve el próximo token
        private bool Match(params Token.TypesOfToken[] types)
        {
            if (position != tokens.Count())
            {
                foreach (var item in types)
                {
                    if (item == tokens[position].Types)
                    {
                        GetNextToken();
                        return true;
                    }
                }
            }
            return false;
        }

        //Se comienza a crear el árbol
        public Expression Parse()
        {
            //Siempre que sea posible y debe terminar en ';'
            try
            {
                Expression expression = SuperiorExperssion();
                if (Match(Token.TypesOfToken.EndLine_Token)) return expression;
                else throw new Error.Syntax_Error(null!, ';');
            }
            catch (System.Exception)
            {
                throw new Error.Syntax_Error(null!, 'n');
            }
        }

        //Function 
        private Expression SuperiorExperssion()
        {
            if (tokens[position].Value == "function")
            {
                position++;
                //Si no sigue un ID lanzan un error en caso contrario guarda el nombre del ID y lo inicializa en el diccionario "variablesInFunction" del scope
                if (!Match(Token.TypesOfToken.ID)) throw new Error.Syntax_Error(tokens[position].Value, 'f');
                Token id = GetPreviuosToken();
                Aplication.scope.Search(new FunctionCallExpression(id, null!, null!));
                //Si no le sigue un '(' lanza un error, en caso contrario guarda los argumentos de la función
                if (!Match(Token.TypesOfToken.OpenParenthesis_Token)) throw new Error.Syntax_Error(tokens[position].Value, '(');
                List<Token> arguments = GetArguments();
                //Si no le sigue un ')' lanza un error
                if (!Match(Token.TypesOfToken.CloseParenthesis_Token)) throw new Error.Syntax_Error(tokens[position].Value, ')');
                //Si no le sigue un '=>' lanza un error, encaso contrario guarda el cuerpo de la función
                if (!Match(Token.TypesOfToken.Arrow_Token)) throw new Error.Syntax_Error(tokens[position].Value, 'a');
                Expression body = ToDeclare();
                //Se revisa que ya no exista una función con el mimsmo nombre y la misma cantidad de argumentos
                FunctionCallExpression final = new(id, arguments, body);
                Aplication.scope.Search(final);
                //Devuelve nulo porque no evalua la función
                return null!;
            }
            return ToDeclare();
        }

        //Let_In 
        private Expression ToDeclare()
        {
            if (tokens[position].Value == "let")
            {
                position++;
                //Crea una lista con las variables y sus valores
                List<AssigmentExpression> assigments = Assigment();
                //Si no le sigue 'in' lanza un error, en caso contrario guarda el valor de la expresión que va después del 'in'
                if (!Match(Token.TypesOfToken.KeyWord_Token) && tokens[position - 1].Value != "in") throw new Error.Syntax_Error(tokens[position - 1].Value, 'd');
                Expression _in = ToDeclare();
                return new LetInExpression(assigments, _in);
            }
            return Conditional();
        }

        //Devuelve una lista que contiene las variables iniciadas en Let_In
        private List<AssigmentExpression> Assigment()
        {
            List<AssigmentExpression> assigments = new();
            //Mientras no exista ',' seguirá añadiendo valores siempre y cuando sea una ecuación que se pueda evaluar
            do
            {
                //Parará si no es una variable
                if (!Match(Token.TypesOfToken.ID))
                {
                    if (Match(Token.TypesOfToken.Constant_Token)) throw new Error.Syntax_Error(tokens[position - 1].Value, 'l');
                    throw new Error.Syntax_Error(tokens[position - 1].Value, 'c');
                }
                Token arguments = GetPreviuosToken();
                //Parará si no le sigue el signo '=' a la declaración de variables, en caso contrario guardará el valor asignado a esta
                if (!Match(Token.TypesOfToken.Equal_Token)) throw new Error.Syntax_Error(tokens[position - 1].Value, '=');
                Expression body = ToDeclare();
                assigments.Add(new AssigmentExpression(arguments, body));
                //Se vuelve true para poder usarse mas adelante
                Is_let_variable = true;
            } while (Match(Token.TypesOfToken.Separator_Token));
            return assigments;
        }

        //If_ELse
        private Expression Conditional()
        {
            if (tokens[position].Value == "if")
            {
                position++;
                //Si no le sigue un '(' lanza un error
                if (!Match(Token.TypesOfToken.OpenParenthesis_Token)) throw new Error.Syntax_Error(tokens[position - 1].Value, '(');
                //Si le sigue un ')' lanza un error porque la condición no puede estar vacía, en caso contrario guarda el valor de la condición
                if (Match(Token.TypesOfToken.CloseParenthesis_Token)) throw new Error.Syntax_Error(null!, 'j');
                Expression condition = ToDeclare();
                //Si no le sigue un ')' lanza un error
                if (!Match(Token.TypesOfToken.CloseParenthesis_Token)) throw new Error.Syntax_Error(tokens[position - 1].Value, ')');
                //Si le sigue "else" lanza un error debido a que no existe instrucciones, en caso contrario guarda el valor de la instrucción después del if 
                if (tokens[position - 1].Value == "else") throw new Error.Syntax_Error(null!, 's');
                Expression if_ = ToDeclare();
                //Si no le sigue "else" lanza un error en caso contrario guarda el valor de la inc¿strucción después del else
                if (tokens[position].Value != "else") throw new Error.Syntax_Error(tokens[position - 1].Value, 'p');
                position++;
                try
                {
                    Expression _else = ToDeclare();
                    return new IfElseExpression(if_, condition, _else);
                }
                catch (System.Exception)
                {
                    throw new Error.Syntax_Error(null!, 'k');
                }

            }
            return Logical();
        }


        // '|' '&'
        private Expression Logical()
        {
            Expression expression = Equal();
            //Mientras sea '|' o '&' continuará el ciclo 
            while (Match(Token.TypesOfToken.And_Token, Token.TypesOfToken.Or_Token))
            {
                Token oper = GetPreviuosToken();
                Expression right = Equal();
                expression = new BinaryExpression(expression, oper, right);
            }
            return expression;
        }

        // '==' '!='
        private Expression Equal()
        {
            Expression expression = Compare();
            //Este contador verificará que no exista más de un operador de igualdad o desigualdad en la misma expresión
            int temp = 0;
            //Mientras sea '==' o '!=' continuará el ciclo 
            while (Match(Token.TypesOfToken.DoubleEqual_Token, Token.TypesOfToken.NoEqual_Token))
            {
                Token oper = GetPreviuosToken();
                if (temp >= 1) throw new Error.Syntax_Error(oper.Value, 'b');
                Expression right = Compare();
                expression = new BinaryExpression(expression, oper, right);
                temp++;
            }
            return expression;
        }

        // '<' '<=' '>' '>='
        private Expression Compare()
        {
            Expression expression = Term();
            //Este contador verificará que no exista más de un operador de comparación
            int temp = 0;
            //Mientras sea un operador de comparación continuará el ciclo 
            while (Match(Token.TypesOfToken.More_Token, Token.TypesOfToken.MoreOrEqual_Token, Token.TypesOfToken.Less_Token, Token.TypesOfToken.LessOrEqual_Token))
            {
                Token oper = GetPreviuosToken();
                if (temp >= 1) throw new Error.Syntax_Error(oper.Value, 'i');
                Expression right = Term();
                expression = new BinaryExpression(expression, oper, right);
                temp++;
            }
            return expression;
        }

        //'+' '-' '@'
        private Expression Term()
        {
            Expression expression = Factor();
            //Mientras sea '+' , '-' o '@' continuará el ciclo 
            while (Match(Token.TypesOfToken.Minus_Token, Token.TypesOfToken.Plus_Token, Token.TypesOfToken.Concatenation_Token))
            {
                Token oper = GetPreviuosToken();
                Expression right = Factor();
                expression = new BinaryExpression(expression, oper, right);
            }
            return expression;
        }

        // '/' '*' '%'
        private Expression Factor()
        {
            Expression expression = Power();
            //Mientras sea '/' , '*' o '%' continuará el ciclo 
            while (Match(Token.TypesOfToken.Mult_Token, Token.TypesOfToken.Div_Token, Token.TypesOfToken.Modu_Token))
            {
                Token oper = GetPreviuosToken();
                Expression right = Power();
                expression = new BinaryExpression(expression, oper, right);
            }
            return expression;
        }

        // '^' 
        private Expression Power()
        {
            Expression expression = Unary();
            //Mientras sea '^' continuará el ciclo 
            while (Match(Token.TypesOfToken.Pow_Token))
            {
                Token oper = GetPreviuosToken();
                Expression right = Unary();
                expression = new BinaryExpression(expression, oper, right);
            }
            return expression;
        }

        //'!' '-' '+' '(' "print"
        private Expression Unary()
        {
            //Si es '!' '-' '+' continuará 
            if (Match(Token.TypesOfToken.Not_Token, Token.TypesOfToken.Minus_Token, Token.TypesOfToken.Plus_Token))
            {
                Token oper = GetPreviuosToken();
                return new UnaryExpression(oper, Unary());
            }

            //Si es '(' continuará y guardará el valor de la expresión en su interior
            if (Match(Token.TypesOfToken.OpenParenthesis_Token))
            {
                Expression expression = ToDeclare();
                //Si no es ')' lanza un error, asi evitamos que los paréntesis no estén balanceados 
                if (!Match(Token.TypesOfToken.CloseParenthesis_Token)) throw new Error.Syntax_Error(tokens[position - 1].Value, ')');
                return expression;
            }
            
            //Si es "print"
            if (tokens[position].Value == "print")
            {
                position++;
                //Si no es '(' lanza un error
                if (!Match(Token.TypesOfToken.OpenParenthesis_Token)) throw new Error.Syntax_Error(tokens[position - 1].Value, '(');
                PrintExpression expression = new(tokens[position - 1], ToDeclare());
                //Si no es ')' lanza un error 
                if (!Match(Token.TypesOfToken.CloseParenthesis_Token)) throw new Error.Syntax_Error(tokens[position - 1].Value, ')');
                return expression;
            }
            return MathFunction();
        }

        // "log" "sin" "cos" "sqrt" "expo" "rand"
        private Expression MathFunction()
        {
            //Si es una función matemática o la evaluación de euler continua y devuelve el operador
            if (Match(Token.TypesOfToken.MathFunction_Token, Token.TypesOfToken.Expo_Token))
            {
                Token oper = GetPreviuosToken();
                //Si no le sigue un '(' lanza un error, en caso contrario crea una expresión unaria a partir del operador
                if (!Match(Token.TypesOfToken.OpenParenthesis_Token)) throw new Error.Syntax_Error(tokens[position - 1].Value, '(');
                //Si es rand entonces el siguiente token deberá ser el paréntesis cerrado, en caso contrario lanza un error
                if (oper.Value == "rand")
                {
                    if (!Match(Token.TypesOfToken.CloseParenthesis_Token)) throw new Error.Syntax_Error(tokens[position - 1].Value, ')');
                    UnaryExpression expression1 = new(oper, null!);
                    return expression1;
                }
                UnaryExpression expression = new(oper, ToDeclare());
                //Si no le sigue un ')' lanza un error, en caso contrario retorna la expresión
                if (!Match(Token.TypesOfToken.CloseParenthesis_Token)) throw new Error.Syntax_Error(tokens[position - 1].Value, ')');
                return expression;
            }

            //Si es un log continúa y devuelve el operador
            if (Match(Token.TypesOfToken.Log_Token))
            {
                Token operlog = GetPreviuosToken();
                //Si no le sigue un '(' lanza un error, en caso contrario guarda el valor del argumento del logaritmo
                if (!Match(Token.TypesOfToken.OpenParenthesis_Token)) throw new Error.Syntax_Error(tokens[position - 1].Value, '(');
                Expression value = ToDeclare();
                //Si no le sigue un ')' lanza un error, en caso contrario guarda el valor de la base
                if (!Match(Token.TypesOfToken.Separator_Token)) throw new Error.Syntax_Error(tokens[position - 1].Value, ',');
                Expression Base = ToDeclare();
                Expression expression = new LogExpression(operlog, value, Base);
                position++;
                return expression;
            }
            return Literal();
        }

        //Números  cadenas de strings  true  false  PI  E
        private Expression Literal()
        {
            //Si es un número
            if (Match(Token.TypesOfToken.Number)) return new NumberExpression(double.Parse(tokens[position - 1].Value));
            //Si es un string
            if (Match(Token.TypesOfToken.Chain)) return new StringExpression(tokens[position - 1].Value);
            //Si es true
            if (Match(Token.TypesOfToken.True_Token)) return new BooleanExpression(true);
            //Si es false
            if (Match(Token.TypesOfToken.False_Token)) return new BooleanExpression(false);
            //Si es PI
            if (tokens[position].Value == "PI")
            {
                GetNextToken();
                return new ConstantExpression(tokens[position - 1].Value);
            }
            //Si es E
            if (tokens[position].Value == "E")
            {
                GetNextToken();
                return new ConstantExpression(tokens[position - 1].Value);
            }
            //Si es una variable
            if (Match(Token.TypesOfToken.ID))
            {
                //Si la variable pertenece a Let_In y no le sigue un paréntesis abierto se entenderá que es una variable
                //Es útil por si se declara una variable con el mismo nombre de una función
                if (Is_let_variable == true)
                {
                    if (!Match(Token.TypesOfToken.OpenParenthesis_Token)) return new IdExpression(tokens[position - 1], null!);
                    position--;
                }
                //Si se encuentra entre las funciones declaradas
                if (Aplication.scope.variablesInFunction.ContainsKey(tokens[position - 1].Value))
                {
                    Token functionName = tokens[position - 1];
                    //Si no le sigue un '(' lanza un error, en caso contrario agrega el valor de los argumentos 
                    if (!Match(Token.TypesOfToken.OpenParenthesis_Token)) throw new Error.Syntax_Error(tokens[position - 1].Value, '(');
                    List<Expression> arguments = Arguments();
                    //Si no le sigue un ')' lanza un error
                    if (!Match(Token.TypesOfToken.CloseParenthesis_Token)) throw new Error.Syntax_Error(tokens[position].Value, ')');
                    return new FunctionExpression(functionName, arguments, null!);
                }
                //Si le sigue un parentesis
                if (Match(Token.TypesOfToken.OpenParenthesis_Token)) throw new Error.Syntax_Error(tokens[position - 2].Value, 'g');
                return new IdExpression(tokens[position - 1], null!);
            }
            throw new Error.Syntax_Error(null!, 'n');
        }

        //Obtiene los argumentos necesarios para la evaluación de la expresión
        private List<Expression> Arguments()
        {
            List<Expression> arguments = new();
            //Si es un paréntesis cerrado retorna
            if (tokens[position].Types == Token.TypesOfToken.CloseParenthesis_Token) return arguments;
            //Continuará el ciclo siempre que después del argumento siga una coma y mientras después de la coma el token inmediato no sea un ')'
            do
            {
                if (tokens[position].Types != Token.TypesOfToken.CloseParenthesis_Token)
                {
                    Expression argument = ToDeclare();
                    arguments.Add(argument);
                }
                else throw new Error.Syntax_Error(tokens[position + 1].Value, 'i');
            } while (Match(Token.TypesOfToken.Separator_Token));
            return arguments;
        }
    }
}