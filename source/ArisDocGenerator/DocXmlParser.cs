/* -----------------------------------------------------------------------------
Copyright 2022 Christopher Whitley

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
----------------------------------------------------------------------------- */
using System.Xml;
using System.Xml.Serialization;

using ArisDocGenerator.Models;

namespace ArisDocGenerator;

internal static class DocXmlParser
{
    public static Doc Parse(string path)
    {
        XmlDocument xmlDoc = new();
        xmlDoc.Load(path);

        XmlElement root = xmlDoc.DocumentElement ?? throw new InvalidOperationException();
        XmlElement assemblyElement = (XmlElement?)root.GetElementsByTagName("assembly")[0] ?? throw new InvalidOperationException();

        AssemblyNode assemblyNode = new(assemblyElement.GetOptionalValue() ?? throw new InvalidOperationException());

        XmlNodeList? memberXmlNodes = xmlDoc.SelectNodes("/doc/members/member");

        List<MemberNode> memberNodes = new();

        if (memberXmlNodes is not null)
        {
            foreach (XmlElement element in memberXmlNodes)
            {
                string name = element.GetRequiredAttribute("name").Value;

                SummaryNode? summaryNode = default;
                RemarksNode? remarksNode = default;
                ReturnsNode? returnNode = default;
                ValueNode? valueNode = default;
                List<ParamNode>? paramNodes = default;
                List<ExceptionNode>? exceptionNodes = default;
                List<TypeParamNode>? typeParamsNodes = default;

                foreach (XmlElement childElement in element.ChildNodes)
                {
                    if (childElement.Name == "summary")
                    {
                        summaryNode = new(childElement.GetOptionalValue());
                    }
                    else if (childElement.Name == "remarks")
                    {
                        remarksNode = new(childElement.GetOptionalValue());
                    }
                    else if (childElement.Name == "returns")
                    {
                        returnNode = new(childElement.GetOptionalValue());
                    }
                    else if (childElement.Name == "param")
                    {
                        if (paramNodes is null)
                        {
                            paramNodes = new List<ParamNode>();
                        }

                        ParamNode paramNode = new(Name: childElement.GetRequiredAttribute("name").Value,
                                                  Text: childElement.GetOptionalValue());

                        paramNodes.Add(paramNode);
                    }
                    else if (childElement.Name == "exception")
                    {
                        if (exceptionNodes is null)
                        {
                            exceptionNodes = new List<ExceptionNode>();
                        }

                        ExceptionNode exNode = new(Cref: childElement.GetRequiredAttribute("cref").Value,
                                                   Text: childElement.GetOptionalValue());

                        exceptionNodes.Add(exNode);
                    }
                    else if (childElement.Name == "value")
                    {
                        valueNode = new(Text: childElement.GetOptionalValue());
                    }
                    else if (childElement.Name == "typeparam")
                    {
                        if (typeParamsNodes is null)
                        {
                            typeParamsNodes = new List<TypeParamNode>();
                        }

                        TypeParamNode typeParamsNode = new(Name: childElement.GetRequiredAttribute("name").Value,
                                                         Text: childElement.GetOptionalValue());

                        typeParamsNodes.Add(typeParamsNode);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Unknown node '{childElement.Name}'");
                    }
                }

                MemberNode member = new(Name: name,
                                        Summary: summaryNode,
                                        Remarks: remarksNode,
                                        Returns: returnNode,
                                        Params: paramNodes,
                                        Exceptions: exceptionNodes,
                                        Value: valueNode,
                                        TypeParams: typeParamsNodes);

                memberNodes.Add(member);
            }
        }

        Doc doc = new(assemblyNode, memberNodes);
        return doc;
    }

    private static XmlAttribute GetRequiredAttribute(this XmlElement element, string name) =>
        element.Attributes[name] ?? throw new InvalidOperationException();

    private static string GetOptionalValue(this XmlElement element) => element.InnerXml ?? string.Empty;
}