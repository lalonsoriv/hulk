namespace hulk
{
    public class Scope
    {
        //Diccionario que contiene como llave el nombre de la variable y como valor una lista en la q se guardan los distintos valores de la variable (util en Let_In)
        public Dictionary<string, List<object>> variable = new();
        //Diccionario que contiene como llave una variable y como argumneto otro diccionario del cual su llave es la cantidad de variables y su argumento es una expressión de tipo FuctionCallExpression
        public Dictionary<string, Dictionary<int, FunctionCallExpression>> variablesInFunction = new();

        public Scope()
        {
            variable = new();
            variablesInFunction = new();
        }

        //Devuelve el valor que le corresponde a la variable escogiendo el último valor que se le dió
        public object Get(Token id)
        {
            try
            {
                return variable[id.Value].Last();
            }
            //Si la variable nunca fue declarada se imprime un error
            catch (System.Exception)
            {
                throw new Error.Syntax_Error(id.Value, 'h');
            }
        }
        //Inicializa la variable con su valor
        public void Set(Token id, object value)
        {
            //Si no se contiene la variable se inicializa
            if (!variable.ContainsKey(id.Value)) variable.Add(id.Value, new List<object>());
            //En caso de que se contenga se añade el nuevo valor           
            variable[id.Value].Add(value);
        }

        //Busca la función y revisa sus parámetros
        public void Search(FunctionCallExpression function)
        {
            string functionName = function.functionName.Value;
            int variables = 0;
            //Si no posee argumentos la cantidad de variables será -1
            if (function.arguments == null!) variables = -1;
            else
            {
                //De lo contrario se inicializa la cantidad de variables
                variables = function.arguments.Count();
            }

            //Si ya existe esta función 
            if (variablesInFunction.ContainsKey(functionName))
            {
                Dictionary<int, FunctionCallExpression> cantvariables = variablesInFunction[functionName];
                //Si continene la misma cantidad de variables se entenderá como un error porque se estará tratando de sobreescribir la función
                if (cantvariables.ContainsKey(variables)) throw new Error.Syntax_Error(functionName, 'o');
                //La cantidad de variables solo será -1 cuando la función posea argumentos vacíos por lo que eliminará los valores del diccionario
                if (variablesInFunction[functionName].ContainsKey(-1)) variablesInFunction[functionName].Remove(-1);
                //Se le agregan los argumentos y el cuerpo
                variablesInFunction[functionName].Add(variables, function);
            }
            //De no existir se crea y se le añaden los valores
            else
            {
                Dictionary<int, FunctionCallExpression> cantvariables = new()
                {
                    { variables, function }
                };
                variablesInFunction.Add(functionName, cantvariables);
            }
        }

        //Devuelve las variables de la función
        public List<Token> GetArguments(string functionName, int cantvariables)
        {
            return variablesInFunction[functionName][cantvariables].arguments;
        }

        //Devuelve el cuerpo de la función
        public Expression GetBody(string functionName, int cantvariables)
        {
            return variablesInFunction[functionName][cantvariables].body;
        }

        //Elimina el ultimo valor q se le intrudujo a la variable 
        public void Delete(Token id)
        {
            variable[id.Value].RemoveAt(variable[id.Value].Count() - 1);
            //En caso de no existir mas valores se elimina la variable
            if (variable[id.Value].Count() == 0) variable.Remove(id.Value);
        }
    }
}