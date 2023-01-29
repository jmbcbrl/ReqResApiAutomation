Feature: GetColorTests

Scenario: GET - List Colors
	Given the system has a list of colors
	When the application sends a GET request for list of colors
	Then the API should return a list of colors

Scenario Outline: GET - Check Single Color By Id
	Given the system has a list of colors
	When the application sends a GET request for a color with ID '<Id>'
	Then the color information should be returned with the corresponding error code '<ErrorCode>'

	Examples: 
		| Id | ErrorCode |
		| 2  | OK        |
		| 23 | NotFound  |