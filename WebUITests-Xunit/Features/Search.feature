Feature: Verify Search Functionality
  As a user
  I want to use the search functionality on the EHU website
  So that I can find relevant information

  Scenario: Successful search for a term
    Given I open the EHU homepage at "https://en.ehu.lt"
    When I perform a search with the term "study programs"
    Then the search results page URL should contain "/?s=study+programs"
    And search results should be present
    And search results should contain the term "study program"
