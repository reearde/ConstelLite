using System.Collections.Generic;
using ProtoBuf;

namespace ConstelLite
{
    /// <summary>
    /// Class <c>Relationship</c> models a Relationship(edge) in the graph.
    /// This is a subclass of class <c>GraphEntity</c>.
    /// </summary>
    [ProtoContract]
    public class Relationship : GraphEntity
    {
        /// <value>
        /// Property <c>RelationshipType</c> represents a token that is assigned to relationships only.
        /// </value>
        [ProtoMember(1)]
        public string RelationshipType { get; set; }

        /// <value>
        /// Property <c>SourceNode</c> represents the source Node of the Relationship.
        /// </value>
        [ProtoMember(2)]
        public Node SourceNode { get; set; }

        /// <value>
        /// Property <c>TargetNode</c> represents the target Node of the Relationship.
        /// </value>
        [ProtoMember(3)]
        public Node TargetNode { get; set; }

        /// <summary>
        /// This parameterless constructor initializes a new Relationship.
        /// </summary>
        public Relationship()
        {

        }

        /// <summary>
        /// This constructor initializes the new Relationship and assigns its type, source node, target node.
        /// It also assigns properties of a relationship, if any.
        /// </summary>
        public Relationship(string relationshipType, Node sourceNode, Node targetNode, Dictionary<string, string> relationshipProperties)
        {
            RelationshipType = relationshipType;
            SourceNode = sourceNode;
            TargetNode = targetNode;

            // If there are properties, add to node
            if (relationshipProperties != null)
            {
                Properties = relationshipProperties;
            }
        }

        /// <summary>
        /// This constructor initializes the new Relationship and assigns its type, source node, target node.
        /// OBSOLETE
        /// </summary>
        public Relationship(string id, string relationshipType, Node sourceNode, Node targetNode)
        {
            Id = id;
            RelationshipType = relationshipType;
            SourceNode = sourceNode;
            TargetNode = targetNode;
        }
    }
}
