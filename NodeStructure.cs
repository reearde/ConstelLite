using System.Collections.Generic;
using ProtoBuf;

namespace ConstelLite
{
    /// <summary>
    /// Class <c>Node</c> models a node(vertex) in the graph.
    /// This is a subclass of class <c>GraphEntity</c>.
    /// </summary>
    [ProtoContract]
    public class Node : GraphEntity
    {
        /// <value>
        /// Property <c>Labels</c> represents a set tokens that is assigned to nodes only.
        /// </value>
        [ProtoMember(1)]
        public HashSet<string> Labels { get; set; }

        /// <summary>
        /// This constructor initializes a new Node.
        /// </summary>
        public Node()
        {

        }

        /// <summary>
        /// This constructor initializes a new Node and assigns its identifier.
        /// OBSOLETE
        /// </summary>
        public Node(string id)
        {
            Id = id;
        }

        /// <summary>
        /// This constructor initializes a new Node and assigns its labels.
        /// OBSOLETE
        /// </summary>
        public Node(List<string> labels)
        {
            Labels = new HashSet<string>();
            foreach (string label in labels)
            {
                if (!Labels.Contains(label))
                {
                    Labels.Add(label); // add label to the node
                }
            }
        }

        /// <summary>
        /// This constructor initializes a new Node and assigns its identifer and labels.
        /// </summary>
        public Node(string id, List<string> labels)
        {
            Id = id;

            if (labels != null)
            {
                Labels = new HashSet<string>();
                foreach (string label in labels)
                {
                    if (!Labels.Contains(label))
                    {
                        Labels.Add(label); // add label to the node
                    }
                }
            }
        }
    }
}
