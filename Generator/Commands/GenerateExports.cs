using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CommandLine;
using Generator.Parsing;
using Generator.Parsing.Languages;

namespace Generator.Commands
{
    [Verb("exports", HelpText = "Parses the specified file(s) and generates a Visual C/C++ module exports file (.def)")]
    public class GenerateExports : Command
    {
        private static readonly Regex SymbolRegex = new Regex(@"\A[a-z0-9_]+(?:\@[a-z0-9_]+)?\z", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        [Option('f', "files", Required = true, HelpText = "The C file(s) to parse for function definitions")]
        public IEnumerable<string> Files { get; set; }

        [Option('n', "name", Required = true, HelpText = "The name of the library which to put in the .def file header")]
        public string LibraryName { get; set; }

        [Option('o', "output", Required = true, HelpText = "The output the file to write the generated code to")]
        public string Output { get; set; }

        [Option('s', "symbol", Required = false, HelpText = "Additional symbol(s) to include in the exports file")]
        public IEnumerable<string> AdditionalSymbols { get; set; }
        
        public override void Execute()
        {
            try
            {
                ClangParser parser = new ClangParser();
                List<FunctionDefinition> functionList = new List<FunctionDefinition>();

                foreach(string file in Files)
                {
                    string contents = File.ReadAllText(file);
                    var functions = parser.ParseFunctions(contents, false);
                    functionList.AddRange(functions);
                }

                string outputFile = Path.GetFullPath(Output);
                string outputPath = Path.GetDirectoryName(outputFile);

                if(Directory.Exists(outputFile))
                    throw new IOException($"Output path \"{outputFile}\" is a directory");

                if(!Directory.Exists(outputFile))
                    Directory.CreateDirectory(outputPath);

                StringBuilder builder = new StringBuilder();
                builder.AppendLine($"LIBRARY {LibraryName}");
                builder.AppendLine("EXPORTS");

                foreach(var function in functionList)
                {
                    builder.Append("\t");
                    builder.AppendLine(function.Name);
                }

                foreach(string symbol in AdditionalSymbols ?? Enumerable.Empty<string>())
                {
                    if(!SymbolRegex.IsMatch(symbol))
                        throw new ArgumentException($"Invalid characters in symbol: {symbol}");

                    builder.Append("\t");
                    builder.AppendLine(symbol);
                }

                File.WriteAllText(outputFile, builder.ToString());
            }
            catch(FileNotFoundException ex)
            {
                Console.WriteLine("Failed to generate module exports file:");
                Console.WriteLine($"  Could not find file: {ex.FileName}");

                Environment.ExitCode = 1;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Failed to generate module exports file:");
                Console.WriteLine($"  {ex}");

                Environment.ExitCode = 1;
            }
        }
    }
}
