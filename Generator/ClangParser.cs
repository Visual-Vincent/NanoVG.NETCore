using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Generator
{
    /// <summary>
    /// Provides functionality for parsing simple C/C++ code.
    /// </summary>
    public class ClangParser
    {
        private const RegexOptions REGEX_OPTIONS = RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase;

        private const string IdentifierNamePattern = @"[a-z0-9_]+";
        private const string PointersPattern = @"(?:\*{1,2}|\&)";
        private const string CommentPattern = @"(?<comment>(?:[\t ]*//[^\r\n]*\r?\n)*)";
        private const string TypePattern = @"(?<type>"+IdentifierNamePattern+@"(?:\s+|\s+"+PointersPattern+"|"+PointersPattern+@"\s+|\s+"+PointersPattern+@"\s+))";
        private const string FuncBodyPattern = @"(?:\;|\{(?:[^\{\}]|(?<open>\{)|(?<-open>\}))+(?(open)(?!))\})";

        private static readonly Regex ArgumentRegex = new Regex($@"{TypePattern}(?<argName>{IdentifierNamePattern})", REGEX_OPTIONS);
        private static readonly Regex FunctionRegex = new Regex($@"{CommentPattern}(?<=^|\r|\n)[a-z0-9_\t ]*?{TypePattern}(?<name>{IdentifierNamePattern})\s*\((?<args>[^)]+)?\)\s*{FuncBodyPattern}", REGEX_OPTIONS);
        private static readonly Regex WhitespaceRegex = new Regex(@"\s", REGEX_OPTIONS);

        /// <summary>
        /// Initializes a new instance of the <see cref="ClangParser"/> class.
        /// </summary>
        public ClangParser()
        {
        }

        /// <summary>
        /// Parses the functions in the specified code. 
        /// NOTE: The parser is very basic. Complex code may generate invalid function definitions.
        /// </summary>
        /// <param name="sourceCode">The code to parse.</param>
        /// <param name="parseComments">Whether or not to include function comments.</param>
        public FunctionDefinition[] ParseFunctions(string sourceCode, bool parseComments = true)
        {
            List<FunctionDefinition> functions = new List<FunctionDefinition>();

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
                string args = match.Groups["args"].Value;

                returnType = WhitespaceRegex.Replace(returnType, "");

                // We've encountered a return statement which calls a method.
                if(returnType == "return")
                    continue;

                FunctionDefinition definition = new FunctionDefinition(funcName, returnType, comment);

                int i = 1;
                foreach(string argDef in args.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    Match argMatch = ArgumentRegex.Match(argDef);

                    if(!argMatch.Success)
                        throw new Exception($"Failed to parse argument {i} of function {funcName}");

                    string argName = argMatch.Groups["name"].Value;
                    string argType = argMatch.Groups["type"].Value;

                    argType = WhitespaceRegex.Replace(argType, "");
                    definition.Arguments.Add(new ArgumentDefinition(argName, argType));

                    i++;
                }

                functions.Add(definition);
            }

            return functions.ToArray();
        }
    }
}
