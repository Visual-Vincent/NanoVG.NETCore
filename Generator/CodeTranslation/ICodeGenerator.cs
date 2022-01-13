using System;
using Generator.Parsing;

namespace Generator.CodeTranslation
{
    /// <summary>
    /// Defines methods for generating source code.
    /// </summary>
    public interface ICodeGenerator
    {
        /// <summary>
        /// Called when the source code generation starts. Returns code to be written to the beginning of the file (headers, class definitions, etc.).
        /// </summary>
        string GenerateStart();

        /// <summary>
        /// Called when the source code generation is finished. Returns code to be written to the end of the file (closing brackets, etc.).
        /// </summary>
        string GenerateEnd();

        /// <summary>
        /// Generates an enum.
        /// </summary>
        /// <param name="enum">Information about the enum which to generate.</param>
        /// <param name="index">The index of the enum.</param>
        /// <param name="count">The total number of enums to generate.</param>
        string GenerateEnum(EnumDefinition @enum, int index, int count);

        /// <summary>
        /// Generates a function.
        /// </summary>
        /// <param name="function">Information about the function which to generate.</param>
        /// <param name="index">The index of the function.</param>
        /// <param name="count">The total number of function to generate.</param>
        string GenerateFunction(FunctionDefinition function, int index, int count);

        /// <summary>
        /// Generates a struct.
        /// </summary>
        /// <param name="struct">Information about the struct which to generate.</param>
        /// <param name="index">The index of the struct.</param>
        /// <param name="count">The total number of structs to generate.</param>
        string GenerateStruct(StructDefinition @struct, int index, int count);
    }
}
