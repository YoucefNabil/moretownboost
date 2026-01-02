using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;

namespace moretownboost
{
    public class moretownboostbehavior : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            CampaignEvents.DailyTickTownEvent.AddNonSerializedListener(this, OnDailyTickTown);
            CampaignEvents.DailyTickSettlementEvent.AddNonSerializedListener(this, OnDailyTickSettlement);
        }

        public override void SyncData(IDataStore dataStore) { }

        private void OnDailyTickTown(Town town)
        {
            // Engine already took 500. We take the rest.
            ProcessDrain(town, BoostConfig.TownBaseDrain);
        }

        private void OnDailyTickSettlement(Settlement settlement)
        {
            // Engine already took 250 for castles. We take the rest.
            if (settlement.IsCastle && settlement.Town != null)
            {
                ProcessDrain(settlement.Town, BoostConfig.CastleBaseDrain);
            }
        }

        private void ProcessDrain(Town town, int engineAlreadyTook)
        {
            if (town.BoostBuildingProcess > 0)
            {
                // 1. What was the gold before the engine touched it?
                int goldBeforeTick = town.BoostBuildingProcess + engineAlreadyTook;

                // 2. What does our Model say the total cost should be?
                // We calculate it manually here to ensure we use the "BeforeTick" gold
                int targetTotalCost = Math.Max(engineAlreadyTook, goldBeforeTick / BoostConfig.ConstructionRate);

                // 3. Subtract the difference
                int extraToTake = targetTotalCost - engineAlreadyTook;
                if (extraToTake > 0)
                {
                    town.BoostBuildingProcess = Math.Max(0, town.BoostBuildingProcess - extraToTake);
                }
            }
        }
    }
}