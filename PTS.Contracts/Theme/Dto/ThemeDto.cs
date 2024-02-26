namespace PTS.Contracts.Theme.Dto;
public class ThemeDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsBanned { get; set; }
    public List<ThemeDto>? SubThemes { get; set; }
}
