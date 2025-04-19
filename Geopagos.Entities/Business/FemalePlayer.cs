namespace Geopagos.Entities.Business
{
    public class FemalePlayer : Player
    {
        public int ReactionTime { get; set; }

        public FemalePlayer(string name, int skillLevel, int reactionTime)
            : base(name, skillLevel, Enums.Gender.Female)
        {
            ReactionTime = reactionTime;
        }

        public override double GetEffectiveScore()
        {
            return SkillLevel * 0.7 + ReactionTime * 0.3;
        }

        public override PlayerSnapshot ToSnapshot()
        {
            return new PlayerSnapshot
            {
                Name = Name,
                Gender = Gender.ToString(),
                SkillLevel = SkillLevel,
                ReactionTime = ReactionTime
            };
        }
    }
}
