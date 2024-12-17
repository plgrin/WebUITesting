Feature: Navigation and Search on EHU Website

A user navigates through the website and uses its search functionality to access the information they need.

Scenario: Navigate to About Page
    Given I am on the homepage
    When I navigate to the "About" page
    Then I should see the "About" page header

Scenario: Perform a search
    Given I am on the homepage
    When I perform a search for "study programs"
    Then I should see search results containing "study program"

Scenario: Change language to Lithuanian
    Given I am on the homepage
    When I change the language to "Lithuanian"
    Then I should be redirected to the Lithuanian version of the site
