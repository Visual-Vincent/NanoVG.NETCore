using System;
using System.Collections.Generic;

namespace Generator
{
    /// <summary>
    /// Holds information about a struct.
    /// </summary>
    public class StructDefinition
    {
        /// <summary>
        /// Gets the name of the struct.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the list of fields within the struct.
        /// </summary>
        public List<FieldDefinition> Fields { get; } = new List<FieldDefinition>();

        /// <summary>
        /// Initializes a new instance of the <see cref="StructDefinition"/> class.
        /// </summary>
        /// <param name="name">The name of the struct.</param>
        public StructDefinition(string name)
        {
            Name = name;
        }
    }
}
