Feature: ShowAProduct

A short summary of the feature

@get @mod3
Scenario: Show a product
  When I send a GET request a product for ID 19
  Then I expect to receive a valid response with status code OK productId
  And the response body contains the product details for ID 19

