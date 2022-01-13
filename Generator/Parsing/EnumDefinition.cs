using System;
using System.Collections.Generic;

namespace Generator.Parsing
{
    /// <summary>
    /// Holds information about an enum.
    /// </summary>
    public class EnumDefinition
    {
        /// <summary>
        /// Gets the name of the enum.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the list of values in the enum.
        /// </summary>
        public List<KeyValuePair<string, string>> Values { get; } = new List<KeyValuePair<string, string>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDefinition"/> class.
        /// </summary>
        /// <param name="name">The name of the enum.</param>
        public EnumDefinition(string name)
        {
            Name = name;
        }
    }
}
