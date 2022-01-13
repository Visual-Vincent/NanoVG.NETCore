using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using Generator.Extensions;
using Generator.Parsing;

using ArgumentMap = System.Collections.Generic.Dictionary<string, System.Func<Generator.Parsing.TypeDefinition, string>>;

namespace Generator.CodeTranslation.Generators
{
    /// <summary>
    /// Generates P/Invoke declarations.
    /// </summary>
    public class PInvokeGenerator : ICodeGenerator
    {
        private const int IndentAmount = 4;

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

        private static readonly HashSet<string> FunctionsToIgnore = new HashSet<string>() {
            "nvgCreateInternal", "nvgDeleteInternal", "nvgInternalParams"
        };

        private static readonly Dictionary<string, ArgumentMap> FunctionArgumentOverrides = new Dictionary<string, ArgumentMap>()  {
        //  { FuncName,                                    {{ ArgName,              ArgType    }} }
            { "nvgCurrentTransform",     new ArgumentMap() {{ "xform",     type => "float[]"   }} },
            { "nvgTransformIdentity",    new ArgumentMap() {{ "dst",       type => "float[]"   }} },
            { "nvgTransformTranslate",   new ArgumentMap() {{ "dst",       type => "float[]"   }} },
            { "nvgTransformScale",       new ArgumentMap() {{ "dst",       type => "float[]"   }} },
            { "nvgTransformRotate",      new ArgumentMap() {{ "dst",       type => "float[]"   }} },
            { "nvgTransformSkewX",       new ArgumentMap() {{ "dst",       type => "float[]"   }} },
            { "nvgTransformSkewY",       new ArgumentMap() {{ "dst",       type => "float[]"   }} },
            { "nvgTransformMultiply",    new ArgumentMap() {{ "dst",       type => "float[]"   }, { "src",       type => "float[]"   }} },
            { "nvgTransformPremultiply", new ArgumentMap() {{ "dst",       type => "float[]"   }, { "src",       type => "float[]"   }} },
            { "nvgTransformInverse",     new ArgumentMap() {{ "dst",       type => "float[]"   }, { "src",       type => "float[]"   }} },
            { "nvgTransformPoint",       new ArgumentMap() {{ "dstx",      type => "float[]"   }, { "dsty",      type => "float[]"   }, { "xform", type => "float[]"   }} },
            { "nvgImageSize",            new ArgumentMap() {{ "w",         type => "out int"   }, { "h",         type => "out int"   }} },
            { "nvgTextMetrics",          new ArgumentMap() {{ "ascender",  type => "out float" }, { "descender", type => "out float" }, { "lineh", type => "out float" }} },
            { "nvgTextBounds",           new ArgumentMap() {{ "bounds",    type => "float[]"   }} },
            { "nvgTextBoxBounds",        new ArgumentMap() {{ "bounds",    type => "float[]"   }} },
            { "nvgTextGlyphPositions",   new ArgumentMap() {{ "positions", type => type.Pointer == PointerType.Standard ? $"{type.Name}[]" : null }} },
            { "nvgTextBreakLines",       new ArgumentMap() {{ "rows",      type => type.Pointer == PointerType.Standard ? $"{type.Name}[]" : null }} },
        };

        private string libraryName;
        private string namespaceName;
        private string className;
        private string functionPrefix;

        private bool partialClass;

        private IndentationHelper indentation = new IndentationHelper(' ');

        /// <summary>
        /// Initializes a new instance of the <see cref="PInvokeGenerator"/> class.
        /// </summary>
        /// <param name="libraryName">The name/path to the library to put in the DllImport header</param>
        /// <param name="namespaceName">The namespace that the P/Invoke definitions will reside in.</param>
        /// <param name="className">The name of the static class that the P/Invoke definitions will reside in.</param>
        /// <param name="functionPrefix">Optional. A common function prefix that may be omitted from the P/Invoke declarations.</param>
        /// <param name="partialClass">Optional. Whether or not to generate a partial class definition.</param>
        public PInvokeGenerator(string libraryName, string namespaceName, string className, string functionPrefix = null, bool partialClass = false)
        {
            if(string.IsNullOrWhiteSpace(libraryName))
                throw new ArgumentNullException(nameof(libraryName));

            if(!IdentifierRegex.IsMatch(namespaceName))
                throw new ArgumentException($"Invalid characters in namespace: {namespaceName}");

            if(!IdentifierRegex.IsMatch(className))
                throw new ArgumentException($"Invalid characters in class name: {className}");

            if(!string.IsNullOrEmpty(functionPrefix) && !IdentifierRegex.IsMatch(functionPrefix))
                throw new ArgumentException($"Invalid characters in function prefix: {functionPrefix}");

            this.libraryName = libraryName;
            this.namespaceName = namespaceName;
            this.className = className;
            this.functionPrefix = functionPrefix;
            this.partialClass = partialClass;
        }

        /// <inheritdoc/>
        public string GenerateStart()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(indentation, "// ");
            builder.AppendLine(indentation, "// AUTO-GENERATED CODE");
            builder.AppendLine(indentation, "// ");
            builder.AppendLine(indentation);
            builder.AppendLine(indentation, "using System;");
            builder.AppendLine(indentation, "using System.Text;");
            builder.AppendLine(indentation, "using System.Runtime.InteropServices;");
            builder.AppendLine(indentation);
            builder.AppendLine(indentation, $"namespace {namespaceName}");
            builder.AppendLine(indentation, "{");

