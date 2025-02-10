# Compilador/Interpretador da Linguagem C em C# com ANTLR

Este projeto é um compilador/interpretador para uma linguagem semelhante ao C, desenvolvido em C# utilizando ANTLR (Another Tool for Language Recognition) para análise léxica e sintática. O projeto inclui a análise semântica e a execução de código C básico.

## Estrutura do Projeto

O projeto é composto por três principais componentes:

1. **Análise Léxica e Sintática**: Utiliza ANTLR para gerar o analisador léxico e sintático a partir de uma gramática definida.
2. **Análise Semântica**: Implementa um listener para verificar erros semânticos, como declarações duplicadas de variáveis e funções, chamadas de funções não declaradas, etc.
3. **Interpretação/Execução**: Implementa um visitor para executar o código C, interpretando as instruções e mantendo o estado das variáveis e funções.

## Componentes Principais

### 1. `CErrorListener`

Esta classe é responsável por capturar e reportar erros sintáticos durante a análise léxica e sintática.

#### Métodos Principais:
- **`SyntaxError`**: Captura erros sintáticos e os armazena em uma lista de mensagens de erro.
- **`ReportSyntaxError`**: Reporta os erros sintáticos encontrados.

### 2. `CSemanticExprListener`

Esta classe é responsável por realizar a análise semântica do código C. Ela verifica erros como declarações duplicadas de variáveis e funções, chamadas de funções não declaradas, etc.

#### Métodos Principais:
- **`ExitMainFunction`**: Verifica se a função `main` está corretamente declarada.
- **`ExitVariableDeclaration`**: Verifica se uma variável já foi declarada.
- **`ExitFunctionDeclaration`**: Verifica se uma função já foi declarada.
- **`ExitChamadaStatement`**: Verifica se uma função chamada foi declarada e se o número de argumentos está correto.
- **`ExitReturnStatement`**: Verifica se a declaração de retorno possui uma expressão.
- **`ExitIfStatement`**: Verifica se a condição do `if` está presente.
- **`ExitWhileStatement`**: Verifica se a condição do `while` está presente.
- **`ExitForStatement`**: Verifica se as expressões do `for` estão presentes.
- **`ExitPrintfStatement`**: Verifica se a expressão do `printf` está presente.
- **`ExitScanfStatement`**: Verifica se a variável do `scanf` está presente.

### 3. `CVisitorImpl`

Esta classe é responsável por interpretar e executar o código C. Ela implementa um visitor que percorre a árvore sintática gerada pelo ANTLR e executa as instruções.

#### Métodos Principais:
- **`VisitFunctionDeclaration`**: Visita a declaração de uma função e a armazena em um dicionário.
- **`VisitChamadaStatement`**: Visita uma chamada de função e executa a função correspondente.
- **`ExecuteMainFunction`**: Executa a função `main` se ela estiver declarada.
- **`VisitVariableDeclaration`**: Visita a declaração de uma variável e a armazena em um dicionário.
- **`VisitDefineDirective`**: Visita uma diretiva `#define` e armazena o valor definido.
- **`VisitAdditiveExpression`**: Visita uma expressão aditiva e realiza a operação correspondente.
- **`VisitMultiplicativeExpression`**: Visita uma expressão multiplicativa e realiza a operação correspondente.
- **`VisitUnaryExpression`**: Visita uma expressão unária e realiza a operação correspondente.
- **`VisitPrimaryExpression`**: Visita uma expressão primária (constante, identificador, etc.).
- **`VisitLogicalOrExpression`**: Visita uma expressão lógica OR.
- **`VisitLogicalAndExpression`**: Visita uma expressão lógica AND.
- **`VisitEqualityExpression`**: Visita uma expressão de igualdade.
- **`VisitRelationalExpression`**: Visita uma expressão relacional.
- **`VisitIfStatement`**: Visita uma declaração `if` e executa o bloco correspondente.
- **`VisitWhileStatement`**: Visita uma declaração `while` e executa o bloco correspondente.
- **`VisitDoWhileStatement`**: Visita uma declaração `do-while` e executa o bloco correspondente.
- **`VisitForStatement`**: Visita uma declaração `for` e executa o bloco correspondente.
- **`VisitSwitchStatement`**: Visita uma declaração `switch` e executa o bloco correspondente.
- **`VisitAssignmentExpression`**: Visita uma expressão de atribuição e atualiza o valor da variável.
- **`VisitPrintfStatement`**: Visita uma declaração `printf` e imprime o valor correspondente.
- **`VisitScanfStatement`**: Visita uma declaração `scanf` e lê a entrada do usuário.
- **`VisitReturnStatement`**: Visita uma declaração `return` e retorna o valor correspondente.
- **`VisitPointerDeclaration`**: Visita uma declaração de ponteiro.
- **`VisitTernaryExpression`**: Visita uma expressão ternária e executa o bloco correspondente.

### 4. `Program`

Esta classe contém o ponto de entrada do programa. Ela lê o código-fonte de um arquivo, realiza a análise léxica e sintática, verifica erros semânticos e, se não houver erros, executa o código.

#### Métodos Principais:
- **`Main`**: Ponto de entrada do programa. Lê o arquivo de entrada, realiza a análise e executa o código.

## Como Usar

1. **Compilação**: Compile o projeto usando o .NET SDK.
   ```bash
   dotnet build

   dotnet run <caminho_do_arquivo.c>


   