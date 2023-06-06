Feature: Match2 - Match relationships

  Scenario: [1] Match non-existent relationships returns empty
    Given an empty graph
    When executing query: "MATCH ()-[r]->() RETURN r"
    Then the result should be, in any order:
      | r |

  Scenario: [2] Matching a relationship pattern using a label predicate on both sides
    Given an empty graph
    And having executed: "CREATE (:A)-[:T1]->(:B), (:B)-[:T2]->(:A), (:B)-[:T3]->(:B), (:A)-[:T4]->(:A)"
    When executing query: "MATCH (:A)-[r]->(:B) RETURN r"
    Then the result should be, in any order:
      | r     |
      | [:T1] |

  Scenario: [3] Matching a self-loop with an undirected relationship pattern
    Given an empty graph
    And having executed: "CREATE (a), (a)-[:T]->(a)"
    When executing query: "MATCH ()-[r]-() RETURN r"
    Then the result should be, in any order:
      | r   |
      | 'T' |

  Scenario: [4] Matching a self-loop with a directed relationship pattern
    Given an empty graph
    And having executed: "CREATE (a), (a)-[:T]->(a)"
    When executing query: "MATCH ()-[r]->() RETURN r"
    Then the result should be, in any order:
      | r   |
      | 'T' |