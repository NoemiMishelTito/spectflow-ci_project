Feature: UpdateAProduct

A short summary of the feature

@put @get @post @mod3
Scenario: Update a product
  Given I am authenticated with a valid token
  When I send a PUT request for product for ID 19
  Then I expect to receive a valid response with status code OK after updated
  And the product with ID 19 has been successfully updated