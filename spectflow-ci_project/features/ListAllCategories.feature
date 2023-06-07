Feature: ListAllCategories

A short summary of the feature

@get @mod3
Scenario: List all categories
  When I send a GET request category
  Then I expect to receive a valid response with status code OK
  And the response body contains a list of categories
