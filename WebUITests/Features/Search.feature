Feature: Search on EHU Website

A user uses the search functionality to find relevant information.

@search
Scenario: Perform a search
    Given I am on the homepage
    When I perform a search for "study programs"
    Then I should see search results containing "study program"
