Feature: GetUserTests

Scenario: GET - List Users
	Given the system has a list of users
	When the application sends a GET request for list of users
	Then the API should return a list of users

Scenario: GET - Check Single User By Name
	Given the system has a list of users
	When the application sends a GET request for list of users
	Then the following user should be returned
		| FirstName | LastName |
		| Lindsay   | Ferguson |

Scenario Outline: GET - Check Single User By Id
	Given the system has a list of users
	When the application sends a GET request for a user with ID '<Id>'
	Then the user information should be returned with the corresponding error code '<ErrorCode>'

	Examples: 
		| Id | ErrorCode |
		| 2  | OK        |
		| 23 | NotFound  |

Scenario Outline: POST - Add New User Record
	Given the system has a list of users
	When the application sends a POST request to add a new user with name '<Name>' and job '<Job>'
	Then the user should be created successfully

	Examples: 
		| Name     | Job    |
		| morpheus | leader |
		| jm       | sdet   |


Scenario Outline: PUT - Update User Record
	Given the system has a list of users
	When the application sends a PUT request to update a user
		| Id   | Name   | Job   |
		| <Id> | <Name> | <Job> |
	Then the user should be updated successfully

	Examples: 
		| Id | Name     | Job           |
		| 2  | morpheus | zion resident |

Scenario Outline: PATCH - Update User Record
	Given the system has a list of users
	When the application sends a PATCH request to update a user
		| Id   | Name   | Job   |
		| <Id> | <Name> | <Job> |
	Then the user should be updated successfully

	Examples: 
		| Id | Name     | Job           |
		| 2  | morpheus | zion resident |

Scenario Outline: DELETE - Delete User Record
	Given the system has a list of users
	When the application sends a DELETE request to delete a user with Id '<Id>'
	Then the user with Id '<Id>' should be deleted successfully

	Examples: 
		| Id | 
		| 2  | 
