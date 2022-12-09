using ArisDocGenerator;
using ArisDocGenerator.Models;

string path = Path.Combine(Environment.CurrentDirectory, "AsepriteDotNet.xml");

Doc doc = DocXmlParser.Parse(path);

Console.WriteLine("finished");