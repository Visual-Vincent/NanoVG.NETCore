using System;
using System.Collections.Generic;
using System.IO;
using CommandLine;
using Generator.CodeTranslation;
using Generator.CodeTranslation.Generators;
using Generator.Parsing.Languages;

namespace Generator.Commands
{
    [Verb("pinvoke", HelpText = "Parses the specified C file(s) and generates their respective P/Invoke signatures")]
    public class GeneratePInvokes : Command
    {
        [Option('f', "files", Required = true, HelpText = "The C file(s) to parse for function definitions")]
        public IEnumerable<string> Files { get; set; }

        [Option('l', "library", Required = true, HelpText = "The name/path to the library to put in the DllImport header")]
        public string LibraryName { get; set; }

        [Option('n', "namespace", Required = true, HelpText = "The namespace that the P/Invoke definitions will reside in")]
        public string Namespace { get; set; }

        [Option('c', "class", Required = true, HelpText = "The name of the static class that the P/Invoke definitions will reside in")]
        public string ClassName { get; set; }

        [Option('o', "output", Required = true, HelpText = "The output the file to write the generated code to")]
        public string Output { get; set; }

        [Option("partial-class", Required = false, HelpText = "Whether or not to generate a partial class definition")]
        public bool PartialClass { get; set; }

        [Option("funcprefix", Required = false, HelpText = "A common function prefix that may be omitted from the P/Invoke declarations")]
        public string FunctionPrefix { get; set; }

        public override void Execute()
        {
            try
            {
                string outputFile = Path.GetFullPath(Output);
                string outputPath = Path.GetDirectoryName(outputFile);

                if(Directory.Exists(outputFile))
                    throw new IOException($"Output path \"{outputFile}\" is a directory");

                if(!Directory.Exists(outputFile))
                    Directory.CreateDirectory(outputPath);

                List<string> sourceCodes = new List<string>();

                foreach(string file in Files)
                {
                    sourceCodes.Add(File.ReadAllText(file));
                }

                ClangParser parser = new ClangParser();
                PInvokeGenerator generator = new PInvokeGenerator(LibraryName, Namespace, ClassName, FunctionPrefix, PartialClass);

                string pinvokeCode = CodeTranslator.Translate(sourceCodes.ToArray(), parser, generator);

                File.WriteAllText(outputFile, pinvokeCode);
            }
            catch(FileNotFoundException ex)
            {
                Console.WriteLine("Failed to generate P/Invoke definitions:");
                Console.WriteLine($"  Could not find file: {ex.FileName}");

                Environment.ExitCode = 1;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Failed to generate P/Invoke definitions:");
                Console.WriteLine($"  {ex}");

                Environment.ExitCode = 1;
            }
        }
    }
}
