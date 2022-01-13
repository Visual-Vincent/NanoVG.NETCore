using System;

namespace Generator.Parsing
{
    /// <summary>
    /// Holds information about a struct field.
    /// </summary>
    public class FieldDefinition
    {
        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the field type.
        /// </summary>
        public TypeDefinition Type { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FieldDefinition"/> class.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <param name="type">The field type.</param>
        public FieldDefinition(string name, TypeDefinition type)
        {
            Name = name;
            Type = type;
        }
    }
}
