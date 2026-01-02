using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Settlements;
// do not touch anything anymore in here
namespace moretownboost
{
    public class moretownboostbehavior : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            // We register both events to handle Towns and Castles separately
            CampaignEvents.DailyTickTownEvent.AddNonSerializedListener(this, OnDailyTickTown);
            CampaignEvents.DailyTickSettlementEvent.AddNonSerializedListener(this, OnDailyTickSettlement);
        }

        public override void SyncData(IDataStore dataStore) { }

        // LOGIC FOR TOWNS (Engine takes 500 automatically)
        private void OnDailyTickTown(Town town)
        {
            if (town.BoostBuildingProcess > 0)
            {
                int currentGold = town.BoostBuildingProcess;

                // Reconstruct what the gold was BEFORE the engine tick
                int goldBeforeEngineTick = currentGold + BoostConfig.BaseGameDrain;

                // Calculate the Total Target Cost using your Shared Config
                int totalTargetCost = Math.Max(BoostConfig.BaseGameDrain, goldBeforeEngineTick / BoostConfig.ConstructionRate);

                // If the target cost is higher than 500, we subtract the difference
                if (totalTargetCost > BoostConfig.BaseGameDrain)
                {
                    int extraToTake = totalTargetCost - BoostConfig.BaseGameDrain;
                    town.BoostBuildingProcess = Math.Max(0, currentGold - extraToTake);
                }
            }
        }

        // --- CASTLE LOGIC (Engine takes 275) ---
        private void OnDailyTickSettlement(Settlement settlement)
        {
            if (settlement.IsCastle && settlement.Town != null)
            {
                Town fort = settlement.Town;
                if (fort.BoostBuildingProcess > 0)
                {
                    int currentGold = fort.BoostBuildingProcess;

                    // 1. Restore gold to "Pre-Engine-Tick" state (Add 275)
                    // The engine has likely already ticked 275 by the time this fires
                    int goldBeforeEngineTick = currentGold + BoostConfig.CastleBaseDrain;

                    // 2. Calculate the total we WANT to drain
                    int totalTargetCost = Math.Max(BoostConfig.CastleBaseDrain, goldBeforeEngineTick / BoostConfig.ConstructionRate);

                    // 3. Subtract what the engine didn't take
                    // If target is 1397, Engine took 275. We take 1147.
                    if (totalTargetCost > BoostConfig.CastleBaseDrain)
                    {
                        int extraToTake = totalTargetCost - BoostConfig.CastleBaseDrain;
                        fort.BoostBuildingProcess = Math.Max(0, currentGold - extraToTake);
                    }
                }
            }
        }
    }
}