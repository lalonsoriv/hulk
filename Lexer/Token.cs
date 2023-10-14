namespace hulk
{
    public class Token
    {
        public enum TypesOfToken     // En este enum se encuentran los posibles símbolos que pueden escibirse en el compilador
        {
            WhiteSpace_Token,            //' '
            Enter_Token,                //'\r'
            KeyWord_Token,               //'let' 'in' 'print' 'if' 'else' 
            True_Token,                  //'true'
            False_Token,                 //'false'
            Function_Token,              //'function'
            Apostrophe_Token,            //'\"'
            Arrow_Token,                 //'=>'
            Plus_Token,                  //'+'
            Minus_Token,                 //'-'
            Mult_Token,                  //'*'
            Div_Token,                   //'/'
            Dot_Token,                   //'.'
            Equal_Token,                 //'='
            NoEqual_Token,               //'!='
            DoubleEqual_Token,           //'=='
            Pow_Token,                   //'^'
            Expo_Token,                  //'exp'
            Modu_Token,                  //'%'
            Not_Token,                   //'!'
            And_Token,                   //'&'
            Or_Token,                    //'|'
            Less_Token,                  //'<'
            More_Token,                  //'>'
            LessOrEqual_Token,           //'<='
            MoreOrEqual_Token,           //'>='
            OpenParenthesis_Token,       //'('
            CloseParenthesis_Token,      //')'
            EndLine_Token,               //';'
            MathFunction_Token,          // 'sen' 'cos' 'sqrt' 'rand'
            Log_Token,                   //'log'
            Constant_Token,              //'PI' 'E'
            Concatenation_Token,         //'@'
            Separator_Token,             //','
            ID,                          //identifier (variable)
            Number,                      //número
            Chain,                       //cadena de string

        }

        public TypesOfToken Types { get; }
        public string Value { get; }

        public Token(TypesOfToken types, string value)
        {
            Types = types;
            Value = value;
        }

        //Diccionario que posee como llave un string y como valor el tipo de token que es
        public static Dictionary<string, Token> Tokens = new()
        {
            {" ", new Token(TypesOfToken.WhiteSpace_Token, " ")},
            {"let", new Token(TypesOfToken.KeyWord_Token, "let")},
            {"in", new Token(TypesOfToken.KeyWord_Token, "in")},
            {"print", new Token(TypesOfToken.KeyWord_Token, "print")},
            {"if", new Token(TypesOfToken.KeyWord_Token, "if")},
            {"else", new Token(TypesOfToken.KeyWord_Token, "else")},
            {"true", new Token(TypesOfToken.True_Token, "true")},
            {"false", new Token(TypesOfToken.False_Token, "false")},
            {"function", new Token(TypesOfToken.Function_Token, "function")},
            {"\"", new Token(TypesOfToken.Apostrophe_Token, "\"")},
            {"\r", new Token(TypesOfToken.Enter_Token, "\r")},
            {"=>", new Token(TypesOfToken.Arrow_Token, "=>")},
            {"+", new Token(TypesOfToken.Plus_Token, "+")},
            {"-", new Token(TypesOfToken.Minus_Token, "-")},
            {"*", new Token(TypesOfToken.Mult_Token, "*")},
            {"/", new Token(TypesOfToken.Div_Token, "/")},
            {".", new Token(TypesOfToken.Dot_Token, ".")},
            {"=", new Token(TypesOfToken.Equal_Token, "=")},
            {"!=", new Token(TypesOfToken.NoEqual_Token, "!=")},
            {"==", new Token(TypesOfToken.DoubleEqual_Token, "==")},
            {"^", new Token(TypesOfToken.Pow_Token, "^")},
            {"%", new Token(TypesOfToken.Modu_Token, "%")},
            {"!", new Token(TypesOfToken.Not_Token, "!")},
            {"&", new Token(TypesOfToken.And_Token, "&")},
            {"|", new Token(TypesOfToken.Or_Token, "|")},
            {"<", new Token(TypesOfToken.Less_Token, "<")},
            {">", new Token(TypesOfToken.More_Token, ">")},
            {"<=", new Token(TypesOfToken.LessOrEqual_Token, "<=")},
            {">=", new Token(TypesOfToken.MoreOrEqual_Token, ">=")},
            {"(", new Token(TypesOfToken.OpenParenthesis_Token, "(")},
            {")", new Token(TypesOfToken.CloseParenthesis_Token, ")")},
            {";", new Token(TypesOfToken.EndLine_Token, ";")},
            {"log", new Token(TypesOfToken.Log_Token, "log")},
            {"sin", new Token(TypesOfToken.MathFunction_Token, "sin")},
            {"cos", new Token(TypesOfToken.MathFunction_Token, "cos")},
            {"rand", new Token(TypesOfToken.MathFunction_Token, "rand")},
            {"sqrt", new Token(TypesOfToken.MathFunction_Token, "sqrt")},
            {"exp", new Token(TypesOfToken.Expo_Token, "exp")},
            {"PI", new Token(TypesOfToken.Constant_Token, "PI")},
            {"E", new Token(TypesOfToken.Constant_Token, "E")},
            {"@", new Token(TypesOfToken.Concatenation_Token, "@")},
            {",", new Token(TypesOfToken.Separator_Token, ",")},
        };
    }
}