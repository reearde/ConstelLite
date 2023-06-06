Feature: Return2 - Return single expression (correctly projecting an expression)

  Scenario: [1] Returning a node property value
    Given an empty graph
    And having executed: "CREATE ({num: 1})"
    When executing query: "MATCH (a) RETURN a.num"
    Then the result should be, in any order:
      | a.num |
      | 1     |

  Scenario: [2] Missing node property should become null
    Given an empty graph
    And having executed: "CREATE ({num: 1})"
    When executing query: "MATCH (a) RETURN a.name"
    Then the result should be, in any order:
      | a.name |
      | null   |

  Scenario: [3] Returning a relationship property value
    Given an empty graph
    And having executed: "CREATE ()-[:T {num: 1}]->())"
    When executing query: "MATCH ()-[r]->() RETURN r.num"
    Then the result should be, in any order:
      | r.num |
      | 1     |

  Scenario: [5] Missing relationship property should become null
    Given an empty graph
    And having executed: "CREATE ()-[:T {name: 1}]->()"
    When executing query: "MATCH ()-[r]->() RETURN r.name2"
    Then the result should be, in any order:
      | r.name2 |
      | null    |