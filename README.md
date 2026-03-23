# SmartExpressions

[![.NET Master](https://github.com/Lasse27/SmartExpressions/actions/workflows/dotnet-master.yml/badge.svg?branch=master)](https://github.com/Lasse27/SmartExpressions/actions/workflows/dotnet-master.yml)

SmartExpressions is a .NET-based framework for parsing and evaluating expressions. It provides a custom expression parser and evaluator supporting arithmetic, logical, and comparison operators, as well as variables, constants, and nested expressions. The library is designed for applications that require dynamic runtime calculation and interpretation of expressions.

> The project was originally made for personal use but I decided to make it public. Although I want to exclaim that this project is still in the works.

## Functionality
The evaluation ultimately consists of a parser and a lexer/tokenizer. The lexer determines the tokens from the input string using statically registered values. The tokens are then checked with the parser and subsequently converted into an AST (abstract syntax tree).

In general, the evaluation is quite lenient with incorrect types and/or null values.
For example, a `True` in a `LessThan (LT)` comparison is handled as 1.

This results in the following functions:

<h3>Conditional functions</h3>
<table width="100%">
  <thead>
    <tr>
      <th>Function</th>
      <th>Description</th>
      <th>Example/s</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>if</td>
      <td>Evaluates a condition and returns one expression if true, otherwise another</td>
      <td>
	  	<code>
			if( eq(1,1) ) 
			{ 
				add(2,3) 
			} 
			elif ( eq(1,2) )
			{
				mult(1,9)
			}
			else 
			{ 
				sub(5,2) 
			}
		</code>
	  </td>
    </tr>
  </tbody>
</table>

<h3>Logical functions</h3>
<table width="100%">
  <thead>
    <tr>
      <th>Function</th>
      <th>Description</th>
      <th>Example/s</th>
    </tr>
  </thead>
  <tbody>
    <tr><td>and</td><td>Returns true if both expressions evaluate to true</td><td><code>and(true, false)</code></td></tr>
    <tr><td>or</td><td>Returns true if at least one expression evaluates to true</td><td><code>or(true, false)</code></td></tr>
    <tr><td>not</td><td>Inverts the boolean value of the expression</td><td><code>not(false)</code></td></tr>
    <tr><td>xor</td><td>Returns true if exactly one expression is true</td><td><code>xor(true, false)</code></td></tr>
    <tr><td>nand</td><td>Returns false only if both expressions are true</td><td><code>nand(true, true)</code></td></tr>
    <tr><td>nor</td><td>Returns true only if both expressions are false</td><td><code>nor(false, false)</code></td></tr>
    <tr><td>xnor</td><td>Returns true if both expressions have the same boolean value</td><td><code>xnor(true, false)</code></td></tr>
  </tbody>
</table>

<h3>Comparison functions</h3>
<table width="100%">
  <thead>
    <tr>
      <th>Function</th>
      <th>Description</th>
      <th>Example/s</th>
    </tr>
  </thead>
  <tbody>
    <tr><td>eq</td><td>Checks if two expressions are equal</td><td><code>eq(5,5)</code></td></tr>
    <tr><td>neq</td><td>Checks if two expressions are not equal</td><td><code>neq(5,3)</code></td></tr>
    <tr><td>lt</td><td>Checks if the first expression is less than the second</td><td><code>lt(2,5)</code></td></tr>
    <tr><td>gt</td><td>Checks if the first expression is greater than the second</td><td><code>gt(10,3)</code></td></tr>
    <tr><td>lte</td><td>Checks if the first expression is less than or equal to the second</td><td><code>lte(3,3)</code></td></tr>
    <tr><td>gte</td><td>Checks if the first expression is greater than or equal to the second</td><td><code>gte(4,2)</code></td></tr>
  </tbody>
</table>

<h3>Arithmetic functions</h3>
<table width="100%">
  <thead>
    <tr>
      <th>Function</th>
      <th>Description</th>
      <th>Example/s</th>
    </tr>
  </thead>
  <tbody>
    <tr><td>add</td><td>Returns the sum of two expressions</td><td><code>add(2,3)</code></td></tr>
    <tr><td>sub</td><td>Returns the difference of two expressions</td><td><code>sub(5,2)</code></td></tr>
    <tr><td>mult</td><td>Returns the product of two expressions</td><td><code>mult(3,4)</code></td></tr>
    <tr><td>div</td><td>Returns the quotient of two expressions</td><td><code>div(10,2)</code></td></tr>
    <tr><td>mod</td><td>Returns the remainder of a division</td><td><code>mod(10,3)</code></td></tr>
    <tr><td>pow</td><td>Raises the first expression to the power of the second</td><td><code>pow(2,3)</code></td></tr>
    <tr><td>root</td><td>Computes the nth root of a number</td><td><code>root(16,2)</code></td></tr>
    <tr><td>abs</td><td>Returns the absolute (non-negative) value</td><td><code>abs(-5)</code></td></tr>
    <tr><td>neg</td><td>Negates the value (multiplies by -1)</td><td><code>neg(7)</code></td></tr>
    <tr><td>rand</td><td>Generates a random number within the given range</td><td><code>rand(2,10)</code></td></tr>
  </tbody>
</table>

<h3>Trigonometric functions</h3>
<table width="100%">
  <thead>
    <tr>
      <th>Function</th>
      <th>Description</th>
      <th>Example/s</th>
    </tr>
  </thead>
  <tbody>
    <tr><td>sin</td><td>Returns the sine of an angle (in radians)</td><td><code>sin(0)</code></td></tr>
    <tr><td>cos</td><td>Returns the cosine of an angle (in radians)</td><td><code>cos(0)</code></td></tr>
    <tr><td>tan</td><td>Returns the tangent of an angle (in radians)</td><td><code>tan(0)</code></td></tr>
    <tr><td>sinh</td><td>Returns the hyperbolic sine of a value</td><td><code>sinh(1)</code></td></tr>
    <tr><td>cosh</td><td>Returns the hyperbolic cosine of a value</td><td><code>cosh(1)</code></td></tr>
    <tr><td>tanh</td><td>Returns the hyperbolic tangent of a value</td><td><code>tanh(1)</code></td></tr>
    <tr><td>asin</td><td>Returns the inverse sine (arcsine) of a value, result in radians</td><td><code>asin(0.5)</code></td></tr>
    <tr><td>acos</td><td>Returns the inverse cosine (arccosine) of a value, result in radians</td><td><code>acos(0.5)</code></td></tr>
    <tr><td>atan</td><td>Returns the inverse tangent (arctangent) of a value, result in radians</td><td><code>atan(1)</code></td></tr>
    <tr><td>rad</td><td>Converts the given value (in degree) to its radians counterpart. </td><td><code>rad(360)</code></td></tr>
    <tr><td>deg</td><td>Converts the given value (in radians) to its degree counterpart.</td><td><code>deg(3.14)</code></td></tr>
  </tbody>
</table>

<h3>Keys and constants</h3>
<table width="100%">
  <thead>
    <tr>
      <th>Function</th>
      <th>Description</th>
      <th>Example/s</th>
    </tr>
  </thead>
  <tbody>
    <tr><td>pi</td><td>Mathematical constant π (~3.14159)</td><td><code>pi</code></td></tr>
    <tr><td>e</td><td>Euler's number (~2.71828)</td><td><code>e</code></td></tr>
    <tr><td>tau</td><td>the ratio of a circle's circumference to its radius (~6.283185)</td><td><code>tau</code></td></tr>
    <tr><td>true</td><td>Boolean constant representing true</td><td><code>true</code></td></tr>
    <tr><td>false</td><td>Boolean constant representing false</td><td><code>false</code></td></tr>
    <tr><td>null</td><td>Represents the absence of a value</td><td><code>null</code></td></tr>
    <tr><td>1..9..100..</td><td>Numeric literal (integer or floating point, optionally negative)</td><td><code>42</code>, <code>-3.14</code></td></tr>
    <tr><td>Identifier</td><td>Variable reference using a named identifier</td><td><code>@{myVar_1}</code></td></tr>
  </tbody>
</table>

## Usage

An example expression looks like this:

```csharp
[Fact]
public void Simple_Expression()
{
	Expression expression = new Expression("add(1, 1)");
	EvaluationResult operation = expression.Evaluate();

	// Assert
	Assert.NotNull(operation.GetValue());
	Assert.Equal(2D, operation.GetValue());

	// Output
	_outputHelper.WriteLine(operation.GetValue().ToString()); // 2.0D
}
```

Function calls can be nested arbitrarily:

```csharp
[Fact]
public void Simple_Nested_Expression()
{
	Expression expression = new Expression("add(1, mult(5,5))");
	EvaluationResult operation = expression.Evaluate();

	// Assert
	Assert.NotNull(operation.GetValue());
	Assert.Equal(26D, operation.GetValue());

	// Output
	_outputHelper.WriteLine(operation.GetValue().ToString()); // 26.0D
}
```

Expressions are not affected by spaces and line breaks:

```csharp
public void Simple_Expression_With_Whitespace()
{
	Expression expression = new Expression("add  (1   , mult    (5,      5))");
	EvaluationResult operation = expression.Evaluate();

	// Assert
	Assert.NotNull(operation.GetValue());
	Assert.Equal(26D, operation.GetValue());

	// Output
	_outputHelper.WriteLine(operation.GetValue().ToString()); // 26.0D
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
	Expression expression = new Expression("add(@{Key_1}, 25)");
	_ = expression.RegisterBinding("Key_1", 66);
	EvaluationResult operation = expression.Evaluate();

	// Assert
	Assert.NotNull(operation.GetValue());
	Assert.Equal(91D, operation.GetValue());

	// Output
	_outputHelper.WriteLine(operation.GetValue().ToString()); // 91.0D
}
```

Rebinding parameters with the same key overwrites the values of the bound key.

```csharp
[Fact]
public void Expression_With_Rebound_Identifier()
{
	Expression expression = new Expression("add(@{Key_1}, 25)");
	_ = expression.RegisterBinding("Key_1", 66);
	EvaluationResult operation = expression.Evaluate();

	// Assert
	Assert.NotNull(operation.GetValue());
	Assert.Equal(91D, operation.GetValue());

	// Output
	_outputHelper.WriteLine(operation.GetValue().ToString()); // 91.0D

	_ = expression.RegisterBinding("Key_1", 60);
	EvaluationResult operation2 = expression.Evaluate();

	// Assert
	Assert.NotNull(operation.GetValue());
	Assert.Equal(85D, operation2.GetValue());

	// Output
	_outputHelper.WriteLine(operation2.GetValue().ToString()); // 85.0D
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

	Expression expression = new Expression("add(sub(2,1),mult(5,5))");
	EvaluationResult operation = expression.Evaluate(progress);

	// Assert
	Assert.NotNull(operation.GetValue());
	Assert.Equal(26D, operation.GetValue());

	// Output
	_outputHelper.WriteLine(operation.GetValue().ToString());

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