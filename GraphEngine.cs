using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConstelLite
{
    /// <summary>
    /// Class <c>GraphEngine</c> acts as the engine of the labeled property graph.
    /// It can execute queries, can keep the time of the query, and can handle exceptions.
    /// It is designed as the only means of access to the graph by a user.
    /// (Currently other classes are <c>public</c>, but when the testing is done, their access modifier will be <c>internal</c>.)
    /// </summary>
    public class GraphEngine
    {
        /// <summary>
        /// This part is used as a means of access during testing.
        /// SUBJECT TO CHANGE
        /// </summary>
        public Graph testGraph;
        public static string ReturnResult { get; set; }
        public static (HashSet<Node>, HashSet<Relationship>) MatchResult { get; set; }
        public void NewGraph()
        {
            Graph.SetInstance(new Graph());
        }
        public static Graph GetGraph()
        {
            return Graph.GetInstance();
        }

        /// <summary>
        /// ExecuteQuery() method executes a query by parsing it, listening(walking) the parsed tree.
        /// Listener acts as an event handler -> waits for a signal, then invokes the necessary methods.
        /// When needed, listener calls the necessary method from the class <c>GraphCypherListener</c> to execute specific behavior (e.g. CREATE, MATCH, DELETE, etc.).
        /// It handles exceptions occured in the query.
        /// It also counts the query time and prints it to Console.
        /// </summary>
        public string ExecuteQuery(string input)
        {
            // Start Stopwatch for Counting Time
            var queryWatch = Stopwatch.StartNew();

            Console.WriteLine("Query:");
            Console.WriteLine(input);

            // Parsing
            ICharStream stream = CharStreams.fromString(input);
            ITokenSource lexer = new CypherLexer(stream);
            ITokenStream tokens = new CommonTokenStream(lexer);
            CypherParser parser = new CypherParser(tokens);
            IParseTree tree = parser.oC_Cypher();
            //Console.WriteLine(tree.ToStringTree(parser)); // Write the parse tree to console, for debugging

            // Walking the Parse Tree using Custom listener
            GraphCypherListener listener = new GraphCypherListener();
            
            try
            {
                ParseTreeWalker.Default.Walk(listener, tree);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception Handler: {e.Message}");
            }

            // Print the Elapsed Time
            queryWatch.Stop();
            var elapsedTime = queryWatch.Elapsed.TotalMilliseconds;
            Console.WriteLine("Query took " + elapsedTime + "ms");
            Console.WriteLine();

            // Return result
            return ReturnResult;
        }

        public void SerializeGraphToFile(string inputFileName)
        {
            GraphSerializer.SerializeGraph(Graph.GetInstance(), $"{inputFileName}.db");
        }

        public void DeserializeGraphFromFile(string inputFileName)
        {
            Graph.SetInstance(GraphSerializer.DeserializeGraph($"{inputFileName}.db"));
        }
    }
}
