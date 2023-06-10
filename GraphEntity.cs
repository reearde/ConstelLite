using System.Collections.Generic;
using ProtoBuf;

namespace ConstelLite
{
    /// <summary>
    /// Class <c>GraphEntity</c> models an abstract entity in the graph.
    /// This is the base class of classes <c>NodeStructure</c> and <c>RelationshipStructure</c>.
    /// </summary>
    [ProtoContract]
    [ProtoInclude(10, typeof(Node))]
    [ProtoInclude(11, typeof(Relationship))]
    public abstract class GraphEntity
    {
        /// <value>
        /// Property <c>Id</c> represents a unique, comparable identity.
        /// </value>
        [ProtoMember(1)]
        public string Id { get; set; }

        /// <value>
        /// Dictionary <c>Properties</c> represents set of properties in a node or relationship that can be uniquely identifiable with their respective property keys.
        /// </value>
        [ProtoMember(2)]
        public Dictionary<string, string> Properties { get; set; }
    }
}
