using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parser;

namespace AutoDocumentor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string readFile = "C:\\Users\\shays\\source\\repos\\AutoDocumentor\\Test.cs";

            string fileContent = File.ReadAllText(Path.GetFullPath(readFile));

            var documentationGenerator = new Parser.FileParser(fileContent);

            Console.WriteLine(documentationGenerator.GetDocumentation());

            Console.WriteLine("Press any key to continue... ");
            Console.ReadKey();

            return;
        }
    }
}
