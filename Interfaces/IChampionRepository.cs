using LeagueProfiles.Models;

namespace LeagueProfiles.Interfaces
{
    public interface IChampionRepository
    {
        public ICollection<Champion> GetChampions();

        public Task<Champion> GetChampionById(int id);
        public Task<Champion> GetChampionByName(string name);

        public bool ChampionExists(int id);
        public bool ChampionExists(string name);

        public bool CreateChampion(Champion Champion);

        public ICollection<Player> GetChampionPlayers(int id);
        public bool UpdateChampion(Champion Champion);
        public bool DeleteChampion(Champion Champion);
    }
}
