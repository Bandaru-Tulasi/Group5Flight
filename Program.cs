using Group5Flight.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();

var dbPath = Path.Combine(builder.Environment.ContentRootPath, "Group5Flights.db");
builder.Services.AddDbContext<AirBnBContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AirBnBContext>();
    
    var dbDirectory = Path.GetDirectoryName(dbPath);
    if (!string.IsNullOrEmpty(dbDirectory) && !Directory.Exists(dbDirectory))
    {
        Directory.CreateDirectory(dbDirectory);
    }
    
    context.Database.EnsureCreated();
    
    if (!context.Airlines.Any())
    {
        var delta = new Airline { Name = "Delta Airlines", ImageName = "delta.png" };
        var united = new Airline { Name = "United Airlines", ImageName = "united.png" };
        var american = new Airline { Name = "American Airlines", ImageName = "american.png" };
        
        context.Airlines.AddRange(delta, united, american);
        context.SaveChanges();

        context.Flights.AddRange(
            new Flight
            {
                FlightCode = "DL1001",
                From = "New York",
                To = "Chicago",
                Date = DateTime.Today.AddDays(1),
                DepartureTime = DateTime.Today.AddDays(1).AddHours(8),
                ArrivalTime = DateTime.Today.AddDays(1).AddHours(10).AddMinutes(30),
                CabinType = "Economy",
                Emission = "Low",
                AircraftType = "Airbus 320 Family",
                Price = 199.99m,
                AirlineId = delta.AirlineId
            },
            new Flight
            {
                FlightCode = "DL2002",
                From = "Chicago",
                To = "Los Angeles",
                Date = DateTime.Today.AddDays(2),
                DepartureTime = DateTime.Today.AddDays(2).AddHours(10),
                ArrivalTime = DateTime.Today.AddDays(2).AddHours(13),
                CabinType = "Business",
                Emission = "Medium",
                AircraftType = "Boeing 737 Family",
                Price = 499.99m,
                AirlineId = delta.AirlineId
            },
            new Flight
            {
                FlightCode = "UA101",
                From = "New York",
                To = "Miami",
                Date = DateTime.Today.AddDays(1),
                DepartureTime = DateTime.Today.AddDays(1).AddHours(9),
                ArrivalTime = DateTime.Today.AddDays(1).AddHours(12),
                CabinType = "Economy Plus",
                Emission = "Low",
                AircraftType = "Airbus 320 Family",
                Price = 249.99m,
                AirlineId = united.AirlineId
            },
            new Flight
            {
                FlightCode = "UA202",
                From = "Chicago",
                To = "Dallas",
                Date = DateTime.Today.AddDays(3),
                DepartureTime = DateTime.Today.AddDays(3).AddHours(7),
                ArrivalTime = DateTime.Today.AddDays(3).AddHours(9).AddMinutes(30),
                CabinType = "Basic Economy",
                Emission = "High",
                AircraftType = "Boeing 737 Family",
                Price = 149.99m,
                AirlineId = united.AirlineId
            },
            new Flight
            {
                FlightCode = "AA303",
                From = "Los Angeles",
                To = "Seattle",
                Date = DateTime.Today.AddDays(1),
                DepartureTime = DateTime.Today.AddDays(1).AddHours(14),
                ArrivalTime = DateTime.Today.AddDays(1).AddHours(16).AddMinutes(30),
                CabinType = "Economy",
                Emission = "Medium",
                AircraftType = "Airbus 320 Family",
                Price = 179.99m,
                AirlineId = american.AirlineId
            },
            new Flight
            {
                FlightCode = "AA404",
                From = "New York",
                To = "Miami",
                Date = DateTime.Today.AddDays(2),
                DepartureTime = DateTime.Today.AddDays(2).AddHours(6),
                ArrivalTime = DateTime.Today.AddDays(2).AddHours(9),
                CabinType = "Business",
                Emission = "Low",
                AircraftType = "Boeing 737 Family",
                Price = 399.99m,
                AirlineId = american.AirlineId
            },
            new Flight
            {
                FlightCode = "DL3003",
                From = "Seattle",
                To = "Chicago",
                Date = DateTime.Today.AddDays(3),
                DepartureTime = DateTime.Today.AddDays(3).AddHours(12),
                ArrivalTime = DateTime.Today.AddDays(3).AddHours(16),
                CabinType = "Economy",
                Emission = "Medium",
                AircraftType = "Airbus 320 Family",
                Price = 299.99m,
                AirlineId = delta.AirlineId
            },
            new Flight
            {
                FlightCode = "UA505",
                From = "Dallas",
                To = "Los Angeles",
                Date = DateTime.Today.AddDays(1),
                DepartureTime = DateTime.Today.AddDays(1).AddHours(15),
                ArrivalTime = DateTime.Today.AddDays(1).AddHours(17).AddMinutes(30),
                CabinType = "Economy Plus",
                Emission = "Low",
                AircraftType = "Airbus 320 Family",
                Price = 229.99m,
                AirlineId = united.AirlineId
            }
        );
        
        context.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();