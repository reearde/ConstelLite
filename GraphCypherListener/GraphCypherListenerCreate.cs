using Antlr4.Runtime.Misc;
using System;
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
        /// <summary>
        /// CreateNode() method is a helper method of the ExitOC_Create() method.
        /// This method creates a node and adds it to the graph.
        /// Nodes may have a set of labels and properties, or not.
        /// </summary>
        /// <param><c>nodePattern</c> is the node pattern related context from the query with CREATE clause.</param>
        /// <returns>
        /// The unique identifier for the created node. It can be needed for comparison.
        /// </returns>
        public string CreateNode([NotNull] CypherParser.OC_NodePatternContext nodePattern)
        {
            var nodeVariable = nodePattern.oC_Variable();
            var nodeLabels = nodePattern.oC_NodeLabels();
            var nodeProperties = nodePattern.oC_Properties();

            List<string> nodeLabelsToReturn;
            Dictionary<string, string> nodePropertiesToReturn;
            string nodeId = Graph.NodeIdCount.ToString(); // Guid.NewGuid().ToString();

            // NEW BEHAVIOR
            /*
            if (nodeVariable is null)
            {
                nodeId = Guid.NewGuid().ToString();
            }
            else
            {
                nodeId = nodeVariable.GetText();
            }
            */

            if (nodeLabels is null)
            {
                nodeLabelsToReturn = null;
            }
            else
            {
                // Node without a variable name, with a label, without a property
                var nodeLabelsList = nodeLabels.oC_NodeLabel();
                nodeLabelsToReturn = new List<string>();
                foreach (var nodeLabel in nodeLabelsList)
                {
                    string a = nodeLabel.oC_LabelName().GetText();
                    nodeLabelsToReturn.Add(a);
                }
            }

            if (nodeProperties is null)
            {
                // There is no Property
                nodePropertiesToReturn = null;
            }
            else
            {
                var nodePropertiesContext = nodeProperties.oC_MapLiteral();
                nodePropertiesToReturn = new Dictionary<string, string>();
                // Get Keys
                if (nodePropertiesContext.oC_PropertyKeyName() != null && nodePropertiesContext.oC_Expression() != null)
                {
                    for (int i = 0; i < nodePropertiesContext.oC_PropertyKeyName().Length; i++)
                    {
                        nodePropertiesToReturn.Add(nodePropertiesContext.oC_PropertyKeyName()[i].GetText(), nodePropertiesContext.oC_Expression()[i].GetText());
                    }
                }
                else
                {
                    // ERROR: Expected key and/or value inside a property is missing
                }
            }

            Graph.GetInstance().CreateQueryHelper(nodeId, nodeLabelsToReturn, nodePropertiesToReturn);
            return nodeId;
        }

        /// <summary>
        /// ExitOC_Create() method performs the behavior for the part of queries which involve the CREATE clause.
        /// This method invokes when the listener exits the Create context of the parse tree.
        /// Its behavior is to create nodes and relationships defined in the query, and add them to the graph.
        /// Nodes may have a set of labels and properties.
        /// Relationships may have a relationship type and a set of properties.
        /// Following example creates two nodes without a label and property, and a relationship with a type of :R between them, in a separate manner. e.g. CREATE (a), (b), (a)-[:R]->(b)
        /// </summary>
        /// <param><c>createContext</c> is the CREATE clause related context which stores the parsed information from the parse tree of the query string.</param>
        public override void ExitOC_Create([NotNull] CypherParser.OC_CreateContext createContext)
        {
            // Variable List stores all variable names and their respective id that points a node for the current CREATE context
            Dictionary<string, string> variableList = new Dictionary<string, string>();

            // Search for patterns
            var patterns = createContext.oC_Pattern().oC_PatternPart();
            foreach (var pattern in patterns)
            {
                var patternElement = pattern.oC_AnonymousPatternPart().oC_PatternElement();
                var nodePattern = patternElement.oC_NodePattern();
                var nodeVariable = nodePattern.oC_Variable();

                // Relationship Pattern
                if (patternElement.oC_PatternElementChain().Length > 0)
                {
                    Dictionary<string, string> relationshipPropertiesToReturn;

                    // Create First Node in the Relationship Pattern
                    string firstNodeIdentifier;
                    if (nodeVariable is null)
                    {
                        firstNodeIdentifier = CreateNode(nodePattern);
                    }
                    else
                    {
                        if (variableList.ContainsKey(nodeVariable.GetText()))
                        {
                            firstNodeIdentifier = variableList[nodeVariable.GetText()];
                        }
                        else
                        {
                            firstNodeIdentifier = CreateNode(nodePattern);
                            variableList.Add(nodeVariable.GetText(), firstNodeIdentifier);
                        }
                    }

                    // Behavior for the remaining Relationship Chain
                    foreach (var relationshipChainContext in patternElement.oC_PatternElementChain())
                    {
                        // Get Relationship Type
                        string relType = relationshipChainContext.oC_RelationshipPattern().oC_RelationshipDetail().oC_RelationshipTypes().GetText();

                        // Create Second Node in the Relationship Pattern
                        string secondNodeIdentifier;

                        if (relationshipChainContext.oC_NodePattern().oC_Variable() is null)
                        {
                            secondNodeIdentifier = CreateNode(relationshipChainContext.oC_NodePattern());
                        }
                        else
                        {
                            if (variableList.ContainsKey(relationshipChainContext.oC_NodePattern().oC_Variable().GetText()))
                            {
                                secondNodeIdentifier = variableList[relationshipChainContext.oC_NodePattern().oC_Variable().GetText()];
                            }
                            else
                            {
                                secondNodeIdentifier = CreateNode(relationshipChainContext.oC_NodePattern());
                                variableList.Add(relationshipChainContext.oC_NodePattern().oC_Variable().GetText(), secondNodeIdentifier);
                            }
                        }

                        // Get nodes from the graph according to their Id
                        Node node1 = new Node();
                        Node node2 = new Node();
                        foreach (var nodeInGraph in Graph.GetInstance().outgoingRelationships.Keys)
                        {
                            if (nodeInGraph.Id == firstNodeIdentifier)
                            {
                                node1 = nodeInGraph;
                            }
                            if (nodeInGraph.Id == secondNodeIdentifier)
                            {
                                node2 = nodeInGraph;
                            }
                        }

                        // Add Properties to Relationship
                        var relationshipProperties = relationshipChainContext.oC_RelationshipPattern().oC_RelationshipDetail().oC_Properties();
                        if (relationshipProperties is null)
                        {
                            // There is no Property
                            relationshipPropertiesToReturn = null;
                        }
                        else
                        {
                            var nodePropertiesContext = relationshipProperties.oC_MapLiteral();
                            relationshipPropertiesToReturn = new Dictionary<string, string>();
                            // Add Properties if key and value is not null
                            if (nodePropertiesContext.oC_PropertyKeyName() != null && nodePropertiesContext.oC_Expression() != null)
                            {
                                for (int i = 0; i < nodePropertiesContext.oC_PropertyKeyName().Length; i++)
                                {
                                    relationshipPropertiesToReturn.Add(nodePropertiesContext.oC_PropertyKeyName()[i].GetText(), nodePropertiesContext.oC_Expression()[i].GetText());
                                }
                            }
                            else
                            {
                                // ERROR: Expected key and/or value inside a property is missing
                            }
                        }

                        // Create Relationship Between Nodes
                        if (relationshipChainContext.oC_RelationshipPattern().oC_RightArrowHead() is null)
                        {
                            if (relationshipChainContext.oC_RelationshipPattern().oC_LeftArrowHead() is null)
                            {
                                // -[]-
                                // ERROR Only directed relationships are supported in CREATE
                                throw new Exception($"Only directed relationships are supported in CREATE.");
                            }
                            else
                            {
                                // <-[]-
                                // Reverse Releationship (Left Direction)
                                //Console.WriteLine("Left Relationship.");
                                Graph.GetInstance().AddRelationship(new Relationship(relType, node2, node1, relationshipPropertiesToReturn));

                            }
                        }
                        else
                        {
                            if (relationshipChainContext.oC_RelationshipPattern().oC_LeftArrowHead() is null)
                            {
                                // -[]->
                                // Relationship (Right Direction)
                                //Console.WriteLine("Right Relationship.");
                                Graph.GetInstance().AddRelationship(new Relationship(relType, node1, node2, relationshipPropertiesToReturn));
                            }
                            else
                            {
                                // <-[]->
                                // ERROR Double directed relationships are not supported in CREATE
                                throw new Exception($"Double directed relationships are not supported in CREATE.");
                            }
                        }
                    }
                }
                else
                {
                    // Node Creation Pattern, if no relationship is given
                    if (nodeVariable is null)
                    {
                        CreateNode(nodePattern);
                    }
                    else
                    {
                        if (variableList.ContainsKey(nodeVariable.GetText()))
                        {
                            // ERROR: There are multiple Nodes with same variable name in the pattern
                            throw new Exception($"Variable '{nodeVariable.GetText()}' already declared.");
                        }
                        else
                        {
                            string nodeId = CreateNode(nodePattern);
                            variableList.Add(nodeVariable.GetText(), nodeId);
                        }
                    }
                }
            }
        }
    }
}
