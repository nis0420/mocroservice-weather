namespace Cloudweather.Report.BusinessLogic;


internal class PrecipitationModel
{
    public DateTime CreatedOn { get; set; }
    public string WeatherType { get; set; } = string.Empty;
    public decimal AmountInches { get; set; }
    public string ZipCode { get; set; } = string.Empty;
}