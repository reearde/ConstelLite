using System;
using System.Collections.Generic;
using ProtoBuf;

namespace ConstelLite
{
    /// <summary>
    /// Class <c>Graph</c> models a directed labeled property graph with nodes and relationships.
    /// It follows the Singleton design pattern, as the graph database supports only one Graph instance.
    /// </summary>
    [ProtoContract]
    public sealed class Graph
    {
        /// <value>
        /// Dictionary <c>outgoingRelationships</c> represents outgoing relationships in the graph.
        /// To do so, it stores nodes and their respective relationships as a key-value pair.
        /// </value>
        [ProtoMember(1)]
        public Dictionary<Node, List<Relationship>> outgoingRelationships;
        /// <value>
        /// Dictionary <c>incomingRelationships</c> represents incoming relationships in the graph.
        /// To do so, it stores nodes and their respective relationships as a key-value pair.
        /// </value>
        [ProtoMember(2)]
        public Dictionary<Node, List<Relationship>> incomingRelationships;
        /// <value>
        /// Property <c>Size</c> represents the size of the graph in terms of number of nodes in the graph.
        /// </value>
        [ProtoMember(3)]
        public static int Size { get; private set; }
        /// <value>
        /// Property <c>NodeIdCount</c> is a counter to be utilized to assign a unique identifier of a node.
        /// </value>
        [ProtoMember(4)]
        public static int NodeIdCount { get; private set; }
        /// <value>
        /// Property <c>RelationshipIdCount</c> is a counter to be utilized to assign a unique identifier of a relationship.
        /// </value>
        [ProtoMember(5)]
        public static int RelationshipIdCount { get; private set; }

        /// <summary>
        /// This constructor initializes the new (and empty) Graph.
        /// </summary>
        public Graph()
        {
            outgoingRelationships = new Dictionary<Node, List<Relationship>>();
            incomingRelationships = new Dictionary<Node, List<Relationship>>();
            Size = 0;
            NodeIdCount = 0;
            RelationshipIdCount = 0;
        }

        /// <summary>
        /// Property <c>graphInstance</c> represents the Singleton implementation for the graph instance to access the Graph whenever necessary.
        /// </summary>
        private static Graph graphInstance;

        public static Graph GetInstance()
        {
            if (graphInstance == null)
            {
                graphInstance = new Graph();
            }
            return graphInstance;
        }

        public static void SetInstance(Graph graph)
        {
            graphInstance = graph;
        }

        /// <summary>
        /// Below are the HELPER METHODS for increasing the functionality of the Graph.
        /// </summary>

        /// <summary>
        /// DecrementSizeByOne() method decrements the size of the graph by one.
        /// </summary>
        public void DecrementSizeByOne()
        {
            Size--;
        }

        /// <summary>
        /// AddNode() method adds a new node to the graph.
        /// OBSOLETE
        /// </summary>
        /// <param><c>node</c> is the new node to be added to the graph.</param>
        public void AddNode(Node node)
        {
            outgoingRelationships[node] = new List<Relationship>();
            incomingRelationships[node] = new List<Relationship>();
        }

        /// <summary>
        /// AddRelationship() method adds a new relationship to the graph.
        /// It also sets the relationship id.
        /// </summary>
        /// <param><c>relationship</c> is the new node to be added to the graph.</param>
        public void AddRelationship(Relationship relationship)
        {
            if (!outgoingRelationships.ContainsKey(relationship.SourceNode) || !outgoingRelationships.ContainsKey(relationship.TargetNode))
            {
                throw new ArgumentException("SourceNode and TargetNode must already be added to the graph.");
            }

            // Set Relationship Unique Identifier
            relationship.Id = RelationshipIdCount.ToString();
            RelationshipIdCount++;

            // Add relationship to outgoing edges of the source node
            outgoingRelationships[relationship.SourceNode].Add(relationship);

            // Add relationship to incoming edges of the target node
            incomingRelationships[relationship.TargetNode].Add(relationship);
        }

