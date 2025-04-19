using Geopagos.Entities.Enums;

namespace Geopagos.Presenter.Models
{
    public class PlayerModel
    {
        public string Name { get; set; }
        public int SkillLevel { get; set; }
        public Gender Gender { get; set; }

        // Male fields
        public int? Strength { get; set; }
        public int? Speed { get; set; }

        // Female fields
        public int? ReactionTime { get; set; }

        public bool IsWinner { get; set; }
    }
}
