using Antlr4.Runtime;

namespace C
{
    public class CErrorListener : BaseErrorListener, IAntlrErrorListener<int>
    {
        public bool HasErrors { get; private set; } = false;
        public List<string> ErrorMessages { get; private set; } = new List<string>();

        public override void SyntaxError(TextWriter output, 
                                         IRecognizer recognizer, IToken offendingSymbol, 
                                         int line, int charPositionInLine, 
                                         string msg, RecognitionException e)
        {
            ReportSyntaxError(output, line, charPositionInLine, msg);
        }

        public void SyntaxError(TextWriter output, 
                                IRecognizer recognizer, int offendingSymbol, 
                                int line, int charPositionInLine, 
                                string msg, RecognitionException e)
        {
            ReportSyntaxError(output, line, charPositionInLine, msg);
        }

        private void ReportSyntaxError(TextWriter output, int line, int charPositionInLine, string msg)
        {
            HasErrors = true;
            string errorMessage = $"Syntax error on line {line}, position {charPositionInLine}: {msg}";
            ErrorMessages.Add(errorMessage);
            output.WriteLine(errorMessage);
        }
    }
}