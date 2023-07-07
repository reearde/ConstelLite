using Antlr4.Runtime.Misc;

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
        /// ExitOC_Delete() method performs the behavior for the part of queries which involve the DELETE clause.
        /// This method invokes when the listener exits the Delete context of the parse tree.
        /// Its behavior is to delete the nodes from the graph that defined in the context (and the query itself).
        /// For each deleted node, it decrements the size of the graph.
        /// </summary>
        /// <param><c>deleteContext</c> is the DELETE clause related context which stores the parsed information from the parse tree of the query string.</param>
        public override void ExitOC_Delete([NotNull] CypherParser.OC_DeleteContext deleteContext)
        {
            // Get DELETE variable name from Parse Tree
            var deleteVariableName = deleteContext.oC_Expression()[0].
                oC_OrExpression().oC_XorExpression()[0].oC_AndExpression()[0].oC_NotExpression()[0].oC_ComparisonExpression().
                oC_StringListNullPredicateExpression().oC_AddOrSubtractExpression().oC_MultiplyDivideModuloExpression()[0].oC_PowerOfExpression()[0].
                oC_UnaryAddOrSubtractExpression()[0].oC_ListOperatorExpression().oC_PropertyOrLabelsExpression().oC_Atom().oC_Variable().GetText();

            // Get DELETE property lookup from Parse Tree
            var deletePropertyLookup = deleteContext.oC_Expression()[0].
                    oC_OrExpression().oC_XorExpression()[0].oC_AndExpression()[0].oC_NotExpression()[0].oC_ComparisonExpression().
                    oC_StringListNullPredicateExpression().oC_AddOrSubtractExpression().oC_MultiplyDivideModuloExpression()[0].oC_PowerOfExpression()[0].
                    oC_UnaryAddOrSubtractExpression()[0].oC_ListOperatorExpression().oC_PropertyOrLabelsExpression().oC_PropertyLookup();

            if (deletePropertyLookup.Length < 1)
            {
                if (deleteVariableName is null)
                {
                    // ERROR: DELETE pattern has no variable name and property key
                }
                else
                {
                    // DELETE pattern with only variable name -> DELETE n
                    if (deleteVariableName == matchVariableName)
                    {
                        if (GraphEngine.MatchResult.Item2.Count != 0)
                        {
                            // Delete Relationships
                            /*
                            foreach (var relationship in matchResult.Item2)
                            {
                                foreach (var graphElement in GraphEngine.GraphInstance.outgoingRelationships.ToList())
                                {
                                    if (graphElement.Value.Contains(relationship))
                                    {
                                        GraphEngine.GraphInstance.outgoingRelationships.Remove(graphElement.Key);
                                    }
                                }
                                foreach (var graphElement in GraphEngine.GraphInstance.incomingRelationships.ToList())
                                {
                                    if (graphElement.Value.Contains(relationship))
                                    {
                                        GraphEngine.GraphInstance.incomingRelationships.Remove(graphElement.Key);
                                    }
                                }
                            }
                            */
                        }
                        else //if (matchResult.Item1.Count != 0)
                        {
                            // Delete Nodes
                            foreach (var node in GraphEngine.MatchResult.Item1)
                            {
                                if (Graph.GetInstance().outgoingRelationships.ContainsKey(node))
                                {
                                    Graph.GetInstance().outgoingRelationships.Remove(node);
                                }
                                if (Graph.GetInstance().incomingRelationships.ContainsKey(node))
                                {
                                    Graph.GetInstance().incomingRelationships.Remove(node);
                                }
                                Graph.GetInstance().DecrementSizeByOne();
                            }
                        }
                    }
                }
            }
            else
            {
                if (deleteVariableName is null)
                {
                    // ERROR: DELETE pattern has no variable name
                }
                else
                {
                    // No variable name, has Property
                }
            }
        }
    }
}
