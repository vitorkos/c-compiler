namespace C
{
    public class CVisitorImpl : CBaseVisitor<object?>
    {
        // dic p var
        private Dictionary<string, object?> memory = new Dictionary<string, object?>();

        // dic p func
        private Dictionary<string, CParser.FunctionDeclarationContext> functions = new Dictionary<string, CParser.FunctionDeclarationContext>();

        // Visitando declarações de funções
        public override object? VisitFunctionDeclaration(CParser.FunctionDeclarationContext context)
        {
            string functionName = context.IDENTIFIER().GetText();
            functions[functionName] = context;
            return null;
        }

        public override object? VisitChamadaStatement(CParser.ChamadaStatementContext context)
        {
            string functionName = context.IDENTIFIER().GetText();

            if (!functions.ContainsKey(functionName))
            {
                throw new Exception($"Error: Function '{functionName}' undeclared.");
            }

            CParser.FunctionDeclarationContext functionContext = functions[functionName];
            CParser.ParameterListContext? parameterList = functionContext.parameterList();
            var arguments = context.expression();

            if (parameterList != null && parameterList.parameter().Length != arguments.Length)
            {
                throw new Exception($"Error: Incorrect number of arguments for function'{functionName}'.");
            }
            var previousMemory = new Dictionary<string, object?>(memory);

            // atrib var atrib -> par
            if (parameterList != null)
            {
                for (int i = 0; i < parameterList.parameter().Length; i++)
                {
                    string paramName = parameterList.parameter(i).IDENTIFIER().GetText();
                    object? argValue = Visit(arguments[i]);
                    memory[paramName] = argValue;
                }
            }
            // exec func
            object? result = Visit(functionContext.block());
            memory = previousMemory;
            return result;
        }

        public void ExecuteMainFunction()
        {
            if (functions.ContainsKey("main"))
            {
                Visit(functions["main"].block());
            }
            else
            {
                throw new Exception("Error: Function 'main' not declared.");
            }
        }

        public override object? VisitVariableDeclaration(CParser.VariableDeclarationContext context)
        {
            foreach (var declarator in context.variableDeclarator())
            {
                string varName = declarator.IDENTIFIER().GetText();

                if (declarator.expression() != null)
                {
                    object? value = Visit(declarator.expression());
                    memory[varName] = value;
                }
                else
                {
                    memory[varName] = null;
                }
            }

            return null;
        }

        public override object? VisitDefineDirective(CParser.DefineDirectiveContext context)
        {
            string varName = context.IDENTIFIER().GetText();
            object? value = double.Parse(context.CONSTANT().GetText());
            memory[varName] = value;
            return null;
        }

        public override object? VisitAdditiveExpression(CParser.AdditiveExpressionContext context)
        {
            object? result = Visit(context.multiplicativeExpression(0));

            for (int i = 1; i < context.multiplicativeExpression().Length; i++)
            {
                object? right = Visit(context.multiplicativeExpression(i));
                if (context.GetChild(2 * i - 1).GetText() == "+")
                {
                    result = Convert.ToDouble(result) + Convert.ToDouble(right);
                }
                else if (context.GetChild(2 * i - 1).GetText() == "-")
                {
                    result = Convert.ToDouble(result) - Convert.ToDouble(right);
                }
            }

            return result;
        }

        public override object? VisitMultiplicativeExpression(CParser.MultiplicativeExpressionContext context)
        {
            object? result = Visit(context.unaryExpression(0));

            for (int i = 1; i < context.unaryExpression().Length; i++)
            {
                object? right = Visit(context.unaryExpression(i));
                if (context.GetChild(2 * i - 1).GetText() == "*")
                {
                    result = Convert.ToDouble(result) * Convert.ToDouble(right);
                }
                else if (context.GetChild(2 * i - 1).GetText() == "/")
                {
                    result = Convert.ToDouble(result) / Convert.ToDouble(right);
                }
                else if (context.GetChild(2 * i - 1).GetText() == "%")
                {
                    result = Convert.ToDouble(result) % Convert.ToDouble(right);
                }
            }

            return result;
        }

        public override object? VisitUnaryExpression(CParser.UnaryExpressionContext context)
        {
            if (context.primaryExpression() != null)
            {
                return Visit(context.primaryExpression());
            }
            else
            {
                string varName = context.GetChild(1).GetText();
                if (memory.TryGetValue(varName, out object? value))
                {
                    if (context.GetChild(0).GetText() == "++")
                    {
                        memory[varName] = Convert.ToDouble(memory[varName]) + 1;
                    }
                    else if (context.GetChild(0).GetText() == "--")
                    {
                        memory[varName] = Convert.ToDouble(memory[varName]) - 1;
                    }
                    else if (context.GetChild(0).GetText() == "!")
                    {
                        memory[varName] = !Convert.ToBoolean(memory[varName]);
                    }
                    else if (context.GetChild(0).GetText() == "&")
                    {
                        return memory[varName];
                    }

                    return memory[varName];
                }
                else
                {
                    throw new Exception($"Error: Variable '{varName}' undeclared.");
                }
            }
        }

        public override object? VisitPrimaryExpression(CParser.PrimaryExpressionContext context)
        {
            if (context.CONSTANT() != null)
            {

                return double.Parse(context.CONSTANT().GetText());
            }
            else if (context.IDENTIFIER() != null)
            {
                string varName = context.IDENTIFIER().GetText();

                if (memory.ContainsKey(varName))
                {
                    return memory[varName];
                }
                else
                {
                    throw new Exception($"Error: Variable '{varName}' undeclared.");
                }
            }
            else if (context.STRING_LITERAL() != null)
            {
                return context.STRING_LITERAL().GetText().Trim('"');
            }
            else
            {
                return Visit(context.expression());
            }
        }

        public override object? VisitLogicalOrExpression(CParser.LogicalOrExpressionContext context)
        {
            object? left = Visit(context.logicalAndExpression(0));
            object? right = context.logicalAndExpression().Length > 1 ? Visit(context.logicalAndExpression(1)) : false;

            if (context.GetChild(1) != null && context.GetChild(1).GetText() == "||")
            {
                return Convert.ToBoolean(left) || Convert.ToBoolean(right);
            }

            return base.VisitLogicalOrExpression(context);
        }

        public override object? VisitLogicalAndExpression(CParser.LogicalAndExpressionContext context)
        {
            object? left = Visit(context.equalityExpression(0));
            object? right = context.equalityExpression().Length > 1 ? Visit(context.equalityExpression(1)) : false;

            if (context.GetChild(1) != null && context.GetChild(1).GetText() == "&&")
            {
                return Convert.ToBoolean(left) && Convert.ToBoolean(right);
            }

            return base.VisitLogicalAndExpression(context);
        }

        public override object? VisitEqualityExpression(CParser.EqualityExpressionContext context)
        {
            object? left = Visit(context.relationalExpression(0));
            object? right = context.relationalExpression().Length > 1 ? Visit(context.relationalExpression(1)) : false;

            if (context.GetChild(1) != null && context.GetChild(1).GetText() == "==")
            {
                return left!.Equals(right);
            }
            else if (context.GetChild(1) != null && context.GetChild(1).GetText() == "!=")
            {
                return !left!.Equals(right);
            }

            return base.VisitEqualityExpression(context);
        }

        public override object? VisitRelationalExpression(CParser.RelationalExpressionContext context)
        {
            object? left = Visit(context.additiveExpression(0));
            object? right = context.additiveExpression().Length > 1 ? Visit(context.additiveExpression(1)) : false;

            if (context.GetChild(1) != null && context.GetChild(1).GetText() == "<")
            {
                return Convert.ToDouble(left) < Convert.ToDouble(right);
            }
            else if (context.GetChild(1) != null && context.GetChild(1).GetText() == "<=")
            {
                return Convert.ToDouble(left) <= Convert.ToDouble(right);
            }
            else if (context.GetChild(1) != null && context.GetChild(1).GetText() == ">")
            {
                return Convert.ToDouble(left) > Convert.ToDouble(right);
            }
            else if (context.GetChild(1) != null && context.GetChild(1).GetText() == ">=")
            {
                return Convert.ToDouble(left) >= Convert.ToDouble(right);
            }

            return base.VisitRelationalExpression(context);
        }

        public override object? VisitIfStatement(CParser.IfStatementContext context)
        {
            object? condition = Visit(context.expression());

            if (Convert.ToBoolean(condition))
            {
                Visit(context.statement(0));
            }
            else if (context.statement().Length > 1)
            {
                Visit(context.statement(1));
            }

            return null;
        }

        public override object? VisitWhileStatement(CParser.WhileStatementContext context)
        {
            while (Convert.ToBoolean(Visit(context.expression())))
            {
                Visit(context.statement());
            }

            return null;
        }

        public override object? VisitDoWhileStatement(CParser.DoWhileStatementContext context)
        {
            do
            {
                Visit(context.statement());
            } while (Convert.ToBoolean(Visit(context.expression())));

            return null;
        }

        public override object? VisitForStatement(CParser.ForStatementContext context)
        {
            if (context.expression(0) != null)
            {
                Visit(context.expression(0));
            }

            while (context.expression(1) == null || Convert.ToBoolean(Visit(context.expression(1))))
            {
                Visit(context.statement());

                if (context.expression(2) != null)
                {
                    Visit(context.expression(2));
                }
            }

            return null;
        }

        public override object? VisitSwitchStatement(CParser.SwitchStatementContext context)
        {
            object? switchValue = Visit(context.expression());
            bool caseMatched = false;

            foreach (var caseStatement in context.caseStatement())
            {
                object? caseValue = double.Parse(caseStatement.CONSTANT().GetText());
                if (switchValue!.Equals(caseValue))
                {
                    caseMatched = true;
                    foreach (var statement in caseStatement.statement())
                    {
                        Visit(statement);
                    }
                    break;
                }
            }

            if (!caseMatched && context.defaultStatement() != null)
            {
                foreach (var statement in context.defaultStatement().statement())
                {
                    Visit(statement);
                }
            }

            return null;
        }

        public override object? VisitAssignmentExpression(CParser.AssignmentExpressionContext context)
        {
            if (context.ChildCount == 3)
            {
                string varName = context.IDENTIFIER().GetText();
                object? value = Visit(context.logicalOrExpression());
                memory[varName] = value;
                return value;
            }
            else
            {
                return Visit(context.logicalOrExpression());
            }
        }

        public override object? VisitPrintfStatement(CParser.PrintfStatementContext context)
        {
            List<object?> args = new List<object?>();
            for (int i = 0; i < context.expression().Length; i++)
            {
                args.Add(Visit(context.expression(i)));
            }
            for (int i = 0; i < args.Count; i++)
            {
                if (i == 0 && args[i] is string str)
                {
                    Console.Write(str.Replace("\\n", "\n"));
                }
                else
                {
                    Console.Write(args[i]);
                }
            }
            Console.WriteLine(); 
            return null;
        }

        public override object? VisitScanfStatement(CParser.ScanfStatementContext context)
        {
            string format = context.STRING_LITERAL().GetText();
            for (int i = 0; i < context.IDENTIFIER().Length; i++)
            {
                string varName = context.IDENTIFIER(i).GetText();
                if (memory.ContainsKey(varName))
                {
                    string? input = Console.ReadLine();
                    if (input != null)
                    {
                        if (format.Contains("%d") && int.TryParse(input, out int intValue))
                        {
                            memory[varName] = intValue;
                        }
                        else if (format.Contains("%f") && float.TryParse(input, out float floatValue))
                        {
                            memory[varName] = floatValue;
                        }
                        else
                        {
                            memory[varName] = input;
                        }
                    }
                }
                else
                {
                    throw new Exception($"Error: Variable'{varName}' undeclared.");
                }
            }
            return null;
        }

        public override object? VisitReturnStatement(CParser.ReturnStatementContext context)
        {
            if (context.expression() != null)
            {
                object? returnValue = Visit(context.expression());
                return returnValue;
            }
            return null;
        }

        public override object? VisitPointerDeclaration(CParser.PointerDeclarationContext context)
        {
            
            string varName = context.IDENTIFIER(0).GetText();
            string pointerName = context.IDENTIFIER(1).GetText();

            if (context.GetChild(3) != null)
            {
                memory[varName] = memory[pointerName];
            }
            else
            {
                memory[varName] = null;
            }
            return null;
        }

        public override object? VisitTernaryExpression(CParser.TernaryExpressionContext context)
        {
            object? condition = Visit(context.logicalOrExpression());
            if (condition == null)
            {
                throw new Exception("Error: Ternary condition cannot be null.");
            }

            if (Convert.ToBoolean(condition))
            {
                return Visit(context.statement(0));
            }
            else
            {
                return Visit(context.statement(1));
            }
        }

    }
}