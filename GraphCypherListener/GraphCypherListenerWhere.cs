using Antlr4.Runtime.Misc;
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
        /// ExitOC_Where() method performs the behavior for the part of queries which involve the WHERE clause.
        /// This method invokes when the listener exits the Where context of the parse tree.
        /// Its behavior is to filter the nodes from the match result that defined in the context (and the query itself).
        /// Because of that the query itself should be combined with a MATCH clause. e.g. MATCH (n) WHERE n:Person RETURN n
        /// It can currently filter nodes by label.
        /// </summary>
        /// <param><c>whereContext</c> is the WHERE clause related context which stores the parsed information from the parse tree of the query string.</param>
        public override void ExitOC_Where([NotNull] CypherParser.OC_WhereContext whereContext)
        {
            // MATCH with WHERE Clause Behavior
            var wherePropertyOrLabelsExpression = whereContext.oC_Expression().
                oC_OrExpression().oC_XorExpression()[0].oC_AndExpression()[0].oC_NotExpression()[0].oC_ComparisonExpression().
                oC_StringListNullPredicateExpression().oC_AddOrSubtractExpression().oC_MultiplyDivideModuloExpression()[0].oC_PowerOfExpression()[0].
                oC_UnaryAddOrSubtractExpression()[0].oC_ListOperatorExpression().oC_PropertyOrLabelsExpression();

            var whereVariableName = wherePropertyOrLabelsExpression.oC_Atom().oC_Variable().GetText();

            if (whereVariableName == matchVariableName)
            {
                // Filter by Property -> WHERE n:Property
                /*
                if (wherePropertyOrLabelsExpression.oC_PropertyLookup() != null)
                {
                    foreach (var propertyLookupContext in wherePropertyOrLabelsExpression.oC_PropertyLookup())
                    {
                        Console.WriteLine(propertyLookupContext.oC_PropertyKeyName().GetText());
                    }
                }
                */

                // Filter by Node Label -> WHERE n:Label
                if (wherePropertyOrLabelsExpression.oC_NodeLabels() != null)
                {
                    var whereNodeLabels = wherePropertyOrLabelsExpression.oC_NodeLabels().oC_NodeLabel();

                    //Console.WriteLine(whereVariableName);
                    foreach (var whereLabel in whereNodeLabels)
                    {
                        //Console.WriteLine(whereLabel.GetText());
                        //Console.WriteLine(whereNodeLabels.Length);

                        if (matchResult.Item1 != null)
                        {
                            foreach (var node in matchResult.Item1.ToList())
                            {
                                int count = 0;
                                if (node.Labels != null)
                                {
                                    foreach (var nodeLabel in node.Labels)
                                    {
                                        if (nodeLabel == whereLabel.oC_LabelName().GetText())
                                        {
                                            count++;
                                        }
                                    }
                                }

                                if (count != whereNodeLabels.Length)
                                {
                                    matchResult.Item1.Remove(node);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
