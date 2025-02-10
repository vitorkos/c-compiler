grammar C;

program : preprocessorDirective* (declaration | functionDeclaration)* mainFunction;

preprocessorDirective
    : '#' 'include' '<' HEADER_FILE '>'
    ;

defineDirective
    : '#' 'define' IDENTIFIER CONSTANT
    ;    

mainFunction
    : 'int' 'main' '(' ')' block
    ;

declaration
    : variableDeclaration
    | statement
    | structDeclaration
    | unionDeclaration
    | defineDirective
    | returnStatement
    ;

functionDeclaration
    : type IDENTIFIER '(' parameterList? ')' block
    ;

parameterList
    : parameter (',' parameter)*
    ;

parameter
    : type IDENTIFIER
    ;

variableDeclaration
    : type variableDeclarator (',' variableDeclarator)* ';'
    ;

variableDeclarator
    : IDENTIFIER ('[' CONSTANT ']')* ('=' expression)?
    | '*' IDENTIFIER ('[' CONSTANT ']')* ('=' expression)?
    ;

structDeclaration
    : 'struct' IDENTIFIER '{' structMember* '}' ';'
    ;

structMember
    : type IDENTIFIER ('[' CONSTANT ']')? ';'
    ;

unionDeclaration
    : 'union' IDENTIFIER '{' unionMember* '}' ';'
    ;

unionMember
    : type IDENTIFIER ('[' CONSTANT ']')? ';'
    ;

block
    : '{' statement* '}'
    ;

statement
    : expressionStatement
    | blockStatement
    | ifStatement
    | returnStatement
    | variableDeclaration
    | forStatement
    | whileStatement
    | doWhileStatement
    | switchStatement
    | scanfStatement
    | printfStatement
    | chamadaStatement ';'
    | returnStatement
    | pointerDeclaration
    | ternaryExpression
    ;

expressionStatement
    : expression ';'
    ;

printfStatement
    : 'printf' '(' (expression | STRING_LITERAL) (',' expression)* ')' ';'
    ;

scanfStatement
    : 'scanf' '(' STRING_LITERAL (',' ('&'? IDENTIFIER))* ')' ';'
    ;

blockStatement
    : '{' statement* '}'
    ;

ifStatement
    : 'if' '(' expression ')' statement ( 'else' statement )?
    ;

switchStatement
    : 'switch' '(' expression ')' '{' caseStatement* defaultStatement? '}'
    ;

caseStatement
    : 'case' CONSTANT ':' statement* 'break' ';'
    ;

defaultStatement
    : 'default' ':' statement*
    ;
    

forStatement
    : 'for' '(' (variableDeclaration | expression)? ';' expression? ';' expression? ')' statement
    ;

whileStatement
    : 'while' '(' expression ')' statement
    ;

doWhileStatement
    : 'do' statement 'while' '(' expression ')' ';'
    ;

chamadaStatement
    : IDENTIFIER '(' (expression (',' expression)*)? ')'
    ;    

returnStatement
    : 'return' (expression)? ';'
    ;

pointerDeclaration
    : type '*' IDENTIFIER ('=' '&' IDENTIFIER)? ';'
    ;


ternaryExpression
    : logicalOrExpression '?' statement ':' statement
    ;


type
    : 'int'
    | 'float'
    | 'void'
    | 'char'
    | 'double'
    | 'short'
    | 'long'
    | 'unsigned'
    ;

expression
    : assignmentExpression
    | ternaryExpression
    ;

assignmentExpression
    : IDENTIFIER '=' logicalOrExpression
    | logicalOrExpression
    ;

logicalOrExpression
    : logicalAndExpression ( '||' logicalAndExpression )*
    ;

logicalAndExpression
    : equalityExpression ( '&&' equalityExpression )*
    ;

equalityExpression
    : relationalExpression ( ( '==' | '!=' ) relationalExpression )*
    ;

relationalExpression
    : additiveExpression ( ( '<' | '>' | '<=' | '>=' ) additiveExpression )*
    ;

additiveExpression
    : multiplicativeExpression ( ( '+' | '-' ) multiplicativeExpression )*
    ;

multiplicativeExpression
    : unaryExpression ( ( '*' | '/' | '%' ) unaryExpression )*
    ;

unaryExpression
    : primaryExpression
    | ('+' | '-' | '!' | '++' | '--' | '&' | '*') unaryExpression
    ;

primaryExpression
    : '(' expression ')'
    | IDENTIFIER
    | CONSTANT
    | STRING_LITERAL
    | 'sizeof' '(' type ')'
    ;

CONSTANT: INT | FLOAT | CHAR;
INT: [0-9]+;
FLOAT: [0-9]+ '.' [0-9]+;
CHAR: '\'' . '\'';
IDENTIFIER : [a-zA-Z_][a-zA-Z0-9_]*;
STRING_LITERAL : '"' (~["\\] | '\\' .)* '"';
HEADER_FILE : [a-zA-Z_][a-zA-Z0-9_]* '.' [a-zA-Z_][a-zA-Z0-9_]*;
WS : [ \t\r\n]+ -> skip;
COMMENT : '/*' .*? '*/' -> skip;
LINE_COMMENT : '//' ~[\r\n]* -> skip;