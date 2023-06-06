Feature: Create2 - Creating relationships

  Scenario: [1] Create two nodes and a single relationship in a single pattern
    Given an empty graph
    When executing query: "CREATE ()-[:R]->()"
    Then the node size should be: 2

  Scenario: [2] Create two nodes and a single relationship in separate patterns
    Given an empty graph
    When executing query: "CREATE (a), (b), (a)-[:R]->(b)"
    Then the node size should be: 2

  Scenario: [3] Create two nodes and a single relationship in separate clauses
    Given an empty graph
    When executing query: "CREATE (a) CREATE (b) CREATE (a)-[:R]->(b)"
    Then the node size should be: 2

  Scenario: [4] Create two nodes and a single relationship in the reverse direction
    Given an empty graph
    When executing query: "CREATE (:A)<-[:R]-(:B)"
    Then the node size should be: 2

  Scenario: [7] Create a single node and a single self loop in a single pattern
    Given an empty graph
    When executing query: "CREATE (root)-[:LINK]->(root)"
    Then the node size should be: 1

  Scenario: [8] Create a single node and a single self loop in separate patterns
    Given an empty graph
    When executing query: "CREATE (root), (root)-[:LINK]->(root)"
    Then the node size should be: 1

  Scenario: [13] Create a single relationship with a property
    Given an empty graph
    When executing query: "CREATE ()-[:R {num: 42}]->()"
    Then the node size should be: 2

  Scenario: [15] Create a single relationship with two properties
    Given an empty graph
    When executing query: "CREATE ()-[:R {id: 12, name: 'foo'}]->()"
    Then the node size should be: 2
