using System.Net.Http.Json;
using Cloudweather.Report.BusinessLogic;
using Microsoft.Extensions.Configuration;

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables()
    .Build();

var serviceConfig = configuration.GetSection("Services");

var tempServiceConfig = serviceConfig.GetSection("Temperature");
var tempServiceHost = tempServiceConfig["Host"];
var tempServicePort = tempServiceConfig["Port"];

var precipServiceConfig = serviceConfig.GetSection("Precipitation");
var precipServiceHost = precipServiceConfig["Host"];
var precipServicePort = precipServiceConfig["Port"];

var zipCodes = new List<string> { "10001", "20001", "30301", "60601", "70112", "94105" };

Console.WriteLine("Starting data loader...");

var tempHttpClient = new HttpClient();
tempHttpClient.BaseAddress = new Uri($"http://{tempServiceHost}:{tempServicePort}");

var precipHttpClient = new HttpClient();
precipHttpClient.BaseAddress = new Uri($"http://{precipServiceHost}:{precipServicePort}");

foreach (var zip in zipCodes)
{
    Console.WriteLine($"Loading temperature data for {zip}...");
    var from = DateTime.Now.AddDays(-2);
    var to = DateTime.Now;

    for (var date = from; date <= to; date = date.AddDays(1))
    {
        var temp = PostTemp(zip, date, tempHttpClient);
        PostPrecip(temp[0], zip, date, precipHttpClient);
    }
}


void PostPrecip(int lowTemp, string zip, DateTime day, HttpClient httpClient){
    var rand = new Random();
    var isPrecip = rand.Next(2) < 1;
    PrecipitationModel precipitation;

    if (isPrecip)
    {
        var precipInches = rand.Next(1, 16);

        if (lowTemp < 32){
            precipitation = new PrecipitationModel
            {
                ZipCode = zip,
                AmountInches = precipInches,
                CreatedOn = day,
                WeatherType = "Snow",
            };
        }
        else
        {
            precipitation = new PrecipitationModel
            {
                ZipCode = zip,
                AmountInches = precipInches,
                CreatedOn = day,
                WeatherType = "Rain",
            };
        }
    }
    else
    {
        precipitation = new PrecipitationModel
        {
            ZipCode = zip,
            AmountInches = 0,
            CreatedOn = day,
            WeatherType = "none",
        };
    }

    var precipResonse = precipHttpClient.PostAsJsonAsync("observation", precipitation).Result;

    if(precipResonse.IsSuccessStatusCode)
    {
        Console.WriteLine($"Precipitation data for {zip} on {day.ToShortDateString()} posted successfully.");
    }
    else
    {
        Console.WriteLine($"Failed to post precipitation data for {zip} on {day.ToShortDateString()}.");
    }
}


List<int> PostTemp(string zip, DateTime day, HttpClient httpClient)
{
    var rand = new Random();
    var highTemp = rand.Next(80, 110);
    var lowTemp = rand.Next(40, highTemp);

    var temperature = new TemperatureModel
    {
        ZipCode = zip,
        TempHighF = highTemp,
        TempLowF = lowTemp,
        CreatedOn = day,
    };

    var tempResponse = httpClient.PostAsJsonAsync("observation", temperature).Result;

    if (tempResponse.IsSuccessStatusCode)
    {
        Console.WriteLine($"Temperature data for {zip} on {day.ToShortDateString()} posted successfully.");
    }
    else
    {
        Console.WriteLine($"Failed to post temperature data for {zip} on {day.ToShortDateString()}.");
    }

    return [lowTemp, highTemp];
}