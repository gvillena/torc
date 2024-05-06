# Torc Take Home Assesment Solution 

![image](https://github.com/gvillena/torc/assets/12707294/bfa69ecc-667c-4498-b1bb-a175502cc0ef)

## Description
This project sets up a Kubernetes cluster to host a Book Library application consisting of various microservices, including a database, API backend, a messaging hub, and a single-page application (SPA). Each microservice is deployed as a separate deployment within the Kubernetes cluster and exposed through corresponding services. This project represents the solution for a take-home assessment as part as the selection process for a Senior Software Engineer position at Torc. 

## Components
1. **sqldata (StatefulSet):**
   - Manages the SQL Server database for the Book Library application.
   - Exposes the database service internally using a ClusterIP service.

2. **booklibrary-api (Deployment):**
   - Hosts the API backend for the Book Library application.
   - Communicates with the database service.
   - Exposes an HTTP endpoint.
   - Configured with environment variables for database connection and Azure Service Bus settings.
   - Exposed internally using a ClusterIP service and externally using a LoadBalancer service for Swagger documentation.

3. **booklibrary-hub (Deployment):**
   - Acts as a messaging hub for the Book Library application.
   - Configured with environment variables for Azure Service Bus settings.
   - Exposed internally using a ClusterIP service.

4. **booklibrary-spa (Deployment):**
   - Contains the single-page application frontend for the Book Library application.
   - Exposed externally using a LoadBalancer service.
   - **[Live Demo](http://172.171.172.7)**

## Services
- **sqldata:** Internal ClusterIP service exposing the SQL database.
- **booklibrary-api:** Internal ClusterIP service exposing the API backend, and a LoadBalancer service exposing the Swagger documentation externally.
- **booklibrary-hub:** Internal ClusterIP service exposing the messaging hub.
- **booklibrary-spa:** LoadBalancer service exposing the single-page application frontend externally.

## Usage
1. Deploy the Kubernetes resources using the provided YAML files.
2. Access the services as per your requirements:
   - Access the API backend internally via the `booklibrary-api` service.
   - Access the Swagger documentation externally via the `booklibrary-api-swagger` service.
   - Access the messaging hub internally via the `booklibrary-hub` service.
   - Access the single-page application frontend externally via the `booklibrary-spa` service.

## Notes
- Ensure appropriate configurations for environment variables, such as database connection strings and Azure Service Bus settings, are provided before deployment.
- Adjust the replicas and resource allocations in the deployments as per your application's requirements.

## Author
Giancarlo Villena

## License
None
