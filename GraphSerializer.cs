using System.IO;
using ProtoBuf;

namespace ConstelLite
{
    /// <summary>
    /// Class <c>GraphSerializer</c> acts as a serializer/deserializer to save/load graph data on disk.
    /// It utilizes Serialize() and Deserialize() methods of the protobuf-net serializer.
    /// </summary>
    internal static class GraphSerializer
    {
        /// <summary>
        /// SerializeGraph() method serializes the graph data and creates/overwrites a file for the given path to save the graph database.
        /// </summary>
        /// <param><c>graph</c> is the graph database to be serialized.</param>
        /// <param><c>filePath</c> specifies the path for the file to be created.</param>
        public static void SerializeGraph(Graph graph, string filePath)
        {
            using (FileStream fileStream = File.Create(filePath))
            {
                Serializer.Serialize(fileStream, graph);
            }
        }

        /// <summary>
        /// DeserializeGraph() method deserializes a file for the given path to load the specified graph database.
        /// </summary>
        /// <param><c>filePath</c> specifies the path for the file to be read.</param>
        public static Graph DeserializeGraph(string filePath)
        {
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                return Serializer.Deserialize<Graph>(fileStream);
            }
        }
    }
}
