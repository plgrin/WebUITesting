Feature: Language Change on EHU Website

A user changes the website language to view content in their preferred language.

@language
Scenario: Change language to Lithuanian
    Given I am on the homepage
    When I change the language to "Lithuanian"
    Then I should be redirected to the Lithuanian version of the site
