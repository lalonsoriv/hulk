namespace hulk
{
    //Operaciones binarias
    public class BinaryExpression : Expression
    {
        public Expression Left { get; }
        public Token Operator { get; }
        public Expression Right { get; }

        public BinaryExpression(Expression left, Token oper, Expression right)
        {
            Left = left;
            Operator = oper;
            Right = right;
        }

        public override T EvaluateExpressions<T>(IHelper<T> helper)
        {
            return helper.BinaryExpression(this);
        }

    }
}