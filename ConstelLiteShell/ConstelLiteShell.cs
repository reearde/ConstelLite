using System;
using System.IO;
using ConstelLite;

namespace ConstelLiteShell
{
    /// <summary>
    /// Class <c>ConstelLiteShell</c> models a Command-Line Interface that uses ConstelLite library.
    /// This is a simple REPL structure utilized to demonstrate the features of the graph database to the user.
    /// </summary>
    class ConstelLiteShell
    {
        static readonly Version shellVersion = new Version();
        static GraphEngine graphEngine;
        static bool isDatabaseOpened = false;
        
        static void EntryPrompt()
        {
            Console.Write($"ConstelLiteShell version {shellVersion.Major}.{shellVersion.Minor}.{shellVersion.Patch} \n" +
                "Enter \".help\" for usage hints. \n");
        }
        static void PrintPrompt()
        {
            if (isDatabaseOpened)
            {
                Console.Write("db > ");
            }
            else
            {
                Console.Write("shell > ");
            }
        }
        
        static void Main(string[] args)
        {
            EntryPrompt();
            string inputBuffer;
            while (true)
            {
                PrintPrompt();

                inputBuffer = Console.ReadLine();

                if (isDatabaseOpened)
                {
                    if (inputBuffer.StartsWith("."))
                    {
                        // Meta Command
                        if (inputBuffer == ".open")
                        {
                            Console.WriteLine("Database already opened.");
                        }
                    }
                    else
                    {
                        // Graph Query
                        PrintPrompt();
                        graphEngine.ExecuteQuery(inputBuffer);
                    }
                }
                if (inputBuffer == ".help")
                {
                    if (isDatabaseOpened)
                    {
                        Console.WriteLine("CREATE - creates nodes and relationships. e.g. CREATE (a:Actor {name: 'Keanu'})-[:ACTED_IN]->(b:Movie {name: 'Matrix'})");
                        Console.WriteLine("DELETE - deletes nodes and relationships. e.g. MATCH (a) DELETE a");
                        Console.WriteLine("MATCH - pattern matching to get an intended subgraph. e.g. MATCH (n {name: 'Keanu'}) RETURN n");
                        Console.WriteLine("WHERE - combined with MATCH to filter matched results. e.g. MATCH (n) WHERE n:Actor");
                        Console.WriteLine("RETURN - combined with MATCH (with WHERE) to print results. e.g. MATCH (n {name: 'Keanu'}) RETURN n");
                    }
                    Console.WriteLine(".open - Opens the ContelLite graph database.");
                    Console.WriteLine(".close - Closes the ContelLite graph database.");
                    Console.WriteLine(".save - Saves the current graph database to specified database file.");
                    Console.WriteLine(".load - Loads specified database from file.");
                    Console.WriteLine(".help - Gives usage hints about ConstelLite Shell, ConstelLite Graph Database queries, and meta-commands.");
                    Console.WriteLine(".exit - Closes the program.");
                    
                }
                if (inputBuffer == ".exit")
                {
                    Environment.Exit(0);
                }
                if (inputBuffer == ".open" && !isDatabaseOpened)
                {
                    graphEngine = new GraphEngine();
                    isDatabaseOpened = true;
                }
                if (inputBuffer == ".close" && isDatabaseOpened)
                {
                    isDatabaseOpened = false;
                }
                if (inputBuffer == ".save" && isDatabaseOpened)
                {
                    Console.Write("Enter the file name to be saved: ");
                    inputBuffer = Console.ReadLine();
                    graphEngine.SerializeGraphToFile(inputBuffer);
                    Console.WriteLine($"Graph database saved to {inputBuffer}.db");
                }
                if (inputBuffer == ".load" && isDatabaseOpened)
                {
                    string directoryPath = AppDomain.CurrentDomain.BaseDirectory;
                    string[] dbFiles = Directory.GetFiles(directoryPath, "*.db");

                    if (dbFiles.Length > 0)
                    {
                        Console.WriteLine("These database files found:");

                        foreach (string filePath in dbFiles)
                        {
                            string fileName = Path.GetFileName(filePath);
                            Console.WriteLine(fileName);
                        }
                        Console.Write("Enter the file name to be loaded: ");
                        inputBuffer = Console.ReadLine();

                        try
                        {
                            if (File.Exists(inputBuffer + ".db"))
                            {
                                // File exists
                                graphEngine.DeserializeGraphFromFile(inputBuffer);
                                Console.WriteLine($"Graph database {inputBuffer}.db loaded.");
                            }
                            else
                            {
                                // File does not exist, handle the error
                                Console.WriteLine($"The database file '{inputBuffer}.db' does not exist.");
                            }
                        }
                        catch (FileNotFoundException ex)
                        {
                            // Handle the exception
                            Console.WriteLine("An error occurred while accessing the file:");
                            Console.WriteLine(ex.Message);
                        }
                        
                    }
                    else
                    {
                        Console.WriteLine("There are no database files found.");
                    }
                    
                }
                else if(inputBuffer != ".exit" && inputBuffer != ".open" && inputBuffer != ".close" && inputBuffer != ".help")
                {
                    if (!isDatabaseOpened)
                    {

                        Console.WriteLine($"Unrecognized meta command '{inputBuffer}'.");
                    }
                }
            }
        }
    }

    public class Version
    {
        public int Major { get; } = 0;
        public int Minor { get; } = 1;
        public int Patch { get; } = 0;

    }
}
