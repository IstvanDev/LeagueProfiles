namespace LeagueProfiles.Models
{
    public class ChampionPlayer
    {
        public int PlayerId { get; set; }

        public int ChampionId { get; set; }

        public Champion Champion { get; set; }

        public Player Player { get; set; }
    }
}
