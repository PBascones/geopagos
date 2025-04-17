using Geopagos.Entities.Enums;

namespace Geopagos.Entities.Business
{
    public abstract class Player
    {
        public string Name { get; set; }
        public int SkillLevel { get; set; } // 0 - 100
        public Gender Gender { get; set; }

        protected Player(string name, int skillLevel, Gender gender)
        {
            Name = name;
            SkillLevel = skillLevel;
            Gender = gender;
        }

        public abstract double GetEffectiveScore();
    }
}
