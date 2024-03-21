using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using BirthCenterPatientAPI.Enums;
using BirthCenterPatientAPI.Models;

Console.WriteLine("Starting application...");

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

var apiUrl = config["AppSettings:ApiBaseUrl"];
var apiRoute = "api/Patients/postman-collection";

if (apiUrl == null)
{
    Console.WriteLine("API URL is not specified in appsettings.json");
    return;
}

using var httpClient = new HttpClient();

try
{
    // Check the availability of the Api
    var response = await httpClient.GetAsync($"{apiUrl}/{apiRoute}");
    if (!response.IsSuccessStatusCode)
    {
        Console.WriteLine($"Failed to connect to PatientApi. Error: {response.StatusCode}");
        return;
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Failed to connect to PatientApi. Error: {ex.Message}");
    return;
}

Console.WriteLine("Starting generating...");

var patient = new Patient
{
    Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
    FirstName = "John",
    LastName = "Smith",
    Patronymic = null,
    Gender = Gender.Male,
    BirthDate = DateTime.Now.AddYears(-30),
    Active = true
};

await AddPatientAsync(httpClient, apiUrl, patient);

List<Patient> patients = GeneratePatients(99);

foreach (var randomPatient in patients)
{
    await AddPatientAsync(httpClient, apiUrl, randomPatient);
}

Console.WriteLine("Generating finished.");

async Task AddPatientAsync(HttpClient httpClient, string apiUrl, Patient patient)
{
    var jsonPatient = JsonSerializer.Serialize(patient);
    var content = new StringContent(jsonPatient, Encoding.UTF8, "application/json");

    var response = await httpClient.PostAsync($"{apiUrl}/api/patients", content);

    if (response.IsSuccessStatusCode)
    {
        Console.WriteLine($"Patient {patient.FirstName} {patient.LastName} added successfully.");
    }
    else
    {
        var errorMessage = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Failed to add patient {patient.FirstName} {patient.LastName}. Error: {response.StatusCode}, {errorMessage}");
    }
}

static List<Patient> GeneratePatients(int count)
{
    var patients = new List<Patient>();

    for (int i = 0; i < count; i++)
    {
        var patient = new Patient
        {
            FirstName = GenerateRandomString(8),
            LastName = GenerateRandomString(10),
            Patronymic = GenerateRandomString(10),
            Gender = (Gender)new Random().Next(0, 3),
            BirthDate = DateTime.Now.AddDays(-new Random().Next(1, 365)),
            Active = true
        };

        patients.Add(patient);
    }

    return patients;
}

static string GenerateRandomString(int length)
{
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    var random = new Random();
    return new string(Enumerable.Repeat(chars, length)
        .Select(s => s[random.Next(s.Length)]).ToArray());
}
