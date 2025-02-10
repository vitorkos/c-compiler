using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

namespace C
{
    public class CSemanticExprListener : CBaseListener
    {
        public Dictionary<string, IParseTree> Functions { get; protected set; } = new Dictionary<string, IParseTree>();
        public HashSet<string> Variables { get; private set; } = new HashSet<string>();
        public bool HasErrors { get; private set; } = false;
        public List<string> ErrorMessages { get; private set; } = new List<string>();

        // main
        public override void ExitMainFunction([NotNull] CParser.MainFunctionContext context)
        {
            if (context == null)
            {
                HasErrors = true;
                ErrorMessages.Add("Main function context is null.");
                return;
            }

         
        }

        // var
        public override void ExitVariableDeclaration([NotNull] CParser.VariableDeclarationContext context)
        {
            if (context == null)
            {
                HasErrors = true;
                ErrorMessages.Add("Variable declaration context is null.");
                return;
            }

            var variableName = context.GetChild<ITerminalNode>(0).GetText();
            if (Variables.Contains(variableName))
            {
                HasErrors = true;
                ErrorMessages.Add($"Variable {variableName} has already been declared.");
            }
            else
            {
                Variables.Add(variableName);
            }
        }

        // func decl
        public override void ExitFunctionDeclaration([NotNull] CParser.FunctionDeclarationContext context)
        {
            if (context == null)
            {
                HasErrors = true;
                ErrorMessages.Add("Function declaration context is null.");
                return;
            }

            var functionName = context.IDENTIFIER().GetText();
            if (Functions.ContainsKey(functionName))
            {
                HasErrors = true;
                ErrorMessages.Add($"Function {functionName} has already been declared.");
            }
            else
            {
                Functions[functionName] = context;
            }
        }

        // func
        public override void ExitChamadaStatement([NotNull] CParser.ChamadaStatementContext context)
        {
            if (context == null)
            {
                HasErrors = true;
                ErrorMessages.Add("Function call context is null.");
                return;
            }

            var functionName = context.IDENTIFIER().GetText();
            if (!Functions.ContainsKey(functionName))
            {
                HasErrors = true;
                ErrorMessages.Add($"Function {functionName} has not been declared.");
                return;
            }

            var functionContext = Functions[functionName] as CParser.FunctionDeclarationContext;
            var parameterList = functionContext?.parameterList();
            var arguments = context.expression();

            if (parameterList != null && parameterList.parameter().Length != arguments.Length)
            {
                HasErrors = true;
                ErrorMessages.Add($"Function {functionName} called with incorrect number of arguments.");
            }
        }

        // return
        public override void ExitReturnStatement([NotNull] CParser.ReturnStatementContext context)
        {
            if (context == null)
            {
                HasErrors = true;
                ErrorMessages.Add("Return statement context is null.");
                return;
            }

            var expression = context.expression();
            if (expression == null)
            {
                HasErrors = true;
                ErrorMessages.Add("Return statement does not have an expression.");
            }
        }

        // if
        public override void ExitIfStatement([NotNull] CParser.IfStatementContext context)
        {
            if (context == null)
            {
                HasErrors = true;
                ErrorMessages.Add("If statement context is null.");
                return;
            }

            var condition = context.expression();
            if (condition == null)
            {
                HasErrors = true;
                ErrorMessages.Add("If statement does not have a condition.");
            }
        }

        // w
        public override void ExitWhileStatement([NotNull] CParser.WhileStatementContext context)
        {
            if (context == null)
            {
                HasErrors = true;
                ErrorMessages.Add("While statement context is null.");
                return;
            }

            var condition = context.expression();
            if (condition == null)
            {
                HasErrors = true;
                ErrorMessages.Add("While statement does not have a condition.");
            }
        }

        // 4
        public override void ExitForStatement([NotNull] CParser.ForStatementContext context)
        {
            if (context == null)
            {
                HasErrors = true;
                ErrorMessages.Add("For statement context is null.");
                return;
            }

            var init = context.expression(0);
            var condition = context.expression(1);
            var update = context.expression(2);

            if (init == null || condition == null || update == null)
            {
                HasErrors = true;
                ErrorMessages.Add("For statement does not have valid expressions.");
            }
        }

        public override void ExitPrintfStatement([NotNull] CParser.PrintfStatementContext context)
        {
            if (context == null)
            {
                HasErrors = true;
                ErrorMessages.Add("Printf statement context is null.");
                return;
            }

            var expression = context.expression();
            if (expression == null)
            {
                HasErrors = true;
                ErrorMessages.Add("Printf statement does not have an expression.");
            }
        }

        public override void ExitScanfStatement([NotNull] CParser.ScanfStatementContext context)
        {
            if (context == null)
            {
                HasErrors = true;
                ErrorMessages.Add("Scanf statement context is null.");
                return;
            }

            var variable = context.IDENTIFIER();
            if (variable == null)
            {
                HasErrors = true;
                ErrorMessages.Add("Scanf statement does not have a variable.");
            }
        }
    }
}