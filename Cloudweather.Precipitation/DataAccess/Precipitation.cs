namespace Cloudweather.Precipitation.DataAccess;

public class Precipitation
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public decimal AmountInches { get; set; }
    public string WeatherType { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
}