using System;
using System.Collections.Generic;
using System.Text;

namespace Generator.Parsing
{
    /// <summary>
    /// Holds information about a function/argument type.
    /// </summary>
    public class TypeDefinition
    {
        private static readonly Dictionary<TypeFlags, string> DefaultTypePrefixes = new Dictionary<TypeFlags, string>() {
            { TypeFlags.Const, "const " },
            { TypeFlags.Unsigned, "unsigned " }
        };

        /// <summary>
        /// Gets the expression defining the size of the array.
        /// </summary>
        public string ArrayBounds { get; } = "";

        /// <summary>
        /// Gets whether or not this is an array type.
        /// </summary>
        public bool IsArray { get; } = false;

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the type flags.
        /// </summary>
        public TypeFlags Flags { get; }
        
        /// <summary>
        /// Gets the pointer type of this type.
        /// </summary>
        public PointerType Pointer { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeDefinition"/> class.
        /// </summary>
        /// <param name="name">The name of the type.</param>
        /// <param name="flags">The type flags.</param>
        public TypeDefinition(string name, TypeFlags flags)
        {
            Name = name.TrimEnd('*', '&');
            Flags = flags;

            if(name.EndsWith('&') || name.EndsWith("**"))
                Pointer = PointerType.AddressOf;
            else if(name.EndsWith('*'))
                Pointer = PointerType.Standard;
            else
                Pointer = PointerType.None;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeDefinition"/> class.
        /// </summary>
        /// <param name="name">The name of the type.</param>
        /// <param name="flags">The type flags.</param>
        /// <param name="pointerType">The pointer type of this type.</param>
        public TypeDefinition(string name, TypeFlags flags, PointerType pointerType)
        {
            if(!Enum.IsDefined(typeof(PointerType), pointerType))
                throw new ArgumentOutOfRangeException(nameof(pointerType));

            Name = name;
            Flags = flags;
            Pointer = pointerType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeDefinition"/> class.
        /// </summary>
        /// <param name="name">The name of the type.</param>
        /// <param name="flags">The type flags.</param>
        /// <param name="arrayBounds">The expression defining the size of the array.</param>
        public TypeDefinition(string name, TypeFlags flags, string arrayBounds)
            : this(name, flags)
        {
            ArrayBounds = arrayBounds;
            IsArray = true;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return ToString(DefaultTypePrefixes);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <param name="prefixes">A lookup table of prefixes to apply for each flag that the type has set.</param>
        /// <param name="arraySuffix">Whether or not to add the array bounds as a suffix if <see cref="IsArray"/> is True.</param>
        public string ToString(IDictionary<TypeFlags, string> prefixes, bool arraySuffix = true)
        {
            StringBuilder builder = new StringBuilder();

            foreach(var kvp in prefixes)
            {
                if(Flags.HasFlag(kvp.Key))
                    builder.Append(kvp.Value);
            }

            builder.Append(Name);

            switch(Pointer)
            {
                case PointerType.Standard:
                    builder.Append("*");
                    break;

                case PointerType.AddressOf:
                    builder.Append("**");
                    break;
            }

            if(arraySuffix && IsArray)
                builder.Append($"[{ArrayBounds}]");

            return builder.ToString();
        }
    }

    /// <summary>
    /// Defines a specific type of pointer.
    /// </summary>
    public enum PointerType : int
    {
        /// <summary>
        /// Not a pointer type
        /// </summary>
        None = 0,

        /// <summary>
        /// A standard pointer (*)
        /// </summary>
        Standard,

        /// <summary>
        /// An address-of pointer (** or &amp;)
        /// </summary>
        AddressOf
    }

    /// <summary>
    /// Defines flags for a <see cref="TypeDefinition"/>.
    /// </summary>
    [Flags]
    public enum TypeFlags : int
    {
        /// <summary>
        /// No flags
        /// </summary>
        None = 0,

        /// <summary>
        /// A const variable
        /// </summary>
        Const = 1 << 0,

        /// <summary>
        /// The type is unsigned (only valid for integer types)
        /// </summary>
        Unsigned = 1 << 1,
    }
}
