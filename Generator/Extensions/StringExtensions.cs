using System;
using System.Text;

namespace Generator.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Appends a copy of the specified string, indented by the specified amount, to the end of the current <see cref="StringBuilder"/> object.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="value">The string to append.</param>
        /// <param name="indentation">The indentation to apply to the string.</param>
        public static void Append(this StringBuilder builder, string value, IndentationHelper indentation)
        {
            if(indentation == null)
                throw new ArgumentNullException(nameof(indentation));

            builder.Append(indentation + value);
        }

        /// <summary>
        /// Appends a copy of the specified string, indented by the specified amount, to the end of the current <see cref="StringBuilder"/> object.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="value">The string to append.</param>
        /// <param name="indentation">The indentation to apply to the string.</param>
        public static void Append(this StringBuilder builder, IndentationHelper indentation, string value)
        {
            builder.Append(value, indentation);
        }

        /// <summary>
        /// Appends a copy of the specified string, indented by the specified amount, followed by the default line terminator to the end of the current <see cref="StringBuilder"/> object.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="value">The string to append.</param>
        /// <param name="indentation">The indentation to apply to the string.</param>
        public static void AppendLine(this StringBuilder builder, string value, IndentationHelper indentation)
        {
            if(indentation == null)
                throw new ArgumentNullException(nameof(indentation));

            builder.AppendLine(indentation + value);
        }

        /// <summary>
        /// Appends a copy of the specified string, indented by the specified amount, followed by the default line terminator to the end of the current <see cref="StringBuilder"/> object.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="value">The string to append.</param>
        /// <param name="indentation">The indentation to apply to the string.</param>
        public static void AppendLine(this StringBuilder builder, IndentationHelper indentation, string value)
        {
            builder.AppendLine(value, indentation);
        }

        /// <summary>
        /// Appends the default line terminator to the end of the current <see cref="StringBuilder"/> object, indented by the specified amount.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        /// <param name="indentation">The indentation to apply.</param>
        public static void AppendLine(this StringBuilder builder, IndentationHelper indentation)
        {
            if(indentation == null)
                throw new ArgumentNullException(nameof(indentation));

            builder.AppendLine(indentation.ToString());
        }
    }
}
