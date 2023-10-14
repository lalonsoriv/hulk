namespace hulk
{
    //Clase que contiene como método la evaluación de la expresión
    public abstract class Expression
    {
        public abstract T EvaluateExpressions<T>(IHelper<T> helper);
    }


    //Interfaz creada para que al evaluar una expressión la clase que implemente la interfaz establezca el método correcto de acuerdo con el tipo de la expresión 
    public interface IHelper<T>
    {
        T IfElseExpression(IfElseExpression expression);
        T LetInExpression(LetInExpression expression);
        T AssigmentExpression(AssigmentExpression expression);
        T FunctionCallExpression(FunctionCallExpression expression);
        T PrintExpression(PrintExpression expression);
        T LogExpression(LogExpression expression);
        T BinaryExpression(BinaryExpression expression);
        T UnaryExpression(UnaryExpression expression);
        T StringExpression(StringExpression expression);
        T IdExpression(IdExpression expression);
        T BooleanExpression(BooleanExpression expression);
        T NumberExpression(NumberExpression expression);
        T ConstantExpression(ConstantExpression expression);
        T FunctionExpression(FunctionExpression expression);
    }
}