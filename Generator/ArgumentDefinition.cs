using System;
using System.Collections.Generic;
using System.Text;

namespace Generator
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
        public string Type { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentDefinition"/> class.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="type">The argument type.</param>
        public ArgumentDefinition(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}
