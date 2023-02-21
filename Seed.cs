using LeagueProfiles.Data;
using LeagueProfiles.Models;

namespace LeagueProfiles
{
    public class Seed
    {
        private readonly DataContext dataContext;
        public Seed(DataContext context)
        {
            this.dataContext = context;
        }
        public void SeedDataContext()
        {
            //if (!dataContext.ChampionPlayers.Any())
            //{
            //    var championPlayers = new List<ChampionPlayer>()
            //    {
            //        new ChampionPlayer
            //        {
            //            Player = new Player
            //            {
            //                Name = "Moist potato",
            //                Level = 420           
            //            },
            //            Champion = new Champion
            //            {
            //                Name = "Aurelion Sol",
            //            }
                        
            //        },
            //        new ChampionPlayer
            //        {
            //            Player = new Player
            //            {
            //                Name = "Moist toucan",
            //                Level = 42
            //            },
            //            Champion = new Champion
            //            {
            //                Name = "Olaf",
            //            }
                        
            //        }
            //    };

            //    dataContext.ChampionPlayers.AddRange(championPlayers);
            //    dataContext.SaveChanges();
            //}
        }
    }
}