        /// <summary>
        /// CreateQueryHelper() method is a helper method for the queries with CREATE clause.
        /// It creates a node from the given parameters, and adds the newly created node to the graph.
        /// This is the improved version of AddNode().
        /// </summary>
        /// <param><c>nodeId</c> is the unique identifier for the node.</param>
        /// <param><c>nodeLabels</c> is the set of labels for the node.</param>
        /// <param><c>nodeProperties</c> is the set of properties for the node.</param>
        /// <returns>
        /// The unique identifier for the created node. It can be needed for comparison.
        /// </returns>
        public string CreateQueryHelper(string nodeId, List<string> nodeLabels, Dictionary<string, string> nodeProperties)
        {
            Node node = new Node(nodeId, nodeLabels);

            // If there are properties, add to node
            if (nodeProperties != null)
            {
                node.Properties = nodeProperties;
            }

            outgoingRelationships.Add(node, new List<Relationship>());
            incomingRelationships.Add(node, new List<Relationship>());
            Size++;
            NodeIdCount++;

            return nodeId;
        }

        /// <summary>
        /// BFS() method performs a Breadth-First Search on the graph.
        /// IMPLEMENTED FOR FUTURE FUNCTIONALITY
        /// </summary>
        /// <param><c>sourceNode</c> is the source node that acts as the starting vertex for Breadth-First Search.</param>
        /// <returns>
        /// The list of visited nodes during the search.
        /// </returns>
        public List<Node> BFS(Node sourceNode)
        {
            List<Node> visited = new List<Node>();
            Queue<Node> queue = new Queue<Node>();

            visited.Add(sourceNode);
            queue.Enqueue(sourceNode);

            while (queue.Count > 0)
            {
                Node currentNode = queue.Dequeue();

                foreach (Relationship relationship in outgoingRelationships[currentNode])
                {
                    Node neighbor = relationship.TargetNode;
                    if (!visited.Contains(neighbor))
                    {
                        visited.Add(neighbor);
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return visited;
        }

        /// <summary>
        /// DFS() method performs a Depth-First Search on the graph.
        /// IMPLEMENTED FOR FUTURE FUNCTIONALITY
        /// </summary>
        /// <param><c>sourceNode</c> is the source node that acts as the starting vertex for Depth-First Search.</param>
        /// <returns>
        /// The list of visited nodes during the search.
        /// </returns>
        public List<Node> DFS(Node sourceNode)
        {
            List<Node> visited = new List<Node>();
            Stack<Node> stack = new Stack<Node>();

            stack.Push(sourceNode);

            while (stack.Count > 0)
            {
                Node currentNode = stack.Pop();

                if (!visited.Contains(currentNode))
                {
                    visited.Add(currentNode);

                    foreach (Relationship relationship in outgoingRelationships[currentNode])
                    {
                        Node neighbor = relationship.TargetNode;
                        stack.Push(neighbor);
                    }
                }
            }

            return visited;
        }

        /// <summary>
        /// ShortestPath() method performs Djikstra's shortest path algorithm on the graph,
        /// to find the shortest path between the source node and the target node.
        /// IMPLEMENTED FOR FUTURE FUNCTIONALITY
        /// </summary>
        /// <param><c>sourceNode</c> is the source node that acts as the starting vertex for shortest path algorithm.</param>
        /// <param><c>sourceNode</c> is the target node that acts as the ending vertex to be found for shortest path algorithm.</param>
        /// <returns>
        /// The shortest path between the source node and the target node in terms of list of nodes.
        /// </returns>
        public List<Node> ShortestPath(Node sourceNode, Node targetNode)
        {
            Dictionary<Node, double> distances = new Dictionary<Node, double>();
            Dictionary<Node, Node> previous = new Dictionary<Node, Node>();
            List<Node> unvisited = new List<Node>();

            foreach (Node node in outgoingRelationships.Keys)
            {
                distances[node] = double.PositiveInfinity;
                previous[node] = null;
                unvisited.Add(node);
            }

            distances[sourceNode] = 0;

            while (unvisited.Count > 0)
            {
                Node currentNode = null;
                double minDistance = double.PositiveInfinity;

                foreach (Node node in unvisited)
                {
                    if (distances[node] < minDistance)
                    {
                        currentNode = node;
                        minDistance = distances[node];
                    }
                }

                if (currentNode == null)
                    break;

                unvisited.Remove(currentNode);

                foreach (Relationship relationship in outgoingRelationships[currentNode])
                {
                    Node neighbor = relationship.TargetNode;
                    double distance = minDistance + 1; // Assume each edge has a weight of 1

                    if (distance < distances[neighbor])
                    {
                        distances[neighbor] = distance;
                        previous[neighbor] = currentNode;
                    }
                }
            }

            List<Node> shortestPath = new List<Node>();
            Node current = targetNode;

            while (current != null)
            {
                shortestPath.Insert(0, current);
                current = previous[current];
            }

            return shortestPath;
        }

    }
}
