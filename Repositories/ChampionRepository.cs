using LeagueProfiles.Data;
using LeagueProfiles.Interfaces;
using LeagueProfiles.Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace LeagueProfiles.Repositories
{
    public class ChampionRepository : IChampionRepository
    {
        private readonly DataContext _dataContext;

        public ChampionRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public bool ChampionExists(int id)
        {
            return _dataContext.Champions.Any(c => c.Id == id);
        }

        public bool ChampionExists(string name)
        {
            return _dataContext.Champions.Any(c => c.Name == name);
        }

        public bool CreateChampion(Champion Champion)
        {
            if (_dataContext.Champions.Any(c => c.Name == Champion.Name))
            {
                return false;
            }

            _dataContext.Add(Champion);

            return Save();
        }

        public Task<Champion> GetChampionById(int id)
        {
            return _dataContext.Champions.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public Task<Champion> GetChampionByName(string name)
        {
            return _dataContext.Champions.Where(c => c.Name == name).FirstOrDefaultAsync();
        }

        public ICollection<Player> GetChampionPlayers(int id)
        {
            return _dataContext.Champions.Where(c => c.Id == id).Select(c => c.Players).FirstOrDefault();
        }

        public ICollection<Champion> GetChampions()
        {
            return _dataContext.Champions.ToList();
        }

        public bool UpdateChampion(Champion Champion)
        {
            if(!ChampionExists(Champion.Id))
                return false;

            _dataContext.Update(Champion);

            return Save();
        }

        public bool Save()
        {
            return _dataContext.SaveChanges() > 0 ? true : false;
        }

        public bool DeleteChampion(Champion Champion)
        {
            if (!ChampionExists(Champion.Id))
                return false;

            _dataContext.Remove(Champion);

            return Save();
        }
    }
}
