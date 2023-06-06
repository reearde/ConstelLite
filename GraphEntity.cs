using System.Collections.Generic;

namespace ConstelLite
{
    /// <summary>
    /// Class <c>GraphEntity</c> models an abstract entity in the graph.
    /// This is the base class of classes <c>NodeStructure</c> and <c>RelationshipStructure</c>.
    /// </summary>
    public abstract class GraphEntity
    {
        /// <value>
        /// Property <c>Id</c> represents a unique, comparable identity.
        /// </value>
        public string Id { get; set; }

        /// <value>
        /// Dictionary <c>Properties</c> represents set of properties in a node or relationship that can be uniquely identifiable with their respective property keys.
        /// </value>
        public Dictionary<string, string> Properties { get; set; }
    }
}
