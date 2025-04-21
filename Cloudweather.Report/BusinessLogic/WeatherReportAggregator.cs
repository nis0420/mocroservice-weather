using System.Text.Json;
using Cloudweather.Temperature.DataAccess;

namespace Cloudweather.Report.BusinessLogic;

public interface IWeatherReportAggregator
{
    Task<WeatherReport> BuildReportAsync(string zip, int days);
}

public class WeatherReportAggregator : IWeatherReportAggregator
{
    private readonly IHttpClientFactory _http;
    private readonly ILogger<WeatherReportAggregator> _logger;
    private readonly WeatherDataConfig _weatherDataConfig;
    private readonly WeatherReportDbContext _db;

    public WeatherReportAggregator(IHttpClientFactory http, ILogger<WeatherReportAggregator> logger, WeatherDataConfig weatherDataConfig, WeatherReportDbContext db)
    {
        _http = http;
        _logger = logger;
        _weatherDataConfig = weatherDataConfig;
        _db = db;
    }

    public async Task<WeatherReport> BuildReportAsync(string zip, int days)
    {
        var httpClient = _http.CreateClient();

        var precipData = await FetchPrecipitationDataAsync(httpClient, zip, days);

        var totalRain = GetTotalRain(precipData);
        var totalSnow = GetTotalSnow(precipData);

        _logger.LogInformation($"Total Rain: {totalRain} inches, Total Snow: {totalSnow} inches");

        var tempData = await FetchTemperatureDataAsync(httpClient, zip, days);
        var averageHighTemp = tempData.Average(t => t.TempHighF);
        var averageLowTemp = tempData.Average(t => t.TempLowF);

        _logger.LogInformation($"Average High Temp: {averageHighTemp} F, Average Low Temp: {averageLowTemp} F");

        var weatherReport = new WeatherReport{
            AverageHighF = Math.Round(averageHighTemp, 1),
            AverageLowF = Math.Round(averageLowTemp, 1),
            RainfallTotalInches = totalRain,
            SnowfallTotalInches = totalSnow,
            ZipCode = zip,
            CreatedOn = DateTime.UtcNow,
        };

        _db.Add(weatherReport);
        await _db.SaveChangesAsync();

        return weatherReport;
    }
    

    private static decimal GetTotalSnow(IEnumerable<PrecipitationModel> precipData)
    {
        var totalSnow = precipData.Where(p => p.WeatherType == "Snow")
            .Sum(p => p.AmountInches);
        return Math.Round(totalSnow, 1);
    }
    private static decimal GetTotalRain(IEnumerable<PrecipitationModel> precipData)
    {
        var totalRain = precipData.Where(p => p.WeatherType == "Rain")
            .Sum(p => p.AmountInches);
        return Math.Round(totalRain, 1);
    }

    private async Task<List<TemperatureModel>> FetchTemperatureDataAsync(HttpClient httpClient, string zip, int days)
    {
        var endpoint = BuildTemperatureEndpoint(zip, days);
        var tempratureRecords = await httpClient.GetAsync(endpoint);
        var jsonSerializer = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        var tempratureData = await tempratureRecords.Content.ReadFromJsonAsync<List<TemperatureModel>>(jsonSerializer);
        return tempratureData ?? new List<TemperatureModel>();
    }

    private async Task<List<PrecipitationModel>> FetchPrecipitationDataAsync(HttpClient httpClient, string zip, int days)
    {
        var endpoint = BuildPrecipitationEndpoint(zip, days);
        var precipRecords = await httpClient.GetAsync(endpoint);
        var jsonSerializer = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        var precipData = await precipRecords.Content.ReadFromJsonAsync<List<PrecipitationModel>>(jsonSerializer);
        return precipData ?? new List<PrecipitationModel>();
    }

    private string BuildTemperatureEndpoint(string zip, int days)
    {
        return $"{_weatherDataConfig.TempDataProtocol}://{_weatherDataConfig.TempDataHost}:{_weatherDataConfig.TempDataPort}/api/temperature/{zip}/{days}";
    }
    private string BuildPrecipitationEndpoint(string zip, int days)
    {
        return $"{_weatherDataConfig.PrecipDataProtocol}://{_weatherDataConfig.PrecipDataHost}:{_weatherDataConfig.PrecipDataPort}/api/precipitation/{zip}/{days}";
    }


}