using System;

namespace Generator.Parsing
{
    /// <summary>
    /// Holds information about a function argument.
    /// </summary>
    public class ArgumentDefinition
    {
        /// <summary>
        /// Gets the name of the argument.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the argument type.
        /// </summary>
        public TypeDefinition Type { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentDefinition"/> class.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="type">The argument type.</param>
        public ArgumentDefinition(string name, TypeDefinition type)
        {
            Name = name;
            Type = type;
        }
    }
}
