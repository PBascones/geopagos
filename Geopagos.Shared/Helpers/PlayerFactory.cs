using Geopagos.Entities.Business;
using Geopagos.Entities.Enums;
using Geopagos.Presenter.Models;

namespace Geopagos.Shared.Helpers
{
    public static class PlayerFactory
    {
        public static Player Create(PlayerModel model)
        {
            return model.Gender switch
            {
                Gender.Male => new MalePlayer(
                    model.Name,
                    model.SkillLevel,
                    model.Strength ?? 0,
                    model.Speed ?? 0
                ),

                Gender.Female => new FemalePlayer(
                    model.Name,
                    model.SkillLevel,
                    model.ReactionTime ?? 0
                ),

                _ => throw new ArgumentException("Unsupported gender type")
            };
        }
    }
}
