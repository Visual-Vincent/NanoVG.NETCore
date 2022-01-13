using System;

namespace Generator.Parsing
{
    /// <summary>
    /// Defines methods for parsing source code.
    /// </summary>
    public interface ICodeParser
    {
        /// <summary>
        /// Parses the enums in the specified code.
        /// </summary>
        /// <param name="sourceCode">The code to parse.</param>
        EnumDefinition[] ParseEnums(string sourceCode);

        /// <summary>
        /// Parses the functions in the specified code.
        /// </summary>
        /// <param name="sourceCode">The code to parse.</param>
        FunctionDefinition[] ParseFunctions(string sourceCode);

        /// <summary>
        /// Parses the structs in the specified code.
        /// </summary>
        /// <param name="sourceCode">The code to parse.</param>
        StructDefinition[] ParseStructs(string sourceCode);

        /// <summary>
        /// Removes all comments from the specified code.
        /// </summary>
        /// <param name="sourceCode">The code to modify.</param>
        string RemoveComments(string sourceCode);
    }
}
