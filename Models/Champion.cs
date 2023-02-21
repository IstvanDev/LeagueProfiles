using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LeagueProfiles.Models
{
    public class Champion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;

        public ICollection<Player> Players { get; set; }

        //public ICollection<ChampionPlayer> ChampionPlayers { get; set; }
    }
}
