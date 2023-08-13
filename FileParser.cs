using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;

namespace Parser
{
    public enum DocumentationTypes
    {
        Html,
        Markdown,
        XML,
        PlainText
    }

    public class FileParser
    {
        private string rawContent { get; set; }

        private string classRegex = "public class .+";
        private string commentRegex = "\\/\\/\\/ .*";
        private string attributeRegex = "\\[.*\\]";
        private string methodRegex = "((public)|(protected)){1}(static)*.*[A-Za-z0-9]+\\(.*\\)";
        private string privateMethodRegex = "((private)){1}(static)*.*[A-Za-z0-9]+\\(.*\\)";

        private List<Tuple<string, Regex>> rxArr { get; set; }

        private List<ClassDocumentation> documentations;


        public FileParser(string fileContent)
        {
            this.rawContent = fileContent;
            this.documentations = new List<ClassDocumentation>();

            this.rxArr = new List<Tuple<string, Regex>>();
            this.rxArr.Add(new Tuple<string, Regex>("Class", new Regex(classRegex, RegexOptions.Compiled)));
            this.rxArr.Add(new Tuple<string, Regex>("Comment", new Regex(commentRegex, RegexOptions.Compiled)));
            this.rxArr.Add(new Tuple<string, Regex>("Attribute", new Regex(attributeRegex, RegexOptions.Compiled)));
            this.rxArr.Add(new Tuple<string, Regex>("Method", new Regex(methodRegex, RegexOptions.Compiled)));
            this.rxArr.Add(new Tuple<string, Regex>("PrivateMethod", new Regex(privateMethodRegex, RegexOptions.Compiled)));

            ParseFile();
        }

        /// <summary>
        /// Parses through file and extracts the relavent comments, attributes, and method information
        /// </summary>
        private void ParseFile()
        {
            int methodIndex = 0;
            int classIndex = -1;

            foreach (var fileLine in this.rawContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                foreach (var rx in this.rxArr)
                {
                    if (rx.Item2.IsMatch(fileLine))
                    {
                        if (rx.Item1 == "Class")
                        {
                            documentations.Add(new ClassDocumentation());
                            classIndex++;
                            methodIndex = 0;
                            documentations[classIndex].className = CleanText(fileLine.Replace("public class ", ""));
                            documentations[classIndex].methods.Add(new MethodDocumentation());
                        }
                        else if (rx.Item1 != "Class" && classIndex > 0)
                        {
                            switch (rx.Item1)
                            {
                                case "Comment":
                                    documentations[classIndex].methods[methodIndex].comments.Add(CleanText(fileLine));
                                    break;
                                case "Attribute":
                                    documentations[classIndex].methods[methodIndex].attributes.Add(CleanText(fileLine));
                                    break;
                                case "Method":
                                    documentations[classIndex].methods[methodIndex].method = CleanText(fileLine);
                                    methodIndex++;
                                    documentations[classIndex].methods.Add(new MethodDocumentation());
                                    break;
                                case "PrivateMethod":
                                    documentations[classIndex].methods[methodIndex].comments = new List<string>();
                                    documentations[classIndex].methods[methodIndex].attributes = new List<string>();
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private string CleanText(string text)
        {
            return text.Trim().Replace("/// ", "");
        }

        public string GetDocumentation(DocumentationTypes docType = DocumentationTypes.Markdown)
        {
            switch (docType)
            {
                case DocumentationTypes.Html:
                    return new HtmlDocumentGenerator().CreateDocumentation(this.documentations);
                default:
                case DocumentationTypes.Markdown:
                    return new MarkdownDocumentGenerator().CreateDocumentation(this.documentations);
            }
        }
    }

    public class ClassDocumentation
    {
        public string className { get; set; }
        public List<MethodDocumentation> methods { get; set; }
        public ClassDocumentation()
        {
            this.methods = new List<MethodDocumentation>();
        }
            

    }

    public class MethodDocumentation
    {
        public List<string> comments { get; set; }
        public List<string> attributes { get; set; }
        public string method { get; set; }

        public MethodDocumentation()
        {
            this.comments = new List<string>();
            this.attributes = new List<string>();
        }
    }

    interface DocumentGenerator
    {
        string CreateDocumentation(List<ClassDocumentation> docs);
        void SaveDocumentation();
    }

    public class HtmlDocumentGenerator : DocumentGenerator
    {
        public string CreateDocumentation(List<ClassDocumentation> docs)
        {
            return "";
        }

        public void SaveDocumentation()
        {

        }
    }

    public class MarkdownDocumentGenerator : DocumentGenerator
    {
        public string CreateDocumentation(List<ClassDocumentation> docs)
        {
            return JsonConvert.SerializeObject(docs);
        }

        public void SaveDocumentation()
        {

        }
    }
}