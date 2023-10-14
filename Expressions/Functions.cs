namespace hulk
{
    //Expresiones logarítmicas
    public class LogExpression : Expression
    {
        public Token Log;
        public Expression Value;
        public Expression Base { get; }
        public LogExpression(Token Log, Expression Value, Expression Base)
        {
            this.Log = Log;
            this.Value = Value;
            this.Base = Base;
        }

        public override T EvaluateExpressions<T>(IHelper<T> helper)
        {
            return helper.LogExpression(this);
        }
    }

    //Expresión a evaluar dentro de "print"
    public class PrintExpression : Expression
    {
        public Token print;
        public Expression expression;

        public PrintExpression(Token print, Expression expression)
        {
            this.print = print;
            this.expression = expression;
        }
        public override T EvaluateExpressions<T>(IHelper<T> helper)
        {
            return helper.PrintExpression(this);
        }
    }

    //Declaración de funciones
    public class FunctionCallExpression : Expression
    {
        public Token functionName;
        public List<Token> arguments;
        public Expression body;

        public FunctionCallExpression(Token functionName, List<Token> arguments, Expression body)
        {
            this.functionName = functionName;
            this.arguments = arguments;
            this.body = body;
        }
        public override T EvaluateExpressions<T>(IHelper<T> helper)
        {
            return helper.FunctionCallExpression(this);
        }
    }

    //Función declarada 
    public class FunctionExpression : Expression
    {
        public Token functionName;
        public List<Expression> arguments;
        public Expression body;

        public FunctionExpression(Token functionName, List<Expression> arguments, Expression body)
        {
            this.functionName = functionName;
            this.arguments = arguments;
            this.body = body;
        }
        public override T EvaluateExpressions<T>(IHelper<T> helper)
        {
            return helper.FunctionExpression(this);
        }
    }

    //Asignación de valores
    public class AssigmentExpression : Expression
    {
        public Token id;
        public Expression assigment;
        public AssigmentExpression(Token id, Expression assigment)
        {
            this.id = id;
            this.assigment = assigment;
        }
        public override T EvaluateExpressions<T>(IHelper<T> helper)
        {
            return helper.AssigmentExpression(this);
        }
    }

    //Creación de variables
    public class LetInExpression : Expression
    {
        public List<AssigmentExpression> let_;
        public Expression _in;

        public LetInExpression(List<AssigmentExpression> let_, Expression _in)
        {
            this.let_ = let_;
            this._in = _in;
        }
        public override T EvaluateExpressions<T>(IHelper<T> helper)
        {
            return helper.LetInExpression(this);
        }
    }

    //Condicionales
    public class IfElseExpression : Expression
    {
        public Expression if_;
        public Expression condition;
        public Expression _else;

        public IfElseExpression(Expression if_, Expression condition, Expression _else)
        {
            this.if_ = if_;
            this.condition = condition;
            this._else = _else;
        }
        public override T EvaluateExpressions<T>(IHelper<T> helper)
        {
            return helper.IfElseExpression(this);
        }
    }
}