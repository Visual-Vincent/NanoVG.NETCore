using System;
using System.Collections.Generic;
using System.Text;
using Generator.Parsing;

namespace Generator.CodeTranslation
{
    /// <summary>
    /// A class for translating source code from one language/format to another.
    /// </summary>
    public static class CodeTranslator
    {
        /// <summary>
        /// Translates the specified source code from one language/format to another.
        /// </summary>
        /// <param name="sourceCode">The source code to translate.</param>
        /// <param name="parser">The <see cref="ICodeParser"/> to parse the source code with.</param>
        /// <param name="generator">The <see cref="ICodeGenerator"/> to generate the new source code with.</param>
        public static string Translate(string sourceCode, ICodeParser parser, ICodeGenerator generator)
        {
            return Translate(new string[] { sourceCode }, parser, generator);
        }

        /// <summary>
        /// Translates the specified source code from one language/format to another.
        /// </summary>
        /// <param name="sourceCode">The array of source code strings to translate.</param>
        /// <param name="parser">The <see cref="ICodeParser"/> to parse the source code with.</param>
        /// <param name="generator">The <see cref="ICodeGenerator"/> to generate the source code with.</param>
        public static string Translate(string[] sourceCode, ICodeParser parser, ICodeGenerator generator)
        {
            if(sourceCode == null)
                throw new ArgumentNullException(nameof(sourceCode));

            if(parser == null)
                throw new ArgumentNullException(nameof(parser));

            if(generator == null)
                throw new ArgumentNullException(nameof(generator));

            List<FunctionDefinition> functionList = new List<FunctionDefinition>();
            List<EnumDefinition> enumList = new List<EnumDefinition>();
            List<StructDefinition> structList = new List<StructDefinition>();

            int i = 0;
            int c = 0;
            string snippet;
            StringBuilder builder = new StringBuilder();

            foreach(string code in sourceCode)
            {
                var functions = parser.ParseFunctions(code);
                functionList.AddRange(functions);

                var structs = parser.ParseStructs(code);
                structList.AddRange(structs);

                var enums = parser.ParseEnums(code);
                enumList.AddRange(enums);
            }

            // Start of file
            // ------------------------------------------------------------------

            snippet = generator.GenerateStart();

            if(!string.IsNullOrEmpty(snippet))
                builder.Append(snippet);

            // Structs
            // ------------------------------------------------------------------

            i = 0;
            c = structList.Count;

            foreach(var @struct in structList)
            {
                snippet = generator.GenerateStruct(@struct, i, c);

                if(!string.IsNullOrEmpty(snippet))
                    builder.Append(snippet);

                i++;
            }

            // Enums
            // ------------------------------------------------------------------

            i = 0;
            c = enumList.Count;

            foreach(var @enum in enumList)
            {
                snippet = generator.GenerateEnum(@enum, i, c);

                if(!string.IsNullOrEmpty(snippet))
                    builder.Append(snippet);

                i++;
            }

            // Functions
            // ------------------------------------------------------------------

            i = 0;
            c = functionList.Count;

            foreach(var function in functionList)
            {
                snippet = generator.GenerateFunction(function, i, c);

                if(!string.IsNullOrEmpty(snippet))
                    builder.Append(snippet);

                i++;
            }

            // End of file
            // ------------------------------------------------------------------

            snippet = generator.GenerateEnd();

            if(!string.IsNullOrEmpty(snippet))
                builder.Append(snippet);

            return builder.ToString();
        }
    }
}
