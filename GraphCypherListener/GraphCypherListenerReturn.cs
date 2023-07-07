using Antlr4.Runtime.Misc;
using System;
using System.Linq;

namespace ConstelLite
{
    /// <summary>
    /// Class <c>GraphCypherListener</c> acts as an event handler.
    /// It listens (and walks) the parse tree. When a signal is triggered, it invokes the related method.
    /// It is derived from CypherBaseListener to implement and invoke necessary behavior for queries.
    /// </summary>
    internal partial class GraphCypherListener : CypherBaseListener
    {
        /// <summary>
        /// PrintNode() is a helper method for the ExitOC_Return() method.
        /// It prints the given node in the labeled property graph style.
        /// </summary>
        /// <param><c>node</c> is the node to be printed.</param>
        public void PrintNode(Node node)
        {
            Console.Write($"(");
            if (node.Id != null)
            {
                Console.Write($"{node.Id}");
            }
            if (node.Labels != null)
            {
                Console.Write(":" + string.Join(":", node.Labels));
            }
            if (node.Properties != null)
            {
                Console.Write(" {");
                Console.Write(string.Join(", ", node.Properties.Select(property => $"{property.Key}: {property.Value}")));
                Console.Write("}");
            }
            Console.Write($")");
        }

        /// <summary>
        /// PrintRelationship() is a helper method for the ExitOC_Return() method.
        /// It prints the given relationship in the labeled property graph style.
        /// </summary>
        /// <param><c>relationship</c> is the relationship to be printed.</param>
        public void PrintRelationship(Relationship relationship)
        {
            Console.Write("-[");
            if (relationship.Id != null)
            {
                Console.Write($"{relationship.Id}");
            }
            if (relationship.RelationshipType != null)
            {
                Console.Write($"{relationship.RelationshipType}");
            }
            if (relationship.Properties != null)
            {
                Console.Write(" {");
                Console.Write(string.Join(", ", relationship.Properties.Select(property => $"{property.Key}: {property.Value}")));
                Console.Write("}");
            }
            Console.Write("]->");
        }

