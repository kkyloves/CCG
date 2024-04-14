using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(menuName = "Create Card Details/Card Detail")]
    public class CardDetails : ScriptableObject
    {
        [SerializeField] private string cardId;
        [SerializeField] private Sprite cardSprite;
        [SerializeField] private CardType cardType;
        [SerializeField] private EnergyType energyType;
        [SerializeField] private EnergyNeed[] energyNeeded;
        [SerializeField] private CardTextDetails cardTextDetails;
        [SerializeField] private int attackAmount;
        [SerializeField] private int defAmount;
        
        public string CardId => cardId;
        public Sprite CardSprite => cardSprite;
        public CardType CardType => cardType;
        public EnergyType EnergyType => energyType;
        public EnergyNeed[] EnergyNeeded => energyNeeded;
        public CardTextDetails CardTextDetails => cardTextDetails; 
        
        public int AttackAmount => attackAmount;
        public int DefAmount => defAmount;
    }

    [System.Serializable]
    public enum CardType
    {
        Energy, Creature
    }

    [System.Serializable]
    public enum EnergyType
    {
        Blue, Black, Red, White, Green, Void, Length
    }

    [System.Serializable]
    public class CardTextDetails
    {
        [SerializeField] private string cardName;
        [SerializeField] private string cardNickname;
        [SerializeField] private string cardDescription;
        
        public string CardName => cardName;
        public string CardNickname => cardNickname;
        public string CardDescription => cardDescription;
    }
    
    [System.Serializable]
    public class EnergyNeed
    {
        [SerializeField] private EnergyType energyNeededType;
        [SerializeField] private int energyNeededAmount;
        
        public EnergyType EnergyNeededType => energyNeededType;
        public int EnergyNeededAmount => energyNeededAmount;
    }
    
}
