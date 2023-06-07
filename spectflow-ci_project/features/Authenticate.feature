Feature: Authenticate

A short summary of the feature

@post @mod3
Scenario: Authenticate
  Given I have the following credentials:
    | username | password |
    | admin    | admin    |
  When I send a POST request to authenticate
  Then I expect to receive a valid response with status code OK in authenthication
  And the response body contains an authentication token
