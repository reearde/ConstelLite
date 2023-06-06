Feature: Return1 - Return single variable (correct return of values according to their type)

  Scenario: [1] Returning a list property
    Given an empty graph
    And having executed: "CREATE ({numbers: [1, 2, 3]})"
    When executing query: "MATCH (n) RETURN n"
    Then the result should be, in any order:
      | n                      |
      | ({numbers: [1, 2, 3]}) |

  Scenario: [2] Fail when returning an undefined variable
    Given an empty graph
    And having executed: "CREATE ({numbers: [1, 2, 3]})"
    When executing query: "MATCH () RETURN foo"
    Then a SyntaxError should be raised at compile time: UndefinedVariable
