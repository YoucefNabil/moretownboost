using System;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Settlements;

namespace moretownboost
{
    // CHANGE YOUR SETTINGS HERE
    public static class BoostConfig
    {
        public const int ConstructionRate = 10; // Lower = Faster drain, faster construction. Higher = opposite (Gold / 10)
        public const float ProductionMultiplier = 0.13f; // Higher = More value out of gold, vanilla is 0.1, 0.13 is a 30% increase from vanilla
        public const int BaseGameDrain = 500; // The vanilla hardcoded drain amount for towns.
        public const int CastleBaseDrain = 275; // I really don't know what this is.
    }
    public class moretownboostmodel : DefaultBuildingConstructionModel
    {
        public override int GetBoostCost(Town town)
        {
            int gold = town.BoostBuildingProcess;
            if (gold <= 0) return 0;
            // Uses the shared config so it matches the behavior perfectly
            return Math.Max(BoostConfig.BaseGameDrain, gold / BoostConfig.ConstructionRate);
        }
        public override int GetBoostAmount(Town town)
        {
            float gold = town.BoostBuildingProcess;
            // Your logic: ((Gold / Rate) * Multiplier)
            // Example: (20,000 / 20) * 0.1 = 100 Bonus Construction
            float boost = (gold / BoostConfig.ConstructionRate) * BoostConfig.ProductionMultiplier;
            // Ensure we return at least the base 50 if gold is sufficient
            return (int)Math.Max(50f, boost);
        }
    }
}