using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;

namespace moretownboost
{
    public class SubModule : MBSubModuleBase
    {
        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            if (game.GameType is Campaign)
            {
                CampaignGameStarter starter = (CampaignGameStarter)gameStarterObject;
                // We add our classes here
                starter.AddModel(new moretownboostmodel());
                starter.AddBehavior(new moretownboostbehavior());
            }
        }
    }
}