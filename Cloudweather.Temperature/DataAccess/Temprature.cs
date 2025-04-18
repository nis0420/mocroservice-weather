namespace Cloudweather.Temperature.DataAccess;

public class Temprature
{
    public Guid Id { get; set; }
    public decimal TempHighF { get; set; }
    public decimal TempLowF { get; set; }
    public DateTime CreatedOn { get; set; }
    public string ZipCode { get; set; } = string.Empty;
}