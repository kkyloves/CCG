using System;
using Scriptable;
using TMPro;
using UnityEngine;

namespace UI
{
    public class EnergyCountUI : MonoBehaviour
    {
        public static EnergyCountUI Instance;
        [SerializeField] private TextMeshProUGUI blueEnergyCount;
        [SerializeField] private TextMeshProUGUI blackEnergyCount;
        [SerializeField] private TextMeshProUGUI redEnergyCount;
        [SerializeField] private TextMeshProUGUI whiteEnergyCount;
        [SerializeField] private TextMeshProUGUI greenEnergyCount;
        [SerializeField] private TextMeshProUGUI voidEnergyCount;

        private void Awake()
        {
            Instance = this;
        }

        public void UpdateEnergyCount(EnergyType energyType, int value)
        {
            switch (energyType)
            {
                case EnergyType.Blue:
                    blueEnergyCount.text = value.ToString();
                    break;
                case EnergyType.Black:
                    blackEnergyCount.text = value.ToString();
                    break;
                case EnergyType.Red:
                    redEnergyCount.text = value.ToString();
                    break;
                case EnergyType.White:
                    whiteEnergyCount.text = value.ToString();
                    break;
                case EnergyType.Green:
                    greenEnergyCount.text = value.ToString();
                    break;
                case EnergyType.Void:
                    voidEnergyCount.text = value.ToString();
                    break;
                case EnergyType.Length:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(energyType), energyType, null);
            }
        }


    }
}
