namespace Geopagos.Entities.Business
{
    public class MalePlayer : Player
    {
        public int Strength { get; set; }
        public int Speed { get; set; }

        public MalePlayer(string name, int skillLevel, int strength, int speed)
            : base(name, skillLevel, Enums.Gender.Male)
        {
            Strength = strength;
            Speed = speed;
        }

        public override double GetEffectiveScore()
        {
            return SkillLevel * 0.6 + Strength * 0.3 + Speed * 0.1;
        }
    }
}
