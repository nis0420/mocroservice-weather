namespace Cloudweather.Temperature.DataAccess;

public class WeatherReport
{
    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public decimal AverageHighF { get; set; }
    public decimal AverageLowF { get; set; }
    public decimal RainfallTotalInches { get; set; }
    public decimal SnowfallTotalInches { get; set; }
    public string ZipCode { get; set; } = string.Empty;
}