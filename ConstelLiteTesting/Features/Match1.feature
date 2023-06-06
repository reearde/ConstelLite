Feature: Match1 - Match nodes

  Scenario: [1] Match non-existent nodes returns empty
    Given an empty graph
    When executing query: "MATCH (n) RETURN n"
    Then the result should be, in any order:
      | n |

  Scenario: [2] Matching all nodes
    Given an empty graph
    And having executed: "CREATE (:A), (:B {name: 'b'}), ({name: 'c'})"
    When executing query: "MATCH (n) RETURN n"
    Then the result should be, in any order:
      | n                |
      | (:A)             |
      | (:B {name: 'b'}) |
      | ({name: 'c'})    |

  Scenario: [3] Matching nodes using multiple labels
    Given an empty graph
    And having executed: "CREATE (:A:B:C), (:A:B), (:A:C), (:B:C), (:A), (:B), (:C), ({name: ':A:B:C'}), ({abc: 'abc'}), ()"
    When executing query: "MATCH (a:A:B) RETURN a"
    Then the result should be, in any order:
      | a        |
      | (:A:B)   |
      | (:A:B:C) |

  Scenario: [4] Simple node inline property predicate
    Given an empty graph
    And having executed: "CREATE ({name: 'bar'}), ({name: 'monkey'}), ({firstname: 'bar'})"
    When executing query: "MATCH (n {name: 'bar'}) RETURN n"
    Then the result should be, in any order:
      | n               |
      | ({name: 'bar'}) |