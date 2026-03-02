# SmartExpressions
[![.NET Master](https://github.com/Lasse27/SmartExpressions/actions/workflows/dotnet-master.yml/badge.svg?branch=master)](https://github.com/Lasse27/SmartExpressions/actions/workflows/dotnet-master.yml)

SmartExpressions is a .NET-based framework for parsing and evaluating expressions. It provides a custom expression parser and evaluator supporting arithmetic, logical, and comparison operators, as well as variables, constants, and nested expressions. The library is designed for applications that require dynamic runtime calculation and interpretation of expressions.

> The project was originally made for personal use but I decided to make it public, so anybody who wants to make use of it can do so.


## EBNF
```
expression      ::= if_expr | logical_expr | arithmetic_expr | primary

if_expr         ::= "if" "(" expression ")" "{" expression "}" "else" "{" expression "}"


logical_expr    ::= and_expr | or_expr | not_expr | xor_expr | nand_expr | nor_expr | xnor_expr | comparison_expr
and_expr        ::= "AND"  "(" expression "," expression ")"
or_expr         ::= "OR"   "(" expression "," expression ")"
not_expr        ::= "NOT"  "(" expression ")"
xor_expr        ::= "XOR"  "(" expression "," expression ")"
nand_expr       ::= "NAND" "(" expression "," expression ")"
nor_expr        ::= "NOR"  "(" expression "," expression ")"
xnor_expr       ::= "XNOR" "(" expression "," expression ")"


comparison_expr ::= eq_expr | neq_expr | lt_expr | gt_expr | lte_expr | gte_expr
eq_expr         ::= "EQ"  "(" expression "," expression ")"
neq_expr        ::= "NEQ" "(" expression "," expression ")"
lt_expr         ::= "LT"  "(" expression "," expression ")"
gt_expr         ::= "GT"  "(" expression "," expression ")"
lte_expr        ::= "LTE" "(" expression "," expression ")"
gte_expr        ::= "GTE" "(" expression "," expression ")"


arithmetic_expr ::= add_expr | sub_expr | mult_expr | div_expr | mod_expr | pow_expr | sqrt_expr | abs_expr | neg_expr
add_expr        ::= "ADD"  "(" expression "," expression ")"
sub_expr        ::= "SUB"  "(" expression "," expression ")"
mult_expr       ::= "MULT" "(" expression "," expression ")"
div_expr        ::= "DIV"  "(" expression "," expression ")"
mod_expr        ::= "MOD"  "(" expression "," expression ")"
pow_expr        ::= "POW"  "(" expression "," expression ")"
sqrt_expr       ::= "SQRT" "(" expression ")"
abs_expr        ::= "ABS"  "(" expression ")"
neg_expr        ::= "NEG"  "(" expression ")"


primary         ::= constant | boolean | number | identifier | null
constant        ::= "pi" | "e"
boolean         ::= "true" | "false"
null            ::= "null"
number          ::= DIGIT+ ("." DIGIT+)?
identifier      ::= "@{" LETTER (LETTER | DIGIT | "_")* "}"
DIGIT           ::= "0".."9"
LETTER          ::= "a".."z" | "A".."Z"
```