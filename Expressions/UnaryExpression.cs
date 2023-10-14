namespace hulk
{
    //Operaciones unarias
    public class UnaryExpression : Expression
    {
        public Token Operator { get; }
        public Expression ToOperate { get; }

        public UnaryExpression(Token oper, Expression toOper)
        {
            Operator = oper;
            ToOperate = toOper;
        }

        public override T EvaluateExpressions<T>(IHelper<T> helper)
        {
            return helper.UnaryExpression(this);
        }
    }
}
