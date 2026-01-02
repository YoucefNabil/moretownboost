using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;

// this just works, do not touch any of this
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

        // class below only works for town for some reason, it also adds or removes 500 gold or something, thus the -500 adjustment
        private void OnDailyTickTown(Town town)
        {
            if (town.BoostBuildingProcess > 0)
            {
                // 1. We need to know what the gold was BEFORE the engine took 500.
                int currentGold = town.BoostBuildingProcess;
                int goldBeforeEngineTick = currentGold + 500;
                // 2. Calculate what the TOTAL drain should have been (Gold / 20)
                int totalTargetDrain = Math.Max(500, goldBeforeEngineTick / 20);
                // 3. Since the engine ALREADY took 500, we only take the extra.
                if (totalTargetDrain > 500)
                {
                    int extraToTake = totalTargetDrain - 500;
                    town.BoostBuildingProcess = Math.Max(0, currentGold - extraToTake);
                }
                // If the totalTargetDrain was 500, we do nothing (the engine already did it).
            }
        }
        // class below works for castle and may work for town, doesnt have the issue of adding or removing 500 randomly
        private void OnDailyTickSettlement(Settlement settlement)
        {
            // Settlement.Town works for both Towns and Castles
            // We check if it exists and if the boost is actually active
            if ((settlement.IsCastle) && settlement.Town != null)
            {
                Town fort = settlement.Town;
                if (fort.BoostBuildingProcess > 0) { 
                    int currentGold = fort.BoostBuildingProcess;
                    int goldBeforeEngineTick = currentGold;
                    int totalTargetDrain = Math.Max(500, goldBeforeEngineTick / 20);
                    if (totalTargetDrain > 500)
                      {
                        int extraToTake = totalTargetDrain;
                        fort.BoostBuildingProcess = Math.Max(0, currentGold - extraToTake);
                       }
                }
            }
        }
    }
}