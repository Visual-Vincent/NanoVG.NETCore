using System;
using Generator.Commands;

namespace Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            Command.ParseAndExecute(args);
        }
    }
}
