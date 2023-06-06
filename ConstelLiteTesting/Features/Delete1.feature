Feature: Delete1 - Deleting nodes

  Scenario: [1] Delete node
    Given an empty graph
    And having executed: "CREATE ()"
    When executing query: "MATCH (n) DELETE n"
    Then the node size should be: 0

  Scenario: [2] Delete multiple nodes
    Given an empty graph
    And having executed: "CREATE (), (), (), (), ()"
    When executing query: "MATCH (n) DELETE n"
    Then the node size should be: 0