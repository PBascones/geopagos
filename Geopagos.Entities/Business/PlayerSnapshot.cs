using System.ComponentModel.DataAnnotations.Schema;

namespace Geopagos.Entities.Business
{
    public class PlayerSnapshot
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SkillLevel { get; set; }
        public string Gender { get; set; }

        public int? Strength { get; set; }
        public int? Speed { get; set; }
        public int? ReactionTime { get; set; }

        public int TournamentResultId { get; set; }
        [ForeignKey(nameof(TournamentResultId))]
        public virtual TournamentResult TournamentResult { get; set; }
    }
}
