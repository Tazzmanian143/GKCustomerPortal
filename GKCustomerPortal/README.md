# Customer Portal API

This repository contains the .NET Technical Assessment for a Customer Portal API. It features a RESTful architecture, CQRS/Service pattern implementation, SQLite integration, PII Data Encryption (AES-256), and Basic Authentication.

## How to Run Locally using Docker

1. Ensure you have Docker and Docker Desktop installed and running.
2. Open a terminal and navigate to the root directory of this repository (where the `docker-compose.yml` file is located).
3. Run the following command to build and start the containers:
   ```bash
   docker-compose up -d --build
4. The API will now be accessible at http://localhost:8080.

API Documentation & Testing
This API utilizes Swagger/OpenAPI for documentation and endpoint testing.

Once the application is running via Docker, navigate to http://localhost:8080/swagger in your web browser. (Note: If running directly via Visual Studio instead of Docker, check your launch console for the assigned localhost port).

Authentication: The API is secured with Basic Authentication. To test the endpoints, click the "Authorize" button in the top right corner of the Swagger UI.

Login Credentials: Enter the following seeded credentials to authenticate:

Username: admin@gk.com

Password: Password123!
