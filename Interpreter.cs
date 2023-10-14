namespace hulk
{
    class Interpreter : IHelper<object>
    {
        public Interpreter()
        {
        }
        //Cantidad de llamadas que se realizan en la función
        private int cantCall = 0;
        //Cantidad máxima de llamadas que se pueden realizar para evitar el Stack Overflows
        private int maxCantCall = 1000;

        //Evalúa la expresión siempre que sea posible
        public string ToInterpret(Expression expression)
        {
            cantCall = 0;
            try
            {
                return Evaluate(expression).ToString()!;
            }
            catch (System.Exception)
            {
                throw new Error.Semantic_Error(null!, null!, null!, 'n');
            }
        }

        //Si la expresión es nula devuelve un espacio en blanco y pedirá la siguiente línea
        private object Evaluate(Expression expression)
        {
            if (expression == null) return "";
            return expression.EvaluateExpressions(this);
        }

        //Evalúa las expresiones de tipo if_else
        public object IfElseExpression(IfElseExpression expression)
        {
            //Evalúa la condición y si es verdadeera devuelve la expresión que le sigue, en caso contrario devuelve la que se encuentra despues del else
            try
            {
                if ((bool)Evaluate(expression.condition)) return Evaluate(expression.if_);
                return Evaluate(expression._else);
            }
            catch (System.Exception)
            {
                throw new Error.Semantic_Error(expression.condition, "", null!, 'c');
            }
        }

        //Evalúa las expresiones de tipo let_in
        public object LetInExpression(LetInExpression expression)
        {
            //Recorre todas las variables e inicializa los valores
            foreach (var item in expression.let_)
            {
                Aplication.scope.Set(item.id, Evaluate(item.assigment));
            }
            //Guarda los valores de la expresión que se encuentra después del in
            object _in_value = Evaluate(expression._in);
            //Se elimina el valor de la variable para evitar que se sobreescriba cuando se evalúan
            foreach (var item in expression.let_)
            {
                Aplication.scope.Delete(item.id);
            }
            return _in_value;
        }

        //Inicializa los valores de las expresiones de tipo assigment
        public object AssigmentExpression(AssigmentExpression expression)
        {
            Aplication.scope.Set(expression.id, expression.assigment);
            return null!;
        }

        //Evalúa las expresiones de tipo FunctionCall
        public object FunctionCallExpression(FunctionCallExpression expression)
        {
            Aplication.scope.Search(expression);
            return null!;
        }

        //Evalúa las expresiones de tipo print
        public object PrintExpression(PrintExpression expression)
        {
            Console.WriteLine(Evaluate(expression.expression));
            //La retorna en caso de que con el valor de la expresion lego se realice otra operacion
            return Evaluate(expression.expression);
        }

        //Evalúa las expresiones de tipo log
        public object LogExpression(LogExpression expression)
        {
            //Evalua la base y el valor
            object bas = Evaluate(expression.Base);
            object value = Evaluate(expression.Value);
            try
            {
                return Math.Log((double)value, (double)bas);
            }
            catch (System.Exception)
            {
                throw new Error.Semantic_Error(value, bas, expression.Log, 'b');
            }
        }

        //Evalúa las expresiones de tipo binaria
        public object BinaryExpression(BinaryExpression expression)
        {
            //Primero evalúa el lado izquierdo, luego el lado derecho y después realiza la operación requerida
            object left = Evaluate(expression.Left);
            object right = Evaluate(expression.Right);

            //Realizará la operación deseada atendiendo a su operador
            if (expression.Operator.Types == Token.TypesOfToken.Plus_Token)
            {
                try
                {
                    return (double)left + (double)right;
                }
                catch (System.Exception)
                {
                    throw new Error.Semantic_Error(left, right, expression.Operator, 'b');
                }

            }
            if (expression.Operator.Types == Token.TypesOfToken.Minus_Token)
            {
                try
                {
                    return (double)left - (double)right;
                }
                catch (System.Exception)
                {
                    throw new Error.Semantic_Error(left, right, expression.Operator, 'b');
                }

            }
            if (expression.Operator.Types == Token.TypesOfToken.Mult_Token)
            {
                try
                {
                    return (double)left * (double)right;
                }
                catch (System.Exception)
                {
                    throw new Error.Semantic_Error(left, right, expression.Operator, 'b');
                }

            }
            if (expression.Operator.Types == Token.TypesOfToken.Div_Token)
            {
                try
                {
                    return (double)left / (double)right;
                }
                catch (System.Exception)
                {
                    throw new Error.Semantic_Error(left, right, expression.Operator, 'b');
                }

            }
            if (expression.Operator.Types == Token.TypesOfToken.Modu_Token)
            {
                try
                {
                    return (double)left % (double)right;
                }
                catch (System.Exception)
                {
                    throw new Error.Semantic_Error(left, right, expression.Operator, 'b');
                }

            }
            if (expression.Operator.Types == Token.TypesOfToken.Concatenation_Token)
            {
                if (left is string v && right is string v1) return v + v1;

                if (left is string v2) return v2 + right.ToString();

                if (right is string v3) return left.ToString() + v3;

                else throw new Error.Semantic_Error(left, right, expression.Operator, 'b');
            }
            if (expression.Operator.Types == Token.TypesOfToken.Pow_Token)
            {
                try
                {
                    return Math.Pow((double)left, (double)right);
                }
                catch (System.Exception)
                {
                    throw new Error.Semantic_Error(left, right, expression.Operator, 'b');
                }

            }
            if (expression.Operator.Types == Token.TypesOfToken.DoubleEqual_Token)
            {
                try
                {
                    if (Equals(left, right)) return true;
                    else return false;
                }
                catch (System.Exception)
                {
                    throw new Error.Semantic_Error(left, right, expression.Operator, 'b');
                }

            }
            if (expression.Operator.Types == Token.TypesOfToken.MoreOrEqual_Token)
            {
                try
                {
                    if ((double)left >= (double)right) return true;
                    else return false;
                }
                catch (System.Exception)
                {
                    throw new Error.Semantic_Error(left, right, expression.Operator, 'b');
                }

            }
            if (expression.Operator.Types == Token.TypesOfToken.LessOrEqual_Token)
            {
                try
                {
                    if ((double)left <= (double)right) return true;
                    else return false;
                }
                catch (System.Exception)
                {
                    throw new Error.Semantic_Error(left, right, expression.Operator, 'b');
                }

            }
            if (expression.Operator.Types == Token.TypesOfToken.NoEqual_Token)
            {
                try
                {
                    if (!Equals(left, right)) return true;
                    else return false;
                }
                catch (System.Exception)
                {
                    throw new Error.Semantic_Error(left, right, expression.Operator, 'b');
                }

            }
            if (expression.Operator.Types == Token.TypesOfToken.Less_Token)
            {
                try
                {
                    if ((double)left < (double)right) return true;
                    else return false;
                }
                catch (System.Exception)
                {
                    throw new Error.Semantic_Error(left, right, expression.Operator, 'b');
                }

            }
            if (expression.Operator.Types == Token.TypesOfToken.More_Token)
            {
                try
                {
                    if ((double)left > (double)right) return true;
                    else return false;
                }
                catch (System.Exception)
                {
                    throw new Error.Semantic_Error(left, right, expression.Operator, 'b');
                }

            }
            if (expression.Operator.Types == Token.TypesOfToken.Or_Token)
            {
                try
                {
                    return (bool)left || (bool)right;
                }
                catch (System.Exception)
                {
                    throw new Error.Semantic_Error(left, right, expression.Operator, 'b');
                }

            }
            if (expression.Operator.Types == Token.TypesOfToken.And_Token)
            {
                try
                {
                    return (bool)left && (bool)right;
                }
                catch (System.Exception)
                {
                    throw new Error.Semantic_Error(left, right, expression.Operator, 'b');
                }

            }
            return null!;
        }

        //Evalúa las expresiones de tipo unaria
        public object UnaryExpression(UnaryExpression expression)
        {
            //Evalúa lo que está dentro de la operación
            object toOper = Evaluate(expression.ToOperate);

            //Realizará la operación deseada atendiendo a su operador
            if (expression.Operator.Value == "sqrt")
            {
                try
                {
                    return Math.Sqrt((double)toOper);
                }
                catch (System.Exception)
                {
                    throw new Error.Semantic_Error(toOper, "", expression.Operator, 'u');
                }
            }
            if (expression.Operator.Value == "sin")
            {
                try
                {
                    return Math.Sin((double)toOper);
                }
                catch (System.Exception)
                {
                    throw new Error.Semantic_Error(toOper, "", expression.Operator, 'u');
                }
            }
            if (expression.Operator.Value == "cos")
            {
                try
                {
                    return Math.Cos((double)toOper);
                }
                catch (System.Exception)
                {
                    throw new Error.Semantic_Error(toOper, "", expression.Operator, 'u');
                }
            }
            if (expression.Operator.Value == "rand")
            {
                Random random = new();
                return random.NextDouble();
            }
            if (expression.Operator.Types == Token.TypesOfToken.Expo_Token)
            {
                try
                {
                    return Math.Pow(Math.E, (double)toOper);
                }
                catch (System.Exception)
                {
                    throw new Error.Semantic_Error(toOper, "", expression.Operator, 'u');
                }
            }
            if (expression.Operator.Types == Token.TypesOfToken.Not_Token)
            {
                try
                {
                    return !(bool)toOper;
                }
                catch (System.Exception)
                {
                    throw new Error.Semantic_Error(toOper, "", expression.Operator, 'u');
                }
            }
            if (expression.Operator.Types == Token.TypesOfToken.Minus_Token)
            {
                try
                {
                    return -(double)toOper;
                }
                catch (System.Exception)
                {
                    throw new Error.Semantic_Error(toOper, "", expression.Operator, 'u');
                }
            }
            if (expression.Operator.Types == Token.TypesOfToken.Plus_Token) return toOper;
            return null!;
        }

        //Evalúa las expresiones de tipo string
        public object StringExpression(StringExpression expression)
        {
            return expression.value;
        }

        //Devuelve los valores de las expresiones de tipo id
        public object IdExpression(IdExpression expression)
        {
            return Aplication.scope.Get(expression.ID);
        }

        //Evalúa las expresiones de tipo boolean
        public object BooleanExpression(BooleanExpression expression)
        {
            return expression.value;
        }

        //Evalúa las expresiones de tipo numerico
        public object NumberExpression(NumberExpression expression)
        {
            return expression.value;
        }

        //Evalúa las expresiones de tipo constante
        public object ConstantExpression(ConstantExpression expression)
        {
            if (expression.value == "PI") return (double)Math.PI;
            if (expression.value == "E") return (double)Math.E;
            return null!;
        }

        //Evalúa las expresiones de tipo function
        public object FunctionExpression(FunctionExpression expression)
        {
            //Toma los argumentos que corresponden según el nombre de la función y la cantidad de argumentos que posee
            List<Token> scope_argumnets = Aplication.scope.GetArguments(expression.functionName.Value, expression.arguments.Count);
            List<AssigmentExpression> variables = new();

            //Se agregara cada variable con su valor
            for (int i = 0; i < expression.arguments.Count; i++)
            {
                variables.Add(new AssigmentExpression(scope_argumnets[i], expression.arguments[i]));
            }

            //De acuerdo con la varibale obtiene el cuerpo de la expresión y después evalúa siempre que no ocurra un llamado a la función muy grande como para que ocurra un Stack Overflow
            LetInExpression function = new(variables, Aplication.scope.GetBody(expression.functionName.Value, expression.arguments.Count));
            if (cantCall > maxCantCall) throw new Error.Semantic_Error(expression.functionName.Value, null!, null!, 's');
            cantCall++;
            object temp = Evaluate(function);
            cantCall--;
            return temp;
        }
    }

}
