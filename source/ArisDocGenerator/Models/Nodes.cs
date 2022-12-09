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
namespace ArisDocGenerator.Models;

internal sealed record AssemblyNode(string Text);
internal sealed record MemberNode(string Name,
                                  SummaryNode? Summary = default,
                                  RemarksNode? Remarks = default,
                                  ReturnsNode? Returns = default,
                                  List<ParamNode>? Params = default,
                                  List<ExceptionNode>? Exceptions = default,
                                  ValueNode? Value = default,
                                  List<TypeParamNode>? TypeParams = default);

internal sealed record SummaryNode(string Text);
internal sealed record RemarksNode(string Text);
internal sealed record ReturnsNode(string Text);
internal sealed record ParamNode(string Name, string Text);
internal sealed record ExceptionNode(string Cref, string Text);
internal sealed record ValueNode(string Text);
internal sealed record TypeParamNode(string Name, string Text);
