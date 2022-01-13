using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Generator.Parsing.Languages
{
    /// <summary>
    /// Provides functionality for parsing simple C code.
    /// </summary>
    /// <remarks>The parser is very basic. Complex code may generate invalid definitions.</remarks>
    public class ClangParser : ICodeParser
    {
        private const RegexOptions REGEX_OPTIONS = RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase;

        private const string IdentifierNamePattern = @"[a-z_][a-z0-9_]*";
        private const string PointersPattern = @"(?:\*{1,2}|\&)";
        private const string CommentPattern = @"^\s*//[^\r\n]*\r?\n|[\t ]*//[^\r\n]*|[\t ]*/\*(?:.|[\r\n])+?\*/";
        private const string FuncCommentPattern = @"(?<comment>(?:[\t ]*//[^\r\n]*\r?\n)*)";
        private const string TypePattern = @"(?<typeprefix>(?:const\s+)?(?:unsigned\s+)?)(?<type>"+IdentifierNamePattern+@"(?:\s+|\s+"+PointersPattern+"|"+PointersPattern+@"\s+|\s+"+PointersPattern+@"\s+))";
        private const string FuncBodyPattern = @"(?:\;|\{(?:[^\{\}]|(?<open>\{)|(?<-open>\}))+(?(open)(?!))\})";
        private const string ArrayBoundsPattern = @"(?:\[(?<arraybounds>[^\]]*)\])";

        private static readonly Regex ArgumentRegex = new Regex($@"{TypePattern}(?<name>{IdentifierNamePattern})\s*{ArrayBoundsPattern}?\s*", REGEX_OPTIONS);
        private static readonly Regex CommentRegex = new Regex(CommentPattern, REGEX_OPTIONS | RegexOptions.Multiline);
        private static readonly Regex FunctionRegex = new Regex($@"{FuncCommentPattern}(?<=^|\r|\n)[a-z0-9_\t ]*?{TypePattern}(?<name>{IdentifierNamePattern})\s*\((?<args>[^)]+)?\)\s*{FuncBodyPattern}", REGEX_OPTIONS);
        private static readonly Regex WhitespaceRegex = new Regex(@"\s", REGEX_OPTIONS);

        private static readonly Regex EnumRegex = new Regex($@"enum\s+(?<name>{IdentifierNamePattern})\s*\{{(?<body>[^{{}}]+)\}}", REGEX_OPTIONS);
        private static readonly Regex EnumValueRegex = new Regex($@"(?<name>{IdentifierNamePattern})(?:\s*=\s*(?<value>[a-z0-9_<>*+\-/^()\s]+))?,?", REGEX_OPTIONS);
        
        private static readonly Regex StructRegex = new Regex($@"struct\s+(?<name>{IdentifierNamePattern})\s*(?<body>{FuncBodyPattern})", REGEX_OPTIONS);
        private static readonly Regex StructFieldRegex = new Regex($@"{TypePattern}(?<name>{IdentifierNamePattern}(?:,\s*{IdentifierNamePattern})*)\s*{ArrayBoundsPattern}?\s*;", REGEX_OPTIONS);

        /// <summary>
        /// Initializes a new instance of the <see cref="ClangParser"/> class.
        /// </summary>
        public ClangParser()
        {
        }

        private TypeDefinition ConvertToTypeDefinition(Match match)
        {
            string type = match.Groups["type"].Value;
            string prefix = match.Groups["typeprefix"].Value ?? "";

            type = WhitespaceRegex.Replace(type, "");

            TypeFlags flags = TypeFlags.None;

            if(prefix.Contains("const "))
                flags |= TypeFlags.Const;
            if(prefix.Contains("unsigned "))
                flags |= TypeFlags.Unsigned;

            if(match.Groups.ContainsKey("arraybounds") && match.Groups["arraybounds"].Success)
            {
                string bounds = match.Groups["arraybounds"].Value ?? "";
                return new TypeDefinition(type, flags, bounds.Trim());
            }

            return new TypeDefinition(type, flags);
        }

        /// <inheritdoc/>
        public EnumDefinition[] ParseEnums(string sourceCode)
        {
            List<EnumDefinition> enums = new List<EnumDefinition>();

            sourceCode = RemoveComments(sourceCode);
            var matches = EnumRegex.Matches(sourceCode);

            foreach(Match match in matches)
            {
                string name = match.Groups["name"].Value;
                string body = match.Groups["body"].Value;
                EnumDefinition @enum = new EnumDefinition(name);

                var valueMatches = EnumValueRegex.Matches(body);

                foreach(Match valueMatch in valueMatches)
                {
                    string valueName = valueMatch.Groups["name"].Value;
                    string value = valueMatch.Groups["value"].Value;

                    @enum.Values.Add(new KeyValuePair<string, string>(valueName, value));
                }

                enums.Add(@enum);
            }

            return enums.ToArray();
        }

        /// <inheritdoc/>
        public FunctionDefinition[] ParseFunctions(string sourceCode)
        {
            return ParseFunctions(sourceCode, true);
        }

        /// <summary>
        /// Parses the functions in the specified code.
        /// </summary>
        /// <param name="sourceCode">The code to parse.</param>
        /// <param name="parseComments">Whether or not to include function comments.</param>
        public FunctionDefinition[] ParseFunctions(string sourceCode, bool parseComments = true)
        {
            List<FunctionDefinition> functions = new List<FunctionDefinition>();

            if(!parseComments)
                sourceCode = RemoveComments(sourceCode);

            var matches = FunctionRegex.Matches(sourceCode);

            foreach(Match match in matches)
            {
                string comment = null;

                if(parseComments)
                {
                    StringBuilder commentBuilder = new StringBuilder();
                    comment = match.Groups["comment"].Value;

                    foreach(string line in comment.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
                        commentBuilder.AppendLine(line.Trim().Replace("//", ""));

                    comment = commentBuilder.ToString();
                }

                string funcName = match.Groups["name"].Value;
                string returnType = match.Groups["type"].Value;
                string typePrefix = match.Groups["typeprefix"].Value ?? "";
                string args = match.Groups["args"].Value;

                returnType = WhitespaceRegex.Replace(returnType, "");

                // We've encountered a return statement which calls a method.
                if(returnType == "return")
                    continue;

                TypeFlags typeFlags = TypeFlags.None;

                if(typePrefix.Contains("const "))
                    typeFlags |= TypeFlags.Const;
                if(typePrefix.Contains("unsigned "))
                    typeFlags |= TypeFlags.Unsigned;

                TypeDefinition retType = new TypeDefinition(returnType, typeFlags);
                FunctionDefinition definition = new FunctionDefinition(funcName, retType, comment);

                int i = 1;
                foreach(string argDef in args.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    Match argMatch = ArgumentRegex.Match(argDef);

                    if(!argMatch.Success)
                        throw new Exception($"Failed to parse argument {i} of function {funcName}");

                    string argName = argMatch.Groups["name"].Value;
                    TypeDefinition type = ConvertToTypeDefinition(argMatch);

                    definition.Arguments.Add(new ArgumentDefinition(argName, type));
                    i++;
                }

                functions.Add(definition);
            }

            return functions.ToArray();
        }

        /// <inheritdoc/>
        public StructDefinition[] ParseStructs(string sourceCode)
        {
            List<StructDefinition> structs = new List<StructDefinition>();

            sourceCode = RemoveComments(sourceCode);
            var matches = StructRegex.Matches(sourceCode);

            foreach(Match match in matches)
            {
                string name = match.Groups["name"].Value;
                string body = match.Groups["body"].Value;
                StructDefinition @struct = new StructDefinition(name);

                // Parsing empty or union structs is not supported.
                if(body.Length <= 0 || body.Contains("union"))
                    continue;

                var fieldMatches = StructFieldRegex.Matches(body);

                foreach(Match fieldMatch in fieldMatches)
                {
                    string fieldName = fieldMatch.Groups["name"].Value;
                    TypeDefinition type = ConvertToTypeDefinition(fieldMatch);

                    @struct.Fields.Add(new FieldDefinition(fieldName, type));
                }

                structs.Add(@struct);
            }

            return structs.ToArray();
        }

        /// <inheritdoc/>
        public string RemoveComments(string sourceCode)
        {
            return CommentRegex.Replace(sourceCode, "");
        }
    }
}
