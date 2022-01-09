using System;
using System.Collections.Generic;

namespace Generator
{
    /// <summary>
    /// Holds information about a function.
    /// </summary>
    public class FunctionDefinition
    {
        /// <summary>
        /// Gets the name of the function.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the return type of the function.
        /// </summary>
        public string ReturnType { get; }

        /// <summary>
        /// Gets the comment describing the function.
        /// </summary>
        public string Comment { get; }

        /// <summary>
        /// Gets the list of arguments that the function requires.
        /// </summary>
        public List<ArgumentDefinition> Arguments { get; } = new List<ArgumentDefinition>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionDefinition"/> class.
        /// </summary>
        /// <param name="name">The name of the function.</param>
        /// <param name="returnType">The return type of the function.</param>
        /// <param name="comment">The comment describing the function.</param>
        public FunctionDefinition(string name, string returnType, string comment = null)
        {
            Name = name;
            ReturnType = returnType;
            Comment = comment;
        }
    }
}
