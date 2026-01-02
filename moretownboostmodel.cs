using System;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Settlements;

namespace moretownboost
{
    public static class BoostConfig
    {
        public const float ConstructionRate = 10f; // Changed to float for precision
        public const float ProductionMultiplier = 0.13f;
        public const int TownBaseDrain = 500;
        public const int CastleBaseDrain = 250;
    }

    public class moretownboostmodel : DefaultBuildingConstructionModel
    {
        public override int TownBoostCost => BoostConfig.TownBaseDrain;
        public override int CastleBoostCost => BoostConfig.CastleBaseDrain;

        public override int GetBoostCost(Town town)
        {
            if (town.BoostBuildingProcess <= 0) return 0;
            int baseDrain = town.IsCastle ? BoostConfig.CastleBaseDrain : BoostConfig.TownBaseDrain;

            // Using (int)Math.Floor ensures the drain matches the UI precisely
            return Math.Max(baseDrain, (int)Math.Floor(town.BoostBuildingProcess / BoostConfig.ConstructionRate));
        }

        public override int GetBoostAmount(Town town)
        {
            if (town.BoostBuildingProcess <= 0) return 0;

            int currentCost = GetBoostCost(town);
            float boost = currentCost * BoostConfig.ProductionMultiplier;

            float minBoost = town.IsCastle ? 20f : 50f;
            return (int)Math.Max(minBoost, boost);
        }
    }
}