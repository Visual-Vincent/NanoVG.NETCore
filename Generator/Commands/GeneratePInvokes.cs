using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using CommandLine;

namespace Generator.Commands
{
    [Verb("pinvoke", HelpText = "Parses the specified C file(s) and generates their respective P/Invoke signatures")]
    public class GeneratePInvokes : Command
    {
        private static readonly Regex IdentifierRegex = new Regex(@"\A[a-z_][a-z0-9_]*\z", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
        private static readonly Regex CommentLinebreakRegex = new Regex(@"(?:\r?\n|\r)[ \t]*", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

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

        [Option("funcprefix", Required = false, HelpText = "A common function prefix that may be omitted from the P/Invoke declarations")]
        public string FunctionPrefix { get; set; }

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
                List<EnumDefinition> enumList = new List<EnumDefinition>();
                List<StructDefinition> structList = new List<StructDefinition>();

                foreach(string file in Files)
                {
                    string contents = File.ReadAllText(file);

                    var functions = parser.ParseFunctions(contents);
                    functionList.AddRange(functions);

                    var structs = parser.ParseStructs(contents);
                    structList.AddRange(structs);

                    var enums = parser.ParseEnums(contents);
                    enumList.AddRange(enums);
                }

                string outputFile = Path.GetFullPath(Output);
                string outputPath = Path.GetDirectoryName(outputFile);

                if(Directory.Exists(outputFile))
                    throw new IOException($"Output path \"{outputFile}\" is a directory");

                if(!Directory.Exists(outputFile))
                    Directory.CreateDirectory(outputPath);

                StringBuilder builder = new StringBuilder();
                builder.AppendLine("// ");
                builder.AppendLine("// AUTO-GENERATED CODE");
                builder.AppendLine("// ");
                builder.AppendLine();
                builder.AppendLine("using System;");
                builder.AppendLine("using System.Text;");
                builder.AppendLine("using System.Runtime.InteropServices;");
                builder.AppendLine();
                builder.AppendLine($"namespace {Namespace}");
                builder.AppendLine("{");

                // Structs
                // -----------------------------------------------------------------------------------------------------------------

                foreach(var @struct in structList)
                {
                    bool isUnsafe = @struct.Fields.Any(field => 
                        field.Type.Pointer != PointerType.None || (field.Type.IsArray && !string.IsNullOrWhiteSpace(field.Type.ArrayBounds))
                    );

                    builder.AppendLine($"    [StructLayout(LayoutKind.Sequential, Pack = 1)]");
                    builder.AppendLine($"    public{(isUnsafe ? " unsafe" : "")} struct {ConvertName(@struct.Name)}");
                    builder.AppendLine(@"    {");

                    foreach(var field in @struct.Fields)
                    {
                        bool isArray = field.Type.IsArray;
                        bool isFixed = isArray && !string.IsNullOrWhiteSpace(field.Type.ArrayBounds);

                        string name = ConvertName(field.Name);
                        string type = ConvertType(field.Type, false);
                        string bounds = isArray ? $"[{field.Type.ArrayBounds}]" : "";

                        builder.Append("        ");

                        if(type == "string" || type == "StringBuilder")
                        {
                            builder.Append("[MarshalAs(UnmanagedType.LPUTF8Str)] ");
                        }

                        builder.AppendLine($"public{(isFixed ? " fixed" : "")} {type} {name}{bounds};");
                    }

                    builder.AppendLine(@"    }");
                    builder.AppendLine();
                }

                // Enums
                // -----------------------------------------------------------------------------------------------------------------

                foreach(var @enum in enumList)
                {
                    builder.AppendLine($"    public enum {ConvertName(@enum.Name)}");
                    builder.AppendLine(@"    {");

                    int longestValueName = @enum.Values.Max(kvp => kvp.Key.Length) + 8 + 1; // 12 = indentation

                    foreach(var kvp in @enum.Values)
                    {
                        string name = ConvertName(kvp.Key);

                        if(!string.IsNullOrEmpty(kvp.Value))
                            builder.AppendLine($"        {name}".PadRight(longestValueName) + $"= {kvp.Value},");
                        else
                            builder.AppendLine($"        {name},");
                    }

                    builder.AppendLine(@"    }");
                    builder.AppendLine();
                }

                // Functions
                // -----------------------------------------------------------------------------------------------------------------

                bool hasFunctionPrefix = !string.IsNullOrWhiteSpace(FunctionPrefix);

                builder.AppendLine($"    public static unsafe{(PartialClass ? " partial" : "")} class {ClassName}");
                builder.AppendLine(@"    {");
                builder.AppendLine($"        public const string LibraryName = \"{LibraryName}\";");

                if(hasFunctionPrefix)
                    builder.AppendLine($"        public const string FunctionPrefix = \"{FunctionPrefix}\";");

                builder.AppendLine();

                foreach(var function in functionList)
                {
                    if(!string.IsNullOrWhiteSpace(function.Comment))
                    {
                        string comment = SecurityElement.Escape(function.Comment.Trim());
                        comment = CommentLinebreakRegex.Replace(comment, $" <br/>{Environment.NewLine}        /// ");

                        builder.AppendLine($"        /// <summary>");
                        builder.AppendLine($"        /// {comment}");
                        builder.AppendLine($"        /// </summary>");
                    }

                    string funcName = function.Name;

                    if(hasFunctionPrefix && funcName.StartsWith(FunctionPrefix))
                    {
                        funcName = funcName.Substring(FunctionPrefix.Length);
                        funcName = ConvertName(funcName);
                        builder.AppendLine($"        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof({funcName}))]");
                    }
                    else
                    {
                        funcName = ConvertName(funcName);
                        builder.AppendLine($"        [DllImport(LibraryName)]");
                    }

                    string returnType = ConvertType(function.ReturnType);

                    if(returnType == "string" || returnType == "StringBuilder")
                    {
                        builder.AppendLine("        [return: MarshalAs(UnmanagedType.LPUTF8Str)]");
                    }

                    builder.Append($"        public static extern {returnType} {funcName}");
                    builder.Append("(");

                    int args = function.Arguments.Count;
                    for(int i = 0; i < args; i++)
                    {
                        var argument = function.Arguments[i];
                        string name = ConvertName(argument.Name);
                        string type = ConvertArgumentType(function, argument);

                        if(type == "string" || type == "StringBuilder")
                        {
                            builder.Append("[MarshalAs(UnmanagedType.LPUTF8Str)] ");
                        }

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

                Environment.ExitCode = 1;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Failed to generate P/Invoke definitions:");
                Console.WriteLine($"  {ex}");

                Environment.ExitCode = 1;
            }
        }

        private static string ConvertName(string name)
        {
            if(ReservedKeywords.Contains(name))
                return $"@{name}";

            return name;
        }

        private static string ConvertType(TypeDefinition type, bool arraySuffix = true)
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

            if(type.Name.Equals("NVGcontext", StringComparison.OrdinalIgnoreCase))
                return type.Name; // Get rid of pointers

            if(type.Name == "GLuint")
            {
                if(type.IsArray)
                    type = new TypeDefinition("int", type.Flags | TypeFlags.Unsigned, type.ArrayBounds);
                else
                    type = new TypeDefinition("int", type.Flags | TypeFlags.Unsigned, type.Pointer);
            }

            return type.ToString(TypePrefixes, arraySuffix);
        }

        // Handles special cases
        private static string ConvertArgumentType(FunctionDefinition function, ArgumentDefinition argument, bool arraySuffix = true)
        {
            TypeDefinition type = argument.Type;

            if(type.Name.Equals("NVGcontext", StringComparison.OrdinalIgnoreCase))
                return $"this {type.Name}";

            if(function.Name == "nvgTextGlyphPositions" && argument.Name == "positions" && type.Pointer == PointerType.Standard)
                return $"{type.Name}[]";

            if(function.Name == "nvgTextBreakLines" && argument.Name == "rows" && type.Pointer == PointerType.Standard)
                return $"{type.Name}[]";

            return ConvertType(type, arraySuffix);
        }
    }
}
