Feature: Verify Navigation to About Page
  As a user
  I want to navigate to the About page on the EHU website
  So that I can view the information about EHU

  Scenario: Successful navigation to About page
    Given I open the EHU homepage at "https://en.ehu.lt"
    When I navigate to the About page
    Then the current URL should be "https://en.ehu.lt/about/"
    And the page title should be "About"
    And the page header should be "About"
