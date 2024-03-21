## Using the API

### API Resources

- `api/BirthCenterPatientAPI`: This endpoint provides access to CRUD operations for patients.

### Import Postman Collection

1. **Ensure the application is running:**
- Make sure the Patient API application is running.

2. **Import the Postman collection:**
- Open Postman.
- Click on "Import" and select "Link".
- Enter the following link:
  ```
  http://localhost:{YourRunningPort}/api/Patients/postman-collection
  ```
- Click "Continue" and then "Import".

3. **Load entities to the db:**
- Change appsettings.json for your local machine, for example:
`DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PatientDb;Trusted_Connection=True;MultipleActiveResultSets=true`
- Open PowerShell instance from this directory ~BirthCenter\BirthCenterPatientAPI
- Enter the following command:
  ```
  dotnet run
  ```
- Run BirthCenterPatientAPI.PatientDataLoader project.
## Additional Materials

- API documentation is available via [Swagger](http://localhost:5005/swagger/index.html).
