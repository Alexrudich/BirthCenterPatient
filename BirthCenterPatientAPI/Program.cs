using BirthCenterPatientAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PatientContext>(options =>
    options.UseSqlite(connectionString)); //used UseSqlServer on my local machine for testing

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Patient API V0");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
