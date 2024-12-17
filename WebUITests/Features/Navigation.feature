Feature: Navigation on EHU Website

A user navigates through the website to access specific pages.

@navigation
Scenario: Navigate to About Page
    Given I am on the homepage
    When I navigate to the "About" page
    Then I should see the "About" page header
