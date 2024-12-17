Feature: Verify Language Change Functionality
  As a user
  I want to change the website language to Lithuanian
  So that I can view the website content in Lithuanian

  Scenario: Successful language change to Lithuanian
    Given I open the EHU homepage at "https://en.ehu.lt"
    When I switch the language to Lithuanian
    Then the current URL should be "https://lt.ehu.lt/"
