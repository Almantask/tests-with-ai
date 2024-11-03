Feature: Healthcheck

Scenario: Healthcheck returns 200 OK
	When I send a GET request to /healthcheck
	Then the response should be 200