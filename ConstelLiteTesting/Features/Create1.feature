Feature: Create1 - Creating nodes

  Scenario: [1] Create a single node
    Given an empty graph
    When executing query: "CREATE ()"
    Then the node size should be: 1

  Scenario: [2] Create two nodes
    Given an empty graph
    When executing query: "CREATE (), ()"
    Then the node size should be: 2

  Scenario: [3] Create a single node with a label
    Given an empty graph
    When executing query: "CREATE (:Label)"
    Then the node size should be: 1

  Scenario: [4] Create two nodes with same label
    Given an empty graph
    When executing query: "CREATE (:Label), (:Label)"
    Then the node size should be: 2

  Scenario: [5] Create a single node with multiple labels
    Given an empty graph
    When executing query: "CREATE (:A:B:C:D)"
    Then the node size should be: 1

  Scenario: [6] Create three nodes with multiple labels
    Given an empty graph
    When executing query: "CREATE (:B:A:D), (:B:C), (:D:E:B)"
    Then the node size should be: 3

  Scenario: [7] Create a single node with a property
    Given an empty graph
    When executing query: "CREATE ({created: true})"
    Then the node size should be: 1

  Scenario: [9] Create a single node with two properties
    Given an empty graph
    When executing query: "CREATE (n {id: 12, name: 'foo'})"
    Then the node size should be: 1