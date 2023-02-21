using LeagueProfiles.Dtos;
using LeagueProfiles.Models;

namespace LeagueProfiles.Interfaces
{
    public interface IPlayerRepository
    {
        public ICollection<Player> GetPlayers();

        public Task<Player> GetPlayerById(int id);
        public Task<ICollection<Player>> GetPlayerByName(string name);
        public Task<Player> GetPlayerByUserName(string username);

        public bool AddChampionToPlayer(int championId, int playerId);

        public bool PlayerExists(int id);
        public bool PlayerExists(string name);

        public bool CreatePlayer(Player player);

        public ICollection<Champion> GetPlayerChampions(int id);
        public bool UpdatePlayer(Player player);

        public string GetPlayerNameTokenBased();
    }
}
