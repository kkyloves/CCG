using System;
using System.Collections.Generic;
using Script.Misc;
using Scriptable;
using UI;

namespace Manager
{
    [Serializable]
    public class EnergyData
    {
        public int OriginalValue;
        public int CurrentValue;
    }
    
    public class PlayerDataManager : Singleton<PlayerDataManager>
    {
        private readonly Dictionary<EnergyType, EnergyData> energyData = new();
        
        private void Awake()
        {
            //set energy data
            for (var i = 0; i < (int)EnergyType.Length; i++)
            {
                var data = new EnergyData
                {
                    OriginalValue =  0,
                    CurrentValue = 0
                };
                
                energyData.Add((EnergyType)i, data);
            }
        }

        public void AddEnergyValue(EnergyType type, int valueToAdd)
        {
            if (energyData.ContainsKey(type))
            {
                energyData[type].OriginalValue += valueToAdd;
                energyData[type].CurrentValue += valueToAdd;
            }
            
            EnergyCountUI.Instance.UpdateEnergyCount(type, energyData[type].CurrentValue);
        }
        
        public bool CanPlaceMinionCard(EnergyNeed[] energyNeeds)
        {
            var canPlace = false;
            foreach (var energy in energyNeeds)
            {
                var type = energy.EnergyNeededType;
                if (energyData.ContainsKey(type))
                {
                    if (energyData[type].CurrentValue < energy.EnergyNeededAmount)
                    {
                        return false;
                    }

                }
                else
                {
                    return false;
                }
            }

            return true;
        }     
        
        public void UseEnergy(EnergyNeed[] energyNeeds)
        {
            foreach (var energy in energyNeeds)
            {
                var type = energy.EnergyNeededType;
                var amount = energy.EnergyNeededAmount;
                
                if (energyData[type].CurrentValue >= amount)
                {
                    energyData[type].CurrentValue -= amount;
                    EnergyCountUI.Instance.UpdateEnergyCount(type, energyData[type].CurrentValue);
                }
            }
        }

        public void ResetEnergy()
        {
            foreach (var energy in energyData)
            {
                energy.Value.CurrentValue = energy.Value.OriginalValue;
                EnergyCountUI.Instance.UpdateEnergyCount(energy.Key, energyData[energy.Key].CurrentValue);
            }
        }
    }
}
