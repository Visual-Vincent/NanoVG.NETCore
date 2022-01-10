using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using CommandLine;

namespace Generator.Commands
{
    [Verb("extract-text", HelpText = "Extracts specific portions from one or more text files")]
    public class ExtractText : Command
    {
        [Option('f', "files", Required = true, HelpText = "The file(s) which to extract text from")]
        public IEnumerable<string> Files { get; set; }

        [Option('s', "start", Required = true, HelpText = "The marker indicating where to start extracting (non-inclusive)")]
        public string Start { get; set; }

        [Option('e', "end", Required = true, HelpText = "The marker indicating when to stop extracting (non-inclusive)")]
        public string End { get; set; }

        [Option('o', "output", Required = true, HelpText = "The resulting output file")]
        public string Output { get; set; }

        [Option('c', "count", Required = false, HelpText = "Maximum number of portions to extract from each file. 0 = infinite")]
        public int Count { get; set; }

        [Option('i', "ignorecase", Required = false, HelpText = "Whether or not the search will be case-insensitive", Default = false)]
        public bool IgnoreCase { get; set; } = false;

        [Option("regex", Required = false, HelpText = "If set, the start and end markers will be interpreted as regular expressions")]
        public bool UseRegex { get; set; }

        public override void Execute()
        {
            RegexOptions options = RegexOptions.CultureInvariant | RegexOptions.Multiline;

            if(IgnoreCase)
                options |= RegexOptions.IgnoreCase;

            string pattern;

            if(!UseRegex)
                pattern = $@"{Regex.Escape(Start)}(?<__extractedContent>(?:.|[\r\n\s])+?){Regex.Escape(End)}";
            else
                pattern = $@"{Start}(?<__extractedContent>(?:.|[\r\n\s])+?){End}";

            Regex regex;

            try
            {
                regex = new Regex(pattern, options, TimeSpan.FromSeconds(10.0));
            }
            catch(ArgumentException)
            {
                Console.WriteLine("Failed to build regex:");
                Console.WriteLine("  Either Start or End pattern is invalid.");

                Environment.ExitCode = 1;
                return;
            }

            try
            {
                string outputFile = Path.GetFullPath(Output);
                string outputPath = Path.GetDirectoryName(outputFile);

                if(Directory.Exists(outputFile))
                    throw new IOException($"Output path \"{outputFile}\" is a directory");

                if(!Directory.Exists(outputFile))
                    Directory.CreateDirectory(outputPath);

                StringBuilder builder = new StringBuilder();
                int max = Count <= 0 ? int.MaxValue : Count;

                foreach(string file in Files)
                {
                    string contents = File.ReadAllText(file);
                    MatchCollection matches = regex.Matches(contents);

                    for(int i = 0; i < matches.Count && i < max; i++)
                    {
                        Match m = matches[i];
                        builder.AppendLine(m.Groups["__extractedContent"].Value);
                    }
                }

                File.WriteAllText(outputFile, builder.ToString());
            }
            catch(FileNotFoundException ex)
            {
                Console.WriteLine($"Could not find file: {ex.FileName}");
                Environment.ExitCode = 1;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                Environment.ExitCode = 1;
            }
        }
    }
}
