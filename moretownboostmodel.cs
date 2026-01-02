using System;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Settlements;

namespace moretownboost
{
    public class moretownboostmodel : DefaultBuildingConstructionModel
    {
        //controls how fast your city consumes gold reserve (in vanilla it is capped at 500 as you know)
        //keep in mind you're still limited by one construction a day (you would waste gold, construction doesnt overflow into the next project)
        //higher number is slower, lower number is faster.
        int constructionrate = 8; 
        public override int GetBoostCost(Town town)
        {
        int gold = town.BoostBuildingProcess;
            if (gold <= 0) return 0;
            return Math.Max(500,gold/constructionrate);
        }
        public override int GetBoostAmount(Town town)
        {
            // 0.1f is the vanilla value for how much construction you're getting per gold
            // increasing above 0.1 would give you more construction per gold than vanilla.
            // 0.13 gives an increase of around 30% compared to the vanilla 0.1
            float productionvalue = 0.13f; 
            float gold = town.BoostBuildingProcess;
            return (int)Math.Floor(Math.Max(50f,((gold/constructionrate)*productionvalue)));
        }
    }
}