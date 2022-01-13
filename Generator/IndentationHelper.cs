using System;

namespace Generator
{
    /// <summary>
    /// Helps keep track of line indentation.
    /// </summary>
    public class IndentationHelper
    {
        private string indentation = "";
        private char indentationChar;

        /// <summary>
        /// Gets the indentation amount.
        /// </summary>
        public int Amount => indentation.Length;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndentationHelper"/> class.
        /// </summary>
        /// <param name="character">The character to indent with.</param>
        public IndentationHelper(char character)
        {
            indentationChar = character;
        }

        /// <summary>
        /// Increases or decreases indentation.
        /// </summary>
        /// <param name="amount">The amount to increase or decrease the indentation.</param>
        public void Indent(short amount)
        {
            if(amount == 0)
                return;

            if(amount > 0)
            {
                indentation += new string(indentationChar, amount);
                return;
            }

            if(-amount >= indentation.Length)
            {
                indentation = "";
                return;
            }

            indentation = indentation.Remove(0, -amount);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return indentation;
        }
    }
}