        /// <summary>
        /// ExitOC_Return() method performs the behavior for the part of queries which involve the RETURN clause.
        /// This method invokes when the listener exits the Return context of the parse tree.
        /// Its behavior is to print the MATCH result (also can be further filtered with WHERE) that defined in the context (and the query itself).
        /// Because of that the query itself should be combined with a MATCH clause. e.g. MATCH (n) WHERE n:Person RETURN n
        /// </summary>
        /// <param><c>returnContext</c> is the RETURN clause related context which stores the parsed information from the parse tree of the query string.</param>
        public override void ExitOC_Return([NotNull] CypherParser.OC_ReturnContext returnContext)
        {

            if (returnContext.oC_ProjectionBody().oC_ProjectionItems().GetText() == "*")
            {
                // RETURN all elements in the context -> RETURN *
                Console.WriteLine(returnContext.oC_ProjectionBody().oC_ProjectionItems().GetText());
            }
            else
            {
                // Get RETURN variable name from Parse Tree
                var returnVariableName = returnContext.oC_ProjectionBody().oC_ProjectionItems().oC_ProjectionItem()[0].oC_Expression().
                    oC_OrExpression().oC_XorExpression()[0].oC_AndExpression()[0].oC_NotExpression()[0].oC_ComparisonExpression().
                    oC_StringListNullPredicateExpression().oC_AddOrSubtractExpression().oC_MultiplyDivideModuloExpression()[0].oC_PowerOfExpression()[0].
                    oC_UnaryAddOrSubtractExpression()[0].oC_ListOperatorExpression().oC_PropertyOrLabelsExpression().oC_Atom().oC_Variable().GetText();

                // Get RETURN property lookup from Parse Tree
                var returnPropertyLookup = returnContext.oC_ProjectionBody().oC_ProjectionItems().oC_ProjectionItem()[0].oC_Expression().
                        oC_OrExpression().oC_XorExpression()[0].oC_AndExpression()[0].oC_NotExpression()[0].oC_ComparisonExpression().
                        oC_StringListNullPredicateExpression().oC_AddOrSubtractExpression().oC_MultiplyDivideModuloExpression()[0].oC_PowerOfExpression()[0].
                        oC_UnaryAddOrSubtractExpression()[0].oC_ListOperatorExpression().oC_PropertyOrLabelsExpression().oC_PropertyLookup();

                if (returnPropertyLookup.Length < 1)
                {
                    if (returnVariableName is null)
                    {
                        // ERROR: RETURN pattern has no variable name and property key
                    }
                    else
                    {
                        // RETURN pattern with only variable name -> RETURN n
                        if (returnVariableName == matchVariableName)
                        {
                            Console.WriteLine("+------------------+");
                            Console.WriteLine(matchVariableName);
                            Console.WriteLine("+------------------+");
                            if (GraphEngine.MatchResult.Item2.Count != 0)
                            {
                                // Return Relationships
                                foreach (var relationship in GraphEngine.MatchResult.Item2)
                                {
                                    PrintNode(relationship.SourceNode);
                                    PrintRelationship(relationship);
                                    PrintNode(relationship.TargetNode);
                                    Console.WriteLine();
                                }
                            }
                            else if (GraphEngine.MatchResult.Item1.Count != 0)
                            {
                                // Return Nodes
                                foreach (var node in GraphEngine.MatchResult.Item1)
                                {
                                    PrintNode(node);
                                    Console.WriteLine();
                                }
                            }
                            Console.WriteLine("+------------------+");
                        }
                        else
                        {
                            // ERROR: Variable is not defined
                            throw new Exception($"Variable '{returnVariableName}' is not defined.");
                        }
                    }
                }
                else
                {
                    if (returnVariableName is null)
                    {
                        // ERROR: RETURN pattern has no variable name
                    }
                    else
                    {
                        // RETURN pattern has a property key -> RETURN n.key
                        foreach (var returnPropertyLookupElement in returnPropertyLookup)
                        {
                            var returnPropertyKey = returnPropertyLookupElement.oC_PropertyKeyName().GetText();
                            // RETURN behavior is printing specified property values
                            if (returnVariableName == matchVariableName)
                            {
                                Console.WriteLine("+------------------+");
                                Console.WriteLine($"{matchVariableName}.{returnPropertyKey}");
                                Console.WriteLine("+------------------+");
                                if (GraphEngine.MatchResult.Item2.Count != 0)
                                {
                                    // Return Relationships filtered by -> RETURN n.key
                                    foreach (var relationship in GraphEngine.MatchResult.Item2)
                                    {
                                        if (relationship.Properties != null)
                                        {
                                            if (relationship.Properties.ContainsKey(returnPropertyKey))
                                            {
                                                Console.WriteLine(relationship.Properties[returnPropertyKey]);
                                                GraphEngine.ReturnResult = relationship.Properties[returnPropertyKey];
                                            }
                                            else
                                            {
                                                Console.WriteLine("null");
                                            }
                                        }
                                    }
                                }
                                else if (GraphEngine.MatchResult.Item1.Count != 0)
                                {
                                    // Return Nodes filtered by -> RETURN n.key
                                    foreach (var node in GraphEngine.MatchResult.Item1)
                                    {
                                        if (node.Properties != null)
                                        {
                                            if (node.Properties.ContainsKey(returnPropertyKey))
                                            {
                                                Console.WriteLine(node.Properties[returnPropertyKey]);
                                                GraphEngine.ReturnResult = node.Properties[returnPropertyKey];
                                            }
                                            else
                                            {
                                                Console.WriteLine("null");
                                            }
                                        }
                                    }
                                }
                                Console.WriteLine("+------------------+");
                            }
                            else
                            {
                                // ERROR: Variable is not defined
                                throw new Exception($"Variable '{returnVariableName}' is not defined.");
                            }
                        }
                    }
                }
            }
        }
    }
}
