using System;
using System.Linq;
using System.Reflection;
using CommandLine;

namespace Generator.Commands
{
    /// <summary>
    /// The base class for a program command.
    /// </summary>
    public abstract class Command
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        public abstract void Execute();

        private static readonly Type[] Commands = 
            Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.BaseType == typeof(Command) && t.GetCustomAttribute<VerbAttribute>() != null).ToArray();

        /// <summary>
        /// Parses the specified arguments and executes the respective command, if one is found.
        /// </summary>
        /// <param name="args">The arguments sent to the application.</param>
        public static void ParseAndExecute(string[] args)
        {
            Parser.Default.ParseArguments(args, Commands)
                .WithParsed(cmd => { ((Command)cmd).Execute(); });
        }
    }
}
