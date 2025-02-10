using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using C;

class Program
{
    static void Main(string[] args)
    {
        string filePath = args[0];
        string input = File.ReadAllText(filePath);

        AntlrInputStream inputStream = new AntlrInputStream(input);
        CLexer lexer = new CLexer(inputStream);
        CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
        CParser parser = new CParser(commonTokenStream);

        CErrorListener errorListener = new CErrorListener();
        lexer.RemoveErrorListeners();
        lexer.AddErrorListener(errorListener);
        parser.RemoveErrorListeners();
        parser.AddErrorListener(errorListener);

        IParseTree tree = parser.program();

        //verifica erros
        if (errorListener.HasErrors)
        {
            Console.WriteLine("Syntax errors found: ");
            foreach (var errorMessage in errorListener.ErrorMessages)
            {
                Console.WriteLine(errorMessage);
            }
            return;
        }

        CSemanticExprListener semanticListener = new CSemanticExprListener();
        ParseTreeWalker walker = new ParseTreeWalker();
        walker.Walk(semanticListener, tree);

        if (semanticListener.HasErrors)
        {
            Console.WriteLine("Semantic errors found: ");
            foreach (var errorMessage in semanticListener.ErrorMessages)
            {
                Console.WriteLine(errorMessage);
            }
            return;
        }  

        CVisitorImpl visitor = new CVisitorImpl();
        visitor.Visit(tree);
    }
}