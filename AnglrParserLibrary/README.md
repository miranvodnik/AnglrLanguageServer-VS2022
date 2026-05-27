# AnglrMSBuildTasks — MSBuild Integration for the Anglr Compiler

AnglrMSBuildTasks provides seamless MSBuild integration for the **Anglr language compiler**, enabling automatic compilation of `.anglr` grammar files during project builds.  
It supports **per‑file configuration**, **incremental builds**, and **custom output generation**.

This package is used by the Anglr compiler, the Anglr VSIX extension, and any .NET project that includes `.anglr` grammar files.

Anglr language is described on https://www.angstlr.com

---

## **Features**

- 🔧 Automatic compilation of `.anglr` files during `dotnet build`
- 📁 Per‑file configuration using MSBuild metadata
- 🧩 Generates C# source files and optional library files
- ⚡ Incremental build support
- 🛠 Fully compatible with Visual Studio, VS Code, and CI pipelines
- 🎛 Optional VSIX extension adds syntax highlighting and property pages

---

## **Installation**

Add the NuGet package:

```bash
dotnet add package AnglrMSBuildTasks
```

Or edit your `.csproj`:

```
<ItemGroup>
  <PackageReference Include="AnglrMSBuildTasks" Version="x.y.z" />
</ItemGroup>
```

Or install it directly from VisualStudio:
- open Solution Explorer
- select a project that is supposed to contain the NuGet package AnglrMSBuildTasks.
- using drop-down menu for selected project, activate Manage NuGet Packages...
- in NuGet tool window browse Package source `nuget.org` using Search string `anglr`
- a list of Anglr* packages appears.
- select AnglrMSBuildTasks package and install it.
- The other Anglr* packages will also be installed because AnglrMSBuildTasks depends on them.

Once installed, MSBuild automatically detects `.anglr` files and compiles them.

---

## **How It Works**

AnglrMSBuildTasks defines an MSBuild item type:

```bash
<AnglrFile Include="**\*.anglr" />
```
Each `.anglr` file is passed to the `Anglr` MSBuild task, which generates:

- ***Source files*** (e.g., `.g.cs`)
- ***Optional library files*** (e.g., .dll or .json depending on your compiler)

You can configure each file individually using ***metadata***.

---

## **Per-File Configuration**

Each `.anglr` file supports the following metadata:

| **Metadata** | **Type** | **Default** | **Description**                                            |
|--------------|----------|-------------|------------------------------------------------------------|
| `Debug`      | bool     | `false`     | Emit debug information                                     |
| `Tree`       | bool     | `true`      | Generate parse tree                                        |
| `Loop`       | bool     | `true`      | Generate parser loop detection                             |
| `Iterator`   | bool     | `false`     | Iterative algorithms have an advantage over recursive ones |
| `Precedence` | bool     | `false`     | Generate precedence grammar                                |
| `OutputDir`  | string   | `Syntax`    | Name of output directory for generated files               |
| `Code`       | string	   | `cs`        | Target language (`cs` - C#, future: `java`, `c++`, etc.)   |

---

## **Example: Configure a single file**

```
<ItemGroup>
  <AnglrFile Include="Grammar/csharp.anglr">
    <Debug>true</Debug>
    <Iterator>true</Iterator>
    <OutputDir>Generated\Csharp</OutputDir>
  </AnglrFile>
</ItemGroup>

```

---

## **Example: Configure multiple files**

```
<ItemGroup>
  <AnglrFile Include="Grammar/math-expression.anglr">
    <Precedence>true</Precedence>
  </AnglrFile>

  <AnglrFile Include="Grammar/very-long-lists.anglr">
    <Iterator>true</Iterator>
  </AnglrFile>
</ItemGroup>

```

---

## **MSBuild Task Reference**

The Anglr MSBuild task is defined as:

```
<Anglr
    SourceFile="@(AnglrFile)"
    Debug="%(AnglrFile.Debug)"
    Tree="%(AnglrFile.Tree)"
    Loop="%(AnglrFile.Loop)"
    Iterator="%(AnglrFile.Iterator)"
    Precedence="%(AnglrFile.Precedence)"
    OutputDir="%(AnglrFile.OutputDir)"
    Code="%(AnglrFile.Code)">
    
    <Output TaskParameter="OutputSourceFileList" ItemName="AnglrGeneratedSource" />
    <Output TaskParameter="OutputLibraryFileList" ItemName="AnglrGeneratedLibrary" />
</Anglr>

```

---

## **Inputs**

| **Property** | **type**    | **Required** | **Description**                                     |
|--------------|-------------|--------------|-----------------------------------------------------|
| `SourceFile` | ITaskItem[] | Yes          | The `.anglr` files to compile                       |
| `Debug`      | bool        | No           | Emit debug info                                     |
| `Tree`       | bool        | No           | Generate parse tree                                 |
| `Loop`       | bool        | No           | Detect parser loops                                 |
| `Iterator`   | bool        | no           | Generate iterative syntax tree visitors             |
| `Precedence` | bool        | No           | Generate precedence grammar                         |
| `OutputDir`  | string      | No           | Name of output directory for generated source files |
| `Code`       | string      | No           | Target language                                     |

---

## **Outputs**

| **Property**            | **Type**    | **Description**                      |
|-------------------------|-------------|--------------------------------------|
| `OutputSourceFileList`  | ITaskItem[] | Generated source files               |
| `OutputLibraryFileList` | ITaskItem[] | Generated Library files (deprecated) |

---

## **Generated Files**

Generated files are placed under:

```
./<OutputDir>
```

if `OutputDir` represents relative path. By the contrary, if `OutputDir` represents absolute path, then generated files are placed under:

```
<OutputDir>
```

---

## **Typical Workflow**

When the package is installed, we proceed with the following procedure:
- Add a .anglr file to the project.
- Enter syntax rules into it
- Build the project. The MSBuild system will automatically invoke the Anglr compiler, which will compile the .anglr file and generate C# source files from it. Generated code contains implementations for the following functionalities:
	- generalized LR parser for syntax rules that are found in a .anglr file.
	- text scanners
	- lexical analyzer.
	- syntax tree builder
	- syntax tree visitors
	- infrastructure that enables semantic analysis
	- a simple program that runs a parser with the appropriate syntax analyzer, creates a syntax tree, and performs a simple recursive traversal through this tree, which demonstrates the basic approach for semantic analysis of the compiled text.
- We can integrate the generated functionalities into the software that is in the project. The basis can be an example of a program generated by the Anglr compiler.

---

## **For a better Experiance**

For working with .anglr files, it is sufficient to install the AnglrMSBuildTasks package, but for a better experience when working with them, it is recommended to also install the **Anglr VSIX extension** (**AnglrLangExtension** from Visual Studio marketplace). You get:
- Syntax highlighting for `.anglr` files
- structured view of .anglr files, especially advantageous for large files
- error detection and reporting
- debugging a syntax analyzer: analysis of the operation of an LR parser

Future versions will also include:
- IntelliSense
- Per‑file property pages

---

## **Example of an `.anglr` file**

```cs

[ Description Text='definitions of tokens and regular expressions used to define syntax']
[ Description Text='of simple arithmetic expressions']
[
	CompilationInfo
		ClassName='MathDecls'
		NameSpace='Math.Declarations'
		Access='public'
]
%declarations mathDecls
%{
	%regex
	{
		decimal-digit [0-9]
		number {decimal-digit}+
		add \+
		sub \-
		mul \*
		div \/
		lb \(
		rb \)
	}

	%terminal
	{
		NUMBER
		add '+'
		sub '-'
		mul '*'
		div '/'
		lb '('
		rb ')'
	}
%}

[ Description Text='definition of scanner, which extracts comments from input string']
[ Declarations Id='mathDecls' ]
[
	CompilationInfo
		ClassName='CommentRegex'
		NameSpace='Math.ScannerLib'
		Access='public'
]
%scanner commentScanner
%{
[\*]+\/
	pop
[\n\r]
	skip
[^\*]+
	skip
[\*]+
	skip
%}

[ Description Text='definition of scanner, which extracts terminal symbols from input string']
[ Declarations Id='mathDecls' ]
[
	CompilationInfo
		ClassName='MathRegex'
		NameSpace='Math.ScannerLib'
		Access='public'
]
%scanner mathScanner
%{
\/\*
	push commentScanner
{number}
	terminal NUMBER
{add}
	terminal add
{sub}
	terminal sub
{mul}
	terminal mul
{div}
	terminal div
{lb}
	terminal lb
{rb}
	terminal rb
[ \t]+
	skip
[\n\r]
	skip
.
	skip
%}

[ Description Text='Lexer for anglr file' Hover='true' ]
[
	UseScanner
		ScannerId='commentScanner'
		InitialScanner='mathScanner'
		Hover='true'
]
[
	CompilationInfo
		ClassName='MathLexer'
		NameSpace='Math.Lexer'
		Access='public'
		Hover='true'
		CodeDir='.'
]
%lexer mathLexer
%{

%}

[ Declarations Id='mathDecls' ]
[ Lexer Id='mathLexer' ]
[
	CompilationInfo
	ClassName='MathParser'
	NameSpace='Math.Parser'
	Access='public'
	CodeDir='mathParser'
]
%parser mathParser
%{

[ Start ]
expression
	:	additive-expression
	;

additive-expression
	:	multiplicative-expression
	|	additive-expression '+' multiplicative-expression
	|	additive-expression '-' multiplicative-expression
	;

multiplicative-expression
	:	unary-expression
	|	multiplicative-expression '*' unary-expression
	|	multiplicative-expression '/' unary-expression
	;

unary-expression
	:	number
	|	'(' expression ')'
	;

number
	:	NUMBER
	|	'+' number
	|	'-' number
	;

%}

```

This file defines the lexical and syntactic analyzer for simple arithmetic expressions: addition, subtraction, multiplication, and division. The syntax is written in such a way that it takes into account the priority of arithmetic operators.

Based on this information, the Anglr compiler generates a set of source C# files, in which the lexical and syntactic analyzer for arithmetic expressions defined in the `.anglr` file are implemented. In addition to the files in which the lexical and syntax analyzers are implemented, there are also files that can be used to implement the semantic analysis of arithmetic expressions.

These files (C# classes contained within them) can also be used to create applications that calculate simple arithmetic expressions.

With the help of the Anglr compiler, it is also possible to generate lexical and syntactic analyzers for all modern programming languages, such as C#, Java, C++, etc. Working examples for C# and Java can be found on the official website for the Anglr compiler.

In `.anglr` files, only anglr source code is located. The source code that is used for semantic analysis (written in C#, Java, C++, ..) is written in separate files. Therefore, the syntactic analyzer defined in this way can be used for various purposes, which we implement by adding a new implementation of semantic rules. We can keep the previous one, allowing us to create applications that serve different purposes. Or we can use the same `.anglr` file to create different applications.
