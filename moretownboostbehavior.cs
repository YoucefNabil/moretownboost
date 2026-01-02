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
            // Towns only trigger here
            ProcessDrain(town, BoostConfig.TownBaseDrain);
        }

        private void OnDailyTickSettlement(Settlement settlement)
        {
            // Castles only trigger here to prevent double-dipping in Towns
            if (settlement.IsCastle && settlement.Town != null)
            {
                ProcessDrain(settlement.Town, BoostConfig.CastleBaseDrain+25);
            }
        }

        private void ProcessDrain(Town town, int engineAlreadyTook)
        {
            if (town.BoostBuildingProcess > 0)
            {
                // We calculate using floats to match the engine's internal precision
                float goldBeforeTick = (float)town.BoostBuildingProcess + engineAlreadyTook;

                int targetTotalCost = Math.Max(engineAlreadyTook, (int)Math.Floor(goldBeforeTick / BoostConfig.ConstructionRate));

                int extraToTake = targetTotalCost - engineAlreadyTook;
                if (extraToTake > 0)
                {
                    // Update the reserve. 
                    // This happens immediately after the engine takes the base amount.
                    town.BoostBuildingProcess = Math.Max(0, town.BoostBuildingProcess - extraToTake);
                }
            }
        }
    }
}