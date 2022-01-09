using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using CommandLine;

namespace Generator.Commands
{
    [Verb("pinvoke", HelpText = "Parses the specified C file(s) and generates their respective P/Invoke signatures")]
    public class GeneratePInvokes : Command
    {
        private static readonly Regex IdentifierRegex = new Regex(@"\A[a-z_][a-z0-9_]*\z", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        private static readonly Dictionary<TypeFlags, string> TypePrefixes = new Dictionary<TypeFlags, string>() {
            { TypeFlags.Const, "" },
            { TypeFlags.Unsigned, "u" }
        };

        [Option('f', "files", Required = true, HelpText = "The C file(s) to parse for function definitions")]
        public IEnumerable<string> Files { get; set; }

        [Option('l', "library", Required = true, HelpText = "The name/path to the library to put in the DllImport header")]
        public string LibraryName { get; set; }

        [Option('n', "namespace", Required = true, HelpText = "The namespace that the P/Invoke definitions will reside in")]
        public string Namespace { get; set; }

        [Option('c', "class", Required = true, HelpText = "The name of the static class that the P/Invoke definitions will reside in")]
        public string ClassName { get; set; }

        [Option('o', "output", Required = true, HelpText = "The output the file to write the generated code to")]
        public string Output { get; set; }

        [Option("partial-class", Required = false, HelpText = "Whether or not to generate a partial class definition")]
        public bool PartialClass { get; set; }

        public override void Execute()
        {
            try
            {
                if(!IdentifierRegex.IsMatch(Namespace))
                    throw new ArgumentException($"Invalid characters in namespace: {Namespace}");

                if(!IdentifierRegex.IsMatch(ClassName))
                    throw new ArgumentException($"Invalid characters in class name: {ClassName}");

                ClangParser parser = new ClangParser();
                List<FunctionDefinition> functionList = new List<FunctionDefinition>();

                foreach(string file in Files)
                {
                    string contents = File.ReadAllText(file);
                    var functions = parser.ParseFunctions(contents, false);
                    functionList.AddRange(functions);
                }

                string outputFile = Path.GetFullPath(Output);
                string outputPath = Path.GetDirectoryName(outputFile);

                if(Directory.Exists(outputFile))
                    throw new IOException($"Output path \"{outputFile}\" is a directory");

                if(!Directory.Exists(outputFile))
                    Directory.CreateDirectory(outputPath);

                StringBuilder builder = new StringBuilder();
                builder.AppendLine("using System;");
                builder.AppendLine("using System.Text;");
                builder.AppendLine("using System.Runtime.InteropServices;");
                builder.AppendLine();
                builder.AppendLine($"namespace {Namespace}");
                builder.AppendLine("{");
                builder.AppendLine($"    public unsafe static{(PartialClass ? " partial" : "")} class {ClassName}");
                builder.AppendLine(@"    {");
                builder.AppendLine($"        public const string LibraryName = \"{LibraryName}\";");
                builder.AppendLine();

                foreach(var function in functionList)
                {
                    builder.AppendLine($"        [DllImport(LibraryName)]");
                    builder.Append($"        public static extern {ConvertType(function.ReturnType)} {ConvertName(function.Name)}");
                    builder.Append("(");

                    int args = function.Arguments.Count;
                    for(int i = 0; i < args; i++)
                    {
                        var argument = function.Arguments[i];
                        string name = ConvertName(argument.Name);
                        string type = ConvertType(argument.Type);

                        builder.Append($"{type} {name}");

                        if(i < args - 1)
                            builder.Append(", ");
                    }

                    builder.AppendLine(");");
                    builder.AppendLine();
                }

                builder.AppendLine("    }");
                builder.AppendLine("}");

                File.WriteAllText(outputFile, builder.ToString());
            }
            catch(FileNotFoundException ex)
            {
                Console.WriteLine("Failed to generate P/Invoke definitions:");
                Console.WriteLine($"  Could not find file: {ex.FileName}");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Failed to generate P/Invoke definitions:");
                Console.WriteLine($"  {ex}");
            }
        }

        private static string ConvertName(string name)
        {
            if(ReservedKeywords.Contains(name))
                return $"@{name}";

            return name;
        }

        private static string ConvertType(TypeDefinition type)
        {
            if(type.Name == "char" && type.Flags.HasFlag(TypeFlags.Unsigned))
            {
                if(type.Pointer == PointerType.Standard)
                    return "byte[]";
                else
                    return "byte";
            }

            if(type.Name == "char" && type.Pointer == PointerType.Standard)
            {
                if(type.Flags.HasFlag(TypeFlags.Const))
                    return "string";
                else
                    return "StringBuilder";
            }

            return type.ToString(TypePrefixes);
        }

        private static readonly HashSet<string> ReservedKeywords = new HashSet<string>() {
            "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", "class", 
            "const", "continue", "decimal", "default", "delegate", "do", "double", "else", "enum", "event", 
            "explicit", "extern", "false", "finally", "fixed", "float", "for", "foreach", "goto", "if", 
            "implicit", "in", "int", "interface", "internal", "is", "lock", "long", "namespace", "new", 
            "null", "object", "operator", "out", "override", "params", "private", "protected", "public", 
            "readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof", "stackalloc", "static", "string", 
            "struct", "switch", "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe", 
            "ushort", "using", "virtual", "void", "volatile", "while"
        };
    }
}
