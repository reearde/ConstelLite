Feature: MatchWhere1 - Filter single variable

  Scenario: [1] Filter node with node label predicate on multi variables with multiple bindings
    Given an empty graph
    And having executed: "CREATE (andy:Swedish:Person {name: 'Andy', age: 36, belt: 'white'}), (timothy:Person {name: 'Timothy', age: 25}), (peter:Person {name: 'Peter', age: 35, email: 'peter_n@example.com'}), (andy)-[:KNOWS {since: 2012}]->(timothy), (andy)-[:KNOWS {since: 1999}]->(peter)"
    When executing query: "MATCH (n) WHERE n:Swedish RETURN n"
    Then the result should be, in any order:
      | n |
      | (0:Swedish:Person {name: 'Andy', age: 36, belt: 'white'}) |