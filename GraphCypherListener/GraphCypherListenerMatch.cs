using Antlr4.Runtime.Misc;
using System.Collections.Generic;

namespace ConstelLite
{
    /// <summary>
    /// Class <c>GraphCypherListener</c> acts as an event handler.
    /// It listens (and walks) the parse tree. When a signal is triggered, it invokes the related method.
    /// It is derived from CypherBaseListener to implement and invoke necessary behavior for queries.
    /// </summary>
    internal partial class GraphCypherListener : CypherBaseListener
    {
        public string matchVariableName;
        //Dictionary<string, string> matchVariableList;
        //public (HashSet<Node>, HashSet<Relationship>) matchResult;


        /// <summary>
        /// MatchNodes() method is a helper method for the EnterOC_Match() method.
        /// This method matches nodes defined in the query.
        /// It can match nodes specified by a single label or multiple labels. e.g. MATCH (a:A:B)
        /// It can match nodes specified by a single property or multiple properties. e.g. MATCH (n {name: 'bar'})
        /// Or, it can match all nodes in the graph. e.g. MATCH (a)
        /// </summary>
        /// <param><c>matchNodePattern</c> is the node pattern related context from the query with MATCH clause.</param>
        public void MatchNodes([NotNull] CypherParser.OC_NodePatternContext matchNodePattern)
        {

            var matchNodeLabels = matchNodePattern.oC_NodeLabels();
            var matchNodeProperties = matchNodePattern.oC_Properties();

            // If there are Labels specified -> MATCH (a:A:B)
            if (matchNodeLabels != null)
            {
                // MATCH Nodes using Labels
                foreach (var graphNode in Graph.GetInstance().outgoingRelationships.Keys)
                {
                    if (graphNode.Labels != null)
                    {
                        int count = 0;
                        foreach (var nodeLabel in matchNodeLabels.oC_NodeLabel())
                        {
                            if (graphNode.Labels.Contains(nodeLabel.oC_LabelName().GetText()))
                            {
                                count++;
                            }
                        }
                        if (count == matchNodeLabels.oC_NodeLabel().Length)
                        {
                            // Found a node, add it to result
                            GraphEngine.MatchResult.Item1.Add(graphNode);
                        }
                    }
                }
                return;
            }

            // If there are Properties specified -> MATCH (n {name: 'bar'})
            if (matchNodeProperties != null)
            {
                var matchNodePropertiesContext = matchNodeProperties.oC_MapLiteral();
                // MATCH Nodes using Properties
                foreach (var graphNode in Graph.GetInstance().outgoingRelationships.Keys)
                {
                    if (graphNode.Properties != null)
                    {
                        int count = 0;
                        if (matchNodePropertiesContext.oC_PropertyKeyName() != null && matchNodePropertiesContext.oC_Expression() != null)
                        {
                            for (int i = 0; i < matchNodePropertiesContext.oC_PropertyKeyName().Length; i++)
                            {
                                if (graphNode.Properties.ContainsKey(matchNodePropertiesContext.oC_PropertyKeyName()[i].GetText()))
                                {
                                    if (graphNode.Properties[matchNodePropertiesContext.oC_PropertyKeyName()[i].GetText()] == matchNodePropertiesContext.oC_Expression()[i].GetText())
                                    {
                                        count++;
                                    }
                                }
                            }
                        }
                        if (count == matchNodePropertiesContext.oC_PropertyKeyName().Length)
                        {
                            // Found a node, add it to result
                            GraphEngine.MatchResult.Item1.Add(graphNode);
                        }
                    }
                }
                return;
            }

            // MATCH all nodes -> MATCH (n)
            foreach (var node in Graph.GetInstance().outgoingRelationships.Keys)
            {
                GraphEngine.MatchResult.Item1.Add(node);
            }
        }

        /// <summary>
        /// MatchRightRelationships() method is a helper method for the EnterOC_Match() method.
        /// This method matches relationships directed to right (outgoing relationships), defined by the query.
        /// </summary>
        /// <param><c>patternChainContext</c> is the chained pattern related context from the query with MATCH clause.</param>
        public void MatchRightRelationships([NotNull] CypherParser.OC_PatternElementChainContext patternChainContext)
        {
            // -[r]->
            // Relationship (Right Direction)
            // Check for Outgoing Relationships in the current Match Result
            var matchRelationshipLabels = patternChainContext.oC_NodePattern().oC_NodeLabels();
            var matchRelationshipProperties = patternChainContext.oC_NodePattern().oC_Properties();

            foreach (var node in GraphEngine.MatchResult.Item1)
            {
                if (Graph.GetInstance().outgoingRelationships.ContainsKey(node))
                {
                    if (Graph.GetInstance().outgoingRelationships[node] != null)
                    {
                        var RelationshipList = Graph.GetInstance().outgoingRelationships[node];

                        foreach (var relationship in RelationshipList)
                        {
                            // If Target Node of the Relationship specifies a label
                            if (relationship.TargetNode.Labels != null)
                            {
                                if (matchRelationshipLabels != null)
                                {
                                    int count = 0;
                                    foreach (var nodeLabel in matchRelationshipLabels.oC_NodeLabel())
                                    {
                                        if (relationship.TargetNode.Labels.Contains(nodeLabel.oC_LabelName().GetText()))
                                        {
                                            count++;
                                        }
                                    }
                                    if (count == matchRelationshipLabels.oC_NodeLabel().Length)
                                    {
                                        // Found a node, add the relationship to match result
                                        GraphEngine.MatchResult.Item2.Add(relationship);
                                    }
                                    return;
                                }
                            }

                            // Relationship does not specify a relationship or property?
                            GraphEngine.MatchResult.Item2.Add(relationship);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// MatchLeftRelationships() method is a helper method for the EnterOC_Match() method.
        /// This method matches relationships directed to left (incoming relationships), defined by the query.
        /// </summary>
        /// <param><c>patternChainContext</c> is the chained pattern related context from the query with MATCH clause.</param>
        public void MatchLeftRelationships([NotNull] CypherParser.OC_PatternElementChainContext patternChainContext)
        {
            // <-[r]-
            // Reverse Releationship (Left Direction)
            // Check for Incoming Relationships in the current Match Result
            var matchRelationshipLabels = patternChainContext.oC_NodePattern().oC_NodeLabels();
            var matchRelationshipProperties = patternChainContext.oC_NodePattern().oC_Properties();

            foreach (var node in GraphEngine.MatchResult.Item1)
            {
                if (Graph.GetInstance().incomingRelationships.ContainsKey(node))
                {
                    if (Graph.GetInstance().incomingRelationships[node] != null)
                    {
                        var RelationshipList = Graph.GetInstance().incomingRelationships[node];

                        foreach (var relationship in RelationshipList)
                        {

                            // If Target Node of the Relationship specifies a label
                            if (relationship.SourceNode.Labels != null)
                            {
                                if (matchRelationshipLabels != null)
                                {
                                    int count = 0;
                                    foreach (var nodeLabel in matchRelationshipLabels.oC_NodeLabel())
                                    {
                                        if (relationship.SourceNode.Labels.Contains(nodeLabel.oC_LabelName().GetText()))
                                        {
                                            count++;
                                        }
                                    }
                                    if (count == matchRelationshipLabels.oC_NodeLabel().Length)
                                    {
                                        // Found a node, add the relationship to match result
                                        GraphEngine.MatchResult.Item2.Add(relationship);
                                    }
                                    return;
                                }
                            }

                            // Relationship does not specify a relationship or property?
                            GraphEngine.MatchResult.Item2.Add(relationship);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// EnterOC_Match() method performs the behavior for the part of queries which involve the MATCH clause.
        /// This method invokes when the listener enters the Match context of the parse tree.
        /// Its behavior is to perform a pattern matching for the given query,
        /// and storing matched nodes or relationships from the graph as a result.
        /// It can be combined with a WHERE and RETURN clauses for better functionality. e.g. MATCH (n) WHERE n:Person RETURN n
        /// </summary>
        /// <param><c>matchContext</c> is the MATCH clause related context which stores the parsed information from the parse tree of the query string.</param>
        public override void EnterOC_Match([NotNull] CypherParser.OC_MatchContext matchContext)
        {
            // Variable List stores all variable names and their respective id that points a node for the current MATCH context
            //matchVariableList = new Dictionary<string, string>();
            GraphEngine.MatchResult = (new HashSet<Node>(), new HashSet<Relationship>());

            // Clear Previous Match Results, not needed currently
            /*
            if (matchResult.Item1.Count > 0)
            {
                matchResult.Item1.Clear();
            }
            if (matchResult.Item2.Count > 0)
            {
                matchResult.Item2.Clear();
            }
            */

            var patternPart = matchContext.oC_Pattern().oC_PatternPart();

            // MATCH Behavior
            foreach (var partialContext in patternPart)
            {
                var patternChain = partialContext.oC_AnonymousPatternPart().oC_PatternElement().oC_PatternElementChain();
                var matchNodePattern = partialContext.oC_AnonymousPatternPart().oC_PatternElement().oC_NodePattern();

                var matchNodeVariableName = matchNodePattern.oC_Variable();
                if (matchNodeVariableName is null)
                {
                    // ERROR - There are no Node Variables specified in MATCH scope
                    //throw new Exception("There are no Node Variables specified in MATCH scope.");
                }
                else
                {
                    matchVariableName = matchNodeVariableName.GetText();
                }

                if (patternChain.Length > 0)
                {
                    // MATCH Relationships
                    MatchNodes(matchNodePattern); // Nodes added to the Match Result

                    foreach (var patternChainContext in patternChain)
                    {
                        var relationshipPattern = patternChainContext.oC_RelationshipPattern();

                        // Get Variable Name of the Relationship
                        matchVariableName = relationshipPattern.oC_RelationshipDetail().oC_Variable().GetText();

                        if (relationshipPattern.oC_RelationshipDetail().oC_RelationshipTypes() != null)
                        {
                            // MATCH Relationship specified a Relationship Type -> ()-[r:REL_TYPE]-()
                            // Match Relationships between nodes
                            if (relationshipPattern.oC_RightArrowHead() is null)
                            {
                                if (relationshipPattern.oC_LeftArrowHead() is null)
                                {
                                    // ()-[r]-()
                                    // Undirected relationship
                                    // Match the relationship in both directions
                                    MatchRightRelationships(patternChainContext);
                                    MatchLeftRelationships(patternChainContext);
                                }
                                else
                                {
                                    // ()<-[r]-()
                                    // Reverse Releationship (Left Direction)
                                    // Check for Incoming Relationships in the current Match Result
                                    MatchLeftRelationships(patternChainContext);
                                }
                            }
                            else
                            {
                                if (relationshipPattern.oC_LeftArrowHead() is null)
                                {
                                    // ()-[r]->()
                                    // Relationship (Right Direction)
                                    // Check for Outgoing Relationships in the current Match Result
                                    MatchRightRelationships(patternChainContext);
                                }
                                else
                                {
                                    // ()<-[r]->()
                                    // Match the relationship in both directions
                                    MatchRightRelationships(patternChainContext);
                                    MatchLeftRelationships(patternChainContext);
                                }
                            }
                        }
                        else
                        {
                            // MATCH Relationship does not specify a Relationship Type -> ()-[r]-()

                            // Match Relationships between nodes
                            if (relationshipPattern.oC_RightArrowHead() is null)
                            {
                                if (relationshipPattern.oC_LeftArrowHead() is null)
                                {
                                    // ()-[r]-()
                                    // Undirected relationship
                                    // Match the relationship in both directions
                                    MatchRightRelationships(patternChainContext);
                                    MatchLeftRelationships(patternChainContext);
                                }
                                else
                                {
                                    // ()<-[r]-()
                                    // Reverse Releationship (Left Direction)
                                    // Check for Incoming Relationships in the current Match Result
                                    MatchLeftRelationships(patternChainContext);
                                }
                            }
                            else
                            {
                                if (relationshipPattern.oC_LeftArrowHead() is null)
                                {
                                    // ()-[r]->()
                                    // Relationship (Right Direction)
                                    // Check for Outgoing Relationships in the current Match Result
                                    MatchRightRelationships(patternChainContext);
                                }
                                else
                                {
                                    // ()<-[r]->()
                                    // Match the relationship in both directions
                                    MatchRightRelationships(patternChainContext);
                                    MatchLeftRelationships(patternChainContext);
                                }
                            }
                        }
                    }
                }
                else
                {
                    // MATCH Nodes
                    MatchNodes(matchNodePattern);
                }
            }

        }
    }
}
