using AutoMapper;
using LeagueProfiles.Dtos;
using LeagueProfiles.Models;

namespace LeagueProfiles.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<PlayerDto, Player>();
            CreateMap<ChampionDto, Champion>();
        }
    }
}
