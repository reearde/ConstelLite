using System;
using ConstelLite;

namespace ConstelLiteTestPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            GraphEngine MyEngine = new GraphEngine();

            //MyEngine.ExecuteQuery("CREATE (n), (n:Turkish:German), (a:Swedish), (b:Swedish {name: 'Herkel'})");
            //MyEngine.ExecuteQuery("MATCH (n) WHERE n:Swedish RETURN n");
            //MyEngine.ExecuteQuery("MATCH (n) DETACH DELETE n");
            //MyEngine.ExecuteQuery("MATCH (n:Turkish) RETURN n");

            //MyEngine.ExecuteQuery("CREATE (:A)-[r:T1]->(:B), (:B)-[r:T2]->(:A), (:B)-[r:T3]->(:B), (:A)-[r:T4]->(:A)");
            //MyEngine.ExecuteQuery("MATCH (:A)-[r]->(:B) RETURN r");

            // CREATE Node with Multiple Properties
            //MyEngine.ExecuteQuery("CREATE (m:Turkish {name: 'Mehmet', surname: 'Bahçeli'})");

            /* Create Feature 1 */
            // CREATE [1] a single node
            //MyEngine.ExecuteQuery("CREATE ()");

            // CREATE [2] two nodes
            //MyEngine.ExecuteQuery("CREATE (), ()");

            // CREATE [3] a single node with a label
            //MyEngine.ExecuteQuery("CREATE (:Label)");

            // CREATE [4] two nodes with same label
            //MyEngine.ExecuteQuery("CREATE (:Label), (:Label)");

            // CREATE [5] a single node with multiple labels
            //MyEngine.ExecuteQuery("CREATE (:A:B:C:D)");

            // CREATE [6] Create three nodes with multiple labels
            //MyEngine.ExecuteQuery("CREATE (:B:A:D), (:B:C), (:D:E:B)");

            // CREATE [7] Create a single node with a property
            //MyEngine.ExecuteQuery("CREATE ({created: true})");

            // CREATE [9] Create a single node with two properties
            //MyEngine.ExecuteQuery("CREATE (n {id: 12, name: 'foo'})");

            /* Create Feature 2 */
            // CREATE [1] two nodes and a single relationship in a single pattern
            //MyEngine.ExecuteQuery("CREATE ()-[:R]->()");
            //MyEngine.ExecuteQuery("CREATE (:A)-[:Re]->(:B)");

            // CREATE [2] two nodes and a single relationship in separate patterns
            //MyEngine.ExecuteQuery("CREATE (a), (b), (a)-[:Rel]->(b)");
            //MyEngine.ExecuteQuery("CREATE (c:Person), (d:Pet), (c)<-[:RUNS]-(d)");

            // NOT SUPPORTED - CREATE [3] two nodes and a single relationship in separate clauses
            //MyEngine.ExecuteQuery("CREATE (a) CREATE (b) CREATE (a)-[:R]->(b)");

            // CREATE [4] two nodes and a single relationship in the reverse direction
            //MyEngine.ExecuteQuery("CREATE (:A)<-[:Rela]-(:B)");
            //MyEngine.ExecuteQuery("CREATE (c:Person)<-[r:RUNSfgdg]-(d:Pet)");

            // CREATE [7] a single node and a single self loop in a single pattern
            //MyEngine.ExecuteQuery("CREATE (root)-[:LINK]->(root)");
            //MyEngine.ExecuteQuery("CREATE (root:Person), (root)<-[:RUNS]-(root), (root)-[:TIMID]->(root)");

            // CREATE [8] a single node and a single self loop in separate patterns
            //MyEngine.ExecuteQuery("CREATE (root), (root)-[:LINK]->(root)");
            //MyEngine.ExecuteQuery("CREATE (root:A)-[:LINK]->(root)");
            //MyEngine.ExecuteQuery("CREATE (root)-[:LINK]->(root:A)");         // TRY THIS, should give an error like: Can't create node `root` with labels or properties here. The variable is already declared in this context

            // CREATE [13] a single relationship with a property
            //MyEngine.ExecuteQuery("CREATE ()-[:R {num: 42}]->()");
            //MyEngine.ExecuteQuery("CREATE (root:Person), (root)<-[:RUNS {num: 42}]-(root), (root)<-[:TIMID {num: 42, name: 'Ahmet'}]-(root)");

            // CREATE [15] a single relationship with two properties
            //MyEngine.ExecuteQuery("CREATE ()-[:R {id: 12, name: 'foo'}]->()");

            /* Match Feature 1 - Match Nodes */
            // MATCH [1] Match non-existent nodes returns empty
            //MyEngine.ExecuteQuery("MATCH (n) RETURN n");

            // MATCH [2] Matching all nodes
            //MyEngine.ExecuteQuery("CREATE (:A), (:B {name: 'b', surname: 'joel'}), ({name: 'c'})");
            //MyEngine.ExecuteQuery("MATCH (n) RETURN n");

            // MATCH [3] Matching nodes using multiple labels
            //MyEngine.ExecuteQuery("CREATE (:A:B:C), (:A:B), (:A:C), (:B:C), (:A), (:B), (:C), ({name: ':A:B:C'}), ({abc: 'abc'}), ()");
            //MyEngine.ExecuteQuery("CREATE (:D:A:B:C)");
            //MyEngine.ExecuteQuery("MATCH (a:A:B) RETURN a");

            // MATCH [4] Simple node inline property predicate
            //MyEngine.ExecuteQuery("CREATE ({name: 'bar'}), ({name: 'monkey'}), ({firstname: 'bar'})");
            //MyEngine.ExecuteQuery("MATCH (n {name: 'bar'}) RETURN n");

            //MyEngine.ExecuteQuery("CREATE (:Person {name: 'Ahmet', Surname: 'Yıldırım'}), ({name: 'Ahmet'}), ({firstname: 'Ahmet', Surname: 'Yıldırım'})");
            //MyEngine.ExecuteQuery("MATCH (n {name: 'Ahmet', Surname: 'Yıldırım'}) RETURN n");

            /* Match Feature 2 - Match Relationships */
            // MATCH [1] Match non-existent relationships returns empty
            //MyEngine.ExecuteQuery("MATCH ()-[r]->() RETURN r");

            // MATCH [2] Matching a relationship pattern using a label predicate on both sides
            //MyEngine.ExecuteQuery("CREATE (:A)-[:T1]->(:B), (:B)-[:T2]->(:A), (:B)-[:T3]->(:B), (:A)-[:T4]->(:A)");
            //MyEngine.ExecuteQuery("MATCH (:A)-[r1]->(:B) RETURN r1");
            //MyEngine.ExecuteQuery("MATCH (:A)<-[r2]-(:B) RETURN r2");
            //MyEngine.ExecuteQuery("MATCH (:A)-[r3]-(:B) RETURN r3");
            //MyEngine.ExecuteQuery("MATCH (:A)<-[r4]->(:B) RETURN r4");

            // MATCH [3] Matching a self-loop with an undirected relationship pattern
            //MyEngine.ExecuteQuery("CREATE (a), (a)-[:T]->(a)");
            //MyEngine.ExecuteQuery("MATCH ()-[r]-() RETURN r");

            // MATCH [4] Matching a self-loop with a directed relationship pattern
            //MyEngine.ExecuteQuery("CREATE (a), (a)-[:T]->(a)");
            //MyEngine.ExecuteQuery("MATCH ()-[r]->() RETURN r");

            /* Where Feature 1 - Filter Single Variable */
            // WHERE [1] Filter node with node label predicate on multi variables with multiple bindings
            //MyEngine.ExecuteQuery("CREATE (:A {id: 0})<-[:ADMIN]-(:B {id: 1})-[:ADMIN]->(:C {id: 2, a: 'A'})");
            //MyEngine.ExecuteQuery("MATCH (a)-[:ADMIN]-(b) WHERE a:A RETURN a.id, b.id");


            MyEngine.ExecuteQuery(@"CREATE (andy:Swedish:Person {name: 'Andy', age: 36, belt: 'white'}), 
            (timothy:Person {name: 'Timothy', age: 25}), (peter:Person {name: 'Peter', age: 35, email: 'peter_n@example.com'}),
            (andy)-[:KNOWS {since: 2012}]->(timothy), (andy)-[:KNOWS {since: 1999}]->(peter)");
            MyEngine.ExecuteQuery("MATCH (n) WHERE n:Swedish RETURN n");


            // NOT SUPPORTED - WHERE [3] Filter node with property predicate on a single variable with multiple bindings
            //MyEngine.ExecuteQuery("CREATE (), ({name: 'Bar'}), (:Bar)");
            //MyEngine.ExecuteQuery("MATCH (n) WHERE n.name = 'Bar' RETURN n");

            /* Return Feature 1 - Return single variable (correct return of values according to their type) */
            // RETURN [1] Returning a list property
            //MyEngine.ExecuteQuery("CREATE ({numbers: [1, 2, 3]})");
            //MyEngine.ExecuteQuery("MATCH (n) RETURN n");

            // RETURN [2] Fail when returning an undefined variable
            //MyEngine.ExecuteQuery("CREATE ({numbers: [1, 2, 3]})");
            //MyEngine.ExecuteQuery("MATCH () RETURN foo");

            /* Return2 - Return single expression (correctly projecting an expression) */
            // RETURN [2] Returning a node property value
            //MyEngine.ExecuteQuery("CREATE ({num: 1})");
            //MyEngine.ExecuteQuery("MATCH (a) RETURN a.num");

            // RETURN [3] Missing node property should become null
            //MyEngine.ExecuteQuery("CREATE ({num: 1})");
            //MyEngine.ExecuteQuery("MATCH (a) RETURN a.name");

            // RETURN [4] Returning a relationship property value
            //MyEngine.ExecuteQuery("CREATE ()-[:T {num: 1}]->())");
            //MyEngine.ExecuteQuery("MATCH ()-[r]->() RETURN r.num");

            // RETURN [5] Missing relationship property should become null
            //MyEngine.ExecuteQuery("CREATE ()-[:T {name: 1}]->()");
            //MyEngine.ExecuteQuery("MATCH ()-[r]->() RETURN r.name2");

            /* Delete - Deleting nodes */
            // DELETE [1] Delete nodes
            //MyEngine.ExecuteQuery("CREATE ()");
            //MyEngine.ExecuteQuery("MATCH (n) DELETE n");

            /*
            MyEngine.ExecuteQuery(@"CREATE (keanu:Person {name: 'Keanu Reever'}), (laurence:Person {name: 'Laurence Fishburne'}),
                                (carrie:Person {name: 'Carrie-Anne Moss'}), (tom:Person {name: 'Tom Hanks'}), (theMatrix:Movie {title: 'The Matrix'}),
                                (keanu)-[:ACTED_IN]->(theMatrix), (laurence)-[:ACTED_IN]->(theMatrix), (carrie)-[:ACTED_IN]->(theMatrix)");
            */


            //MyEngine.ExecuteQuery("MATCH (n) DELETE n");
            //MyEngine.ExecuteQuery("MATCH (n:Person {name: 'Tom Hanks'}) DELETE n");  // THIS MATCH NOT SUPPORTED
            //MyEngine.ExecuteQuery("MATCH (n:Person) DELETE n");
            //MyEngine.ExecuteQuery("MATCH (n {name: 'Tom Hanks'}) DELETE n");
            //MyEngine.ExecuteQuery("MATCH ()-[r:ACTED_IN]->() RETURN r");        // THIS IS NICE
        }
    }
}
