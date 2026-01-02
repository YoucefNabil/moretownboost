using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Settlements;

namespace moretownboost
{
    public static class BoostConfig
    {
        public const int ConstructionRate = 10;
        public const float ProductionMultiplier = 0.13f;
        public const int TownBaseDrain = 500;   // Vanilla Town value
        public const int CastleBaseDrain = 275; // Vanilla Castle value (Confirmed via Source)
    }

    public class moretownboostmodel : DefaultBuildingConstructionModel
    {
        // We override these so other mods/UI see our "Minimum" costs
        public override int TownBoostCost => BoostConfig.TownBaseDrain;
        public override int CastleBoostCost => BoostConfig.CastleBaseDrain;

        public override int GetBoostCost(Town town)
        {
            if (town.BoostBuildingProcess <= 0) return 0;
            int baseDrain = town.IsCastle ? BoostConfig.CastleBaseDrain : BoostConfig.TownBaseDrain;

            // Formula: Gold / 10, but never less than the vanilla base
            return Math.Max(baseDrain, town.BoostBuildingProcess / BoostConfig.ConstructionRate);
        }

        public override int GetBoostAmount(Town town)
        {
            if (town.BoostBuildingProcess <= 0) return 0;

            // We calculate based on what the COST will be
            float currentCost = GetBoostCost(town);
            float boost = currentCost * BoostConfig.ProductionMultiplier;

            // Base game gives 50 for towns, 20 for castles. We ensure we don't go below that.
            float minBoost = town.IsCastle ? 20f : 50f;
            return (int)Math.Max(minBoost, boost);
        }
    }
}