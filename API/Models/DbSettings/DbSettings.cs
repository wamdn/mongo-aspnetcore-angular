namespace API.Models.DbSettings;
public class DbSettings
{
    public string? ConnectionString { get; set; }
    public string? DbName { get; set; }
    public CollectionsSettings? Collections { get; set; }
}
