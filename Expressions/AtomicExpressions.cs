namespace hulk
{
    //Expresiones puramente numericas
    public class NumberExpression : Expression
    {
        public double value;
        public NumberExpression(double value)
        {
            this.value = value;
        }
        public override T EvaluateExpressions<T>(IHelper<T> helper)
        {
            return helper.NumberExpression(this);
        }
    }

    //Expresiones booleanas
    public class BooleanExpression : Expression
    {
        public bool value;
        public BooleanExpression(bool value)
        {
            this.value = value;
        }
        public override T EvaluateExpressions<T>(IHelper<T> helper)
        {
            return helper.BooleanExpression(this);
        }
    }

    //Variables
    public class IdExpression : Expression
    {
        public Token ID;
        public Expression value;

        public IdExpression(Token ID, Expression value)
        {
            this.ID = ID;
            this.value = value;
        }
        public override T EvaluateExpressions<T>(IHelper<T> helper)
        {
            return helper.IdExpression(this);
        }
    }

    //Palabras 
    public class StringExpression : Expression
    {
        public string value;
        public StringExpression(string value)
        {
            this.value = value;
        }
        public override T EvaluateExpressions<T>(IHelper<T> helper)
        {
            return helper.StringExpression(this);
        }
    }

    //Constantes
    public class ConstantExpression : Expression
    {
        public string value;
        public ConstantExpression(string value)
        {
            this.value = value;
        }
        public override T EvaluateExpressions<T>(IHelper<T> helper)
        {
            return helper.ConstantExpression(this);
        }
    }
}