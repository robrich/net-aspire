namespace AspireFull.ApiService.Data;

[Table(name: "weatherstations", Schema = "public")]
public class WeatherStation
{
    [Key]
    [StringLength(5)]
    [Column("id")]
    public string Id { get; set; } = "";
    [StringLength(50)]
    [Column("name")]
    public string Name { get; set; } = "";
    [StringLength(50)]
    [Column("timezone")]
    public string Timezone { get; set; } = "";
    [StringLength(2)]
    [Column("country")]
    public string Country { get; set; } = "";
    [Column("latitude")]
    public double Latitude { get; set; }
    [Column("longitude")]
    public double Longitude { get; set; }
    [Column("elevation")]
    public int Elevation { get; set; }
}
