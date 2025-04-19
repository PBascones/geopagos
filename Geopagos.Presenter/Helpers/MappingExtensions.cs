using Geopagos.Entities.Business;
using Geopagos.Entities.Enums;
using Geopagos.Presenter.Models;

namespace Geopagos.Presenter.Helpers
{
    public static class MappingExtensions
    {
        public static PlayerModel ToModel(this PlayerSnapshot snapshot, int? winnerId)
        {
            return new PlayerModel
            {
                Name = snapshot.Name,
                Gender = Enum.TryParse<Gender>(snapshot.Gender, true, out var gender) ? gender : default,
                SkillLevel = snapshot.SkillLevel,
                Strength = snapshot.Strength,
                Speed = snapshot.Speed,
                ReactionTime = snapshot.ReactionTime,
                IsWinner = snapshot.Id == winnerId
            };
        }

        public static TournamentResultModel ToModel(this TournamentResult result)
        {
            return new TournamentResultModel
            {
                Id = result.Id,
                PlayedDate = result.PlayedDate,
                Gender = result.Gender,
                Players = result.Players.Select(p => p.ToModel(result.WinnerSnapshotId)).ToList()
            };
        }
    }
}
