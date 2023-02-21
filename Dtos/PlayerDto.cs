namespace LeagueProfiles.Dtos
{
    public class PlayerDto
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public string Role { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }
}