            indentation.Indent(IndentAmount);

            return builder.ToString();
        }

        /// <inheritdoc/>
        public string GenerateEnd()
        {
            StringBuilder builder = new StringBuilder();

            indentation.Indent(-IndentAmount);
            builder.AppendLine(indentation, "}");

            return builder.ToString();
        }

        /// <inheritdoc/>
        public string GenerateEnum(EnumDefinition @enum, int index, int count)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(indentation, $"public enum {ConvertName(@enum.Name)}");
            builder.AppendLine(indentation, @"{");

            indentation.Indent(IndentAmount);

            int longestValueName = @enum.Values.Max(kvp => kvp.Key.Length) + 1;

            foreach(var kvp in @enum.Values)
            {
                string name = ConvertName(kvp.Key);

                if(!string.IsNullOrEmpty(kvp.Value))
                    builder.AppendLine(indentation, $"{name}".PadRight(longestValueName) + $"= {kvp.Value},");
                else
                    builder.AppendLine(indentation, $"{name},");
            }

            indentation.Indent(-IndentAmount);

            builder.AppendLine(indentation, @"}");
            builder.AppendLine(indentation);

            return builder.ToString();
        }

        /// <inheritdoc/>
        public string GenerateFunction(FunctionDefinition function, int index, int count)
        {
            if(FunctionsToIgnore.Contains(function.Name))
                return null;

            StringBuilder builder = new StringBuilder();

            bool hasFunctionPrefix = !string.IsNullOrWhiteSpace(functionPrefix);

            if(index == 0)
            {
                builder.AppendLine(indentation, $"public static{(partialClass ? " partial" : "")} class {className}");
                builder.AppendLine(indentation, @"{");
                indentation.Indent(IndentAmount);

                builder.AppendLine(indentation, $"public const string LibraryName = \"{libraryName}\";");

                if(hasFunctionPrefix)
                    builder.AppendLine(indentation, $"public const string FunctionPrefix = \"{functionPrefix}\";");

                builder.AppendLine(indentation);
            }

            if(!string.IsNullOrWhiteSpace(function.Comment))
            {
                string comment = SecurityElement.Escape(function.Comment.Trim());
                comment = CommentLinebreakRegex.Replace(comment, $" <br/>{Environment.NewLine}{indentation}/// ");

                builder.AppendLine(indentation, $"/// <summary>");
                builder.AppendLine(indentation, $"/// {comment}");
                builder.AppendLine(indentation, $"/// </summary>");
            }

            string funcName = function.Name;

            if(hasFunctionPrefix && funcName.StartsWith(functionPrefix))
            {
                funcName = funcName.Substring(functionPrefix.Length);
                funcName = ConvertName(funcName);
                builder.AppendLine(indentation, $"[DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof({funcName}))]");
            }
            else
            {
                funcName = ConvertName(funcName);
                builder.AppendLine(indentation, $"[DllImport(LibraryName)]");
            }

            string returnType = ConvertType(function.ReturnType);

            if(returnType == "string" || returnType == "StringBuilder")
            {
                builder.AppendLine(indentation, "[return: MarshalAs(UnmanagedType.LPUTF8Str)]");
            }

            builder.Append(indentation, $"public static extern {returnType} {funcName}");
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

            if(index < count - 1)
                builder.AppendLine(indentation);

            if(index == count - 1)
            {
                indentation.Indent(-IndentAmount);
                builder.AppendLine(indentation, "}");
            }

            return builder.ToString();
        }

        /// <inheritdoc/>
        public string GenerateStruct(StructDefinition @struct, int index, int count)
        {
            StringBuilder builder = new StringBuilder();

            bool isUnsafe = @struct.Fields.Any(field =>
                field.Type.Pointer != PointerType.None || (field.Type.IsArray && !string.IsNullOrWhiteSpace(field.Type.ArrayBounds))
            );

            builder.AppendLine(indentation, $"[StructLayout(LayoutKind.Sequential, Pack = 1)]");
            builder.AppendLine(indentation, $"public{(isUnsafe ? " unsafe" : "")} struct {ConvertName(@struct.Name)}");
            builder.AppendLine(indentation, @"{");

            indentation.Indent(IndentAmount);

            foreach(var field in @struct.Fields)
            {
                bool isArray = field.Type.IsArray;
                bool isFixed = isArray && !string.IsNullOrWhiteSpace(field.Type.ArrayBounds);

                string name = ConvertName(field.Name);
                string type = ConvertType(field.Type, false);
                string bounds = isArray ? $"[{field.Type.ArrayBounds}]" : "";

                builder.Append(indentation);

                if(type == "string" || type == "StringBuilder")
                {
                    builder.Append("[MarshalAs(UnmanagedType.LPUTF8Str)] ");
                }

                builder.AppendLine($"public{(isFixed ? " fixed" : "")} {type} {name}{bounds};");
            }

            indentation.Indent(-IndentAmount);

            builder.AppendLine(indentation, @"}");
            builder.AppendLine(indentation);

            return builder.ToString();
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

            if(FunctionArgumentOverrides.TryGetValue(function.Name, out var map) && map.TryGetValue(argument.Name, out var argOverride))
            {
                var newType = argOverride(type);
                if(newType != null)
                    return newType;
            }

            return ConvertType(type, arraySuffix);
        }
    }
}
