using System.IO;
using ProtoBuf;

namespace ConstelLite
{
    internal static class GraphSerializer
    {
        public static void SerializeGraph(Graph customClass, string filePath)
        {
            using (FileStream fileStream = File.Create(filePath))
            {
                Serializer.Serialize(fileStream, customClass);
            }
        }

        public static Graph DeserializeGraph(string filePath)
        {
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                return Serializer.Deserialize<Graph>(fileStream);
            }
        }
    }
}
