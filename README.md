# SmartExpressions

[![.NET Master](https://github.com/Lasse27/SmartExpressions/actions/workflows/dotnet-master.yml/badge.svg?branch=master)](https://github.com/Lasse27/SmartExpressions/actions/workflows/dotnet-master.yml)

SmartExpressions is a .NET-based framework for parsing and evaluating expressions. It provides a custom expression parser and evaluator supporting arithmetic, logical, and comparison operators, as well as variables, constants, and nested expressions. The library is designed for applications that require dynamic runtime calculation and interpretation of expressions.

> The project was originally made for personal use but I decided to make it public. Anybody who wants to make use of it, can do so. Although I want to exclaim that this project is still in the works.

## Functionality
The evaluation ultimately consists of a parser and a lexer/tokenizer. The lexer determines the tokens from the input string using statically registered values. The tokens are then checked with the parser and subsequently converted into an AST (abstract syntax tree).

Both follow the EBNF listed below during the analysis.

### EBNF
```
expression      ::= if_expr | logical_expr | arithmetic_expr | statistic_expr | primary_expr
```

```
if_expr         ::= "if" "(" expression ")" "{" expression "}" "else" "{" expression "}"
```

```
logical_expr    ::= and_expr | or_expr | not_expr | xor_expr | nand_expr | nor_expr | xnor_expr | comparison_expr
and_expr        ::= "AND"  "(" expression "," expression ")"
or_expr         ::= "OR"   "(" expression "," expression ")"
not_expr        ::= "NOT"  "(" expression ")"
xor_expr        ::= "XOR"  "(" expression "," expression ")"
nand_expr       ::= "NAND" "(" expression "," expression ")"
nor_expr        ::= "NOR"  "(" expression "," expression ")"
xnor_expr       ::= "XNOR" "(" expression "," expression ")"
```

```
comparison_expr ::= eq_expr | neq_expr | lt_expr | gt_expr | lte_expr | gte_expr
eq_expr         ::= "EQ"  "(" expression "," expression ")"
neq_expr        ::= "NEQ" "(" expression "," expression ")"
lt_expr         ::= "LT"  "(" expression "," expression ")"
gt_expr         ::= "GT"  "(" expression "," expression ")"
lte_expr        ::= "LTE" "(" expression "," expression ")"
gte_expr        ::= "GTE" "(" expression "," expression ")"
```

```
arithmetic_expr ::= add_expr | sub_expr | mult_expr | div_expr | mod_expr | pow_expr | sqrt_expr | abs_expr | neg_expr
add_expr        ::= "ADD"  "(" expression "," expression ")"
sub_expr        ::= "SUB"  "(" expression "," expression ")"
mult_expr       ::= "MULT" "(" expression "," expression ")"
div_expr        ::= "DIV"  "(" expression "," expression ")"
mod_expr        ::= "MOD"  "(" expression "," expression ")"
pow_expr        ::= "POW"  "(" expression "," expression ")"
sqrt_expr       ::= "ROOT" "(" expression "," expression ")"
abs_expr        ::= "ABS"  "(" expression ")"
neg_expr        ::= "NEG"  "(" expression ")"
```

```
primary         ::= constant | boolean | number | identifier | null
constant        ::= "pi" | "e"
boolean         ::= "true" | "false"
null            ::= "null"
number          ::= (-)? DIGIT+ ("." DIGIT+)?
identifier      ::= "@{" LETTER (LETTER | DIGIT | "_")* "}"
DIGIT           ::= "0".."9"
LETTER          ::= "a".."z" | "A".."Z"
```

In general, the evaluation is quite lenient with incorrect types and/or null values.
For example, a `True` in a `LessThan (LT)` comparison is evaluated as 1.

This results in the following functions:

### Conditional 

| Function    | Format                                              | Example/s                                    | 
| ----------- | --------------------------------------------------- | -------------------------------------------- |
| If		  | `if(expression) { expression } else { expression }` | `if(EQ(1,1)) { ADD(2,3) } else { SUB(5,2) }` | 


### Logical

| Function    | Format                                              | Example/s                                    | 
| ----------- | --------------------------------------------------- | -------------------------------------------- |
| AND         | `AND(expression, expression)`                       | `AND(true, false)`                           | 
| OR          | `OR(expression, expression)`                        | `OR(true, false)`                            | 
| NOT         | `NOT(expression)`                                   | `NOT(false)`                                 | 
| XOR         | `XOR(expression, expression)`                       | `XOR(true, false)`                           | 
| NAND        | `NAND(expression, expression)`                      | `NAND(true, true)`                           | 
| NOR         | `NOR(expression, expression)`                       | `NOR(false, false)`                          | 
| XNOR        | `XNOR(expression, expression)`                      | `XNOR(true, false)`                          | 


### Comparison

| Function    | Format                                              | Example/s                                    | 
| ----------- | --------------------------------------------------- | -------------------------------------------- |
| EQ          | `EQ(expression, expression)`                        | `EQ(5,5)`                                    | 
| NEQ         | `NEQ(expression, expression)`                       | `NEQ(5,3)`                                   | 
| LT          | `LT(expression, expression)`                        | `LT(2,5)`                                    | 
| GT          | `GT(expression, expression)`                        | `GT(10,3)`                                   | 
| LTE         | `LTE(expression, expression)`                       | `LTE(3,3)`                                   | 
| GTE         | `GTE(expression, expression)`                       | `GTE(4,2)`                                   | 


### Arithmetic

| Function    | Format                                              | Example/s                                    | 
| ----------- | --------------------------------------------------- | -------------------------------------------- |
| ADD         | `ADD(expression, expression)`                       | `ADD(2,3)`                                   | 
| SUB         | `SUB(expression, expression)`                       | `SUB(5,2)`                                   | 
| MULT        | `MULT(expression, expression)`                      | `MULT(3,4)`                                  | 
| DIV         | `DIV(expression, expression)`                       | `DIV(10,2)`                                  | 
| MOD         | `MOD(expression, expression)`                       | `MOD(10,3)`                                  | 
| POW         | `POW(expression, expression)`                       | `POW(2,3)`                                   | 
| ROOT        | `ROOT(expression, expression)`                      | `ROOT(16,2)`                                 | 
| ABS         | `ABS(expression)`                                   | `ABS(-5)`                                    | 
| NEG         | `NEG(expression)`                                   | `NEG(7)`                                     | 
| RAND        | `RAND(expression, expression)`                      | `RAND(2,10)`                                 | 


### Keys and constants

| Function    | Format                                              | Example/s                                    | 
| ----------- | --------------------------------------------------- | -------------------------------------------- |
| PI		  | `PI`                                                | `PI`                                         |
| E			  | `E`                                                 | `E`                                          |
| TRUE        | `TRUE`                                              | `TRUE`                                       |
| FALSE       | `FALSE`                                             | `FALSE`                                      |
| NULL        | `null`                                              | `null`                                       |
| 1..9..100.. | `(-)? DIGIT+ ("." DIGIT+)?`                         | `42`, `-3.14`                                | 
| Identifier  | `@{LETTER(LETTER | DIGIT | _)*}`					| `@{myVar_1}`								   |


## Usage

An example expression looks like this:

```csharp
[Fact]
public void Simple_Expression()
{
	Expression expression = new Expression("Add(1, 1)");
	Operation<object> operation = expression.Evaluate();

	// Assert
	Assert.Equal(Status.Ok, operation.Status);
	Assert.NotNull(operation.Value);
	Assert.Equal(2D, operation.Value);

	// Output
	_outputHelper.WriteLine(operation.Value.ToString()); // 2
}
```

Function calls can be nested arbitrarily:

```csharp
[Fact]
public void Simple_Nested_Expression()
{
	Expression expression = new Expression("Add(1, MULT(5,5))");
	Operation<object> operation = expression.Evaluate();

	// Assert
	Assert.Equal(Status.Ok, operation.Status);
	Assert.NotNull(operation.Value);
	Assert.Equal(26D, operation.Value);

	// Output
	_outputHelper.WriteLine(operation.Value.ToString()); // 26
}
```

Expressions are not affected by spaces and line breaks:

```csharp
[Fact]
public void Simple_Expression_With_Whitespace()
{
	Expression expression = new Expression("Add  (1   , MULT    (5,      5))");
	Operation<object> operation = expression.Evaluate();

	// Assert
	Assert.Equal(Status.Ok, operation.Status);
	Assert.NotNull(operation.Value);
	Assert.Equal(26D, operation.Value);

	// Output
	_outputHelper.WriteLine(operation.Value.ToString()); // 26
}
```

The parsing and tokenization/lexing of the input values is first performed when the formula is evaluated for the first time. After that, it is only performed again when the formula changes, for example, by `Expression.SetFormula()`.

> In general, the methods `Expression.Tokenize()` and `Expression.Parse()` are idempotent. They only run once as long as the expression input does not change.

If parsing and/or tokenizing is to be performed earlier, automatic parsing and tokenizing can be disabled using the `AssembleOnEvaluation` property. The user is then responsible for parsing and tokenizing the expression at the appropriate moment. 


### Identifiers

Identifiers are the way to integrate your own values into expressions. To do this, the value must first be bound to the expression using the BindParameters() method via a key-value pair.

```csharp
[Fact]
public void Expression_With_Identifier()
{
	Expression expression = new Expression("Add(@{Key_1}, 25)");
	_ = expression.Bind("Key_1", 66);
	Operation<object> operation = expression.Evaluate();

	// Assert
	Assert.Equal(Status.Ok, operation.Status);
	Assert.NotNull(operation.Value);
	Assert.Equal(91D, operation.Value);

	// Output
	_outputHelper.WriteLine(operation.Value.ToString()); // 91
}
```

Rebinding parameters with the same key overwrites the values of the bound key.

```
[Fact]
public void Expression_With_Rebound_Identifier()
{
	Expression expression = new Expression("Add(@{Key_1}, 25)");
	_ = expression.Bind("Key_1", 66);
	Operation<object> operation = expression.Evaluate();

	// Assert
	Assert.Equal(Status.Ok, operation.Status);
	Assert.NotNull(operation.Value);
	Assert.Equal(91D, operation.Value);

	// Output
	_outputHelper.WriteLine(operation.Value.ToString());

	_ = expression.Bind("Key_1", 60);
	Operation<object> operation2 = expression.Evaluate();

	// Assert
	Assert.Equal(Status.Ok, operation2.Status);
	Assert.NotNull(operation.Value);
	Assert.Equal(85D, operation2.Value);

	// Output
	_outputHelper.WriteLine(operation2.Value.ToString()); // 85
}
}
```

Binding only supports primary types. To be precise, these are the following types:

- `bool`
- `byte`
- `short`
- `integer`
- `double`
- `decimal`


## Progress

The `Expression.Evaluate()` method accepts an optional parameter `IProgress<string>`. If you enter a corresponding object of this type into the method, intermediate results of the individual nodes are returned.

```csharp
[Fact]
public void Simple_Expression_With_Progress()
{
	Progress<string> progress = new Progress<string>();
	progress.ProgressChanged += (_, e) => _outputHelper.WriteLine(e);

	Expression expression = new Expression("Add(SUB(2,1),MULT(5,5))");
	Operation<object> operation = expression.Evaluate(progress);

	// Assert
	Assert.Equal(Status.Ok, operation.Status);
	Assert.NotNull(operation.Value);
	Assert.Equal(26D, operation.Value);

	// Output
	_outputHelper.WriteLine(operation.Value.ToString());

	// Console output
	// SUB(2, 1) = 1
	// ADD(SUB(2, 1), MULT(5, 5)) = 26
	// MULT(5, 5) = 25
	// 26
}
```

# Benchmarks

I am continuing to work on improving the performance of the lexer, parser, and evaluator. 
I am using `DotNetBenchmark` to analyze performance and will publish benchmark results at appropriate times.


# Contributing

If you don't like something, want to suggest ideas, or have encountered a bug, feel free to create an issue or pull request. 
Forks are always welcome. I appreciate all feedback, whether positive or negative.

The project will initially be managed exclusively by me. For serious inquiries, please contact me at lassehillen@gmx.de.

# License

The project is published under the MIT License. See LICENSE.txt for more infos.