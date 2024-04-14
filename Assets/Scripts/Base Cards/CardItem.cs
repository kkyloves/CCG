using System;
using DG.Tweening;
using HandCard_Item;
using Scriptable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum CardItemType
{
    HandCard,
    DiscardCard,
    InitialDrawCard,
    HandCardDragTool,
    HandCardDrawTool,
    CardChosenDisplay,
    BattleCard,
    DrawCard,
    JustCardItem,
}

public class CardItem : MonoBehaviour
{    
    [SerializeField] private CardItemType cardItemType;

    [SerializeField] private Image cardCreatureSprite;
    [SerializeField] private Image cardEnergySprite;
    [SerializeField] private Image borderBanner;
    [SerializeField] private Image glowSprite;
    [SerializeField] private Image nameBannerSprite;
    [SerializeField] private Image attackDefFrame;
    [SerializeField] private Image cardEnergyLogo;
    [SerializeField] private Image cardEnergyContainer;
    [SerializeField] private Image[] energyCosts;
    [SerializeField] private Image hurtEffect;
    [SerializeField] private Image disableEffect;

    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private TextMeshProUGUI cardNickname;
    [SerializeField] private TextMeshProUGUI cardDescription;
    [SerializeField] private TextMeshProUGUI cardAttack;
    [SerializeField] private TextMeshProUGUI cardDefense;

    [SerializeField] private Transform backgroundTransform;
    [SerializeField] private CanvasGroup canvasGroup;

    public Image RedHurtEffect => hurtEffect;
    public Image DisableEffect => disableEffect;
    public Image CardGlow => glowSprite;

    public Transform BackgroundTransform => backgroundTransform;
    public CanvasGroup CanvasGroup => canvasGroup;

    private HandCardManager handCardManager;
    public HandCardManager HandCardManager => handCardManager;
    
    private DiscardCardItem discardCardItem;
    public DiscardCardItem DiscardCardItem => discardCardItem;
    
    private InitialDrawManager initialDrawManager;
    public InitialDrawManager InitialDrawManager => initialDrawManager;
    
    private HandCardDragTool handCardDragTool;
    public HandCardDragTool HandCardDragTool => handCardDragTool;
    
    private CardDrawAnimationTool cardDrawAnimationTool;
    public CardDrawAnimationTool CardDrawAnimationTool => cardDrawAnimationTool;
    
    private CardChosenDisplayController cardChosenDisplayController;
    public CardChosenDisplayController CardChosenDisplayController => cardChosenDisplayController;
    
    private CardDetails cardDetails;
    public CardDetails CardDetails => cardDetails;

    private StatsCounter statsCounter;
    public StatsCounter StatsCounter => statsCounter;
    
    private void Awake()
    {
        hurtEffect.DOFade(0f, 0f);
        statsCounter = new StatsCounter(cardAttack, cardDefense);
        
        switch (cardItemType)
        {
            case CardItemType.HandCard:
                handCardManager = gameObject.AddComponent<HandCardManager>();
                break;
            
            case CardItemType.DiscardCard:
                discardCardItem = gameObject.AddComponent<DiscardCardItem>();
                break;
            
            case CardItemType.InitialDrawCard:                
                initialDrawManager = gameObject.AddComponent<InitialDrawManager>();
                break;
            
            case CardItemType.DrawCard:
                break;
            
            case CardItemType.HandCardDragTool:
                handCardDragTool = gameObject.AddComponent<HandCardDragTool>();
                break;

            case CardItemType.CardChosenDisplay:
                cardChosenDisplayController = gameObject.AddComponent<CardChosenDisplayController>();

                break;

            case CardItemType.BattleCard:
                break;

            case CardItemType.JustCardItem:
                break;

            case CardItemType.HandCardDrawTool:
                cardDrawAnimationTool = gameObject.AddComponent<CardDrawAnimationTool>();

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Init(CardDetails cardDetails)
    {
        this.cardDetails = cardDetails;

        glowSprite.DOFade(0f, 0f);
        cardName.text = cardDetails.CardTextDetails.CardName;
        cardNickname.text = cardDetails.CardTextDetails.CardNickname;
        cardDescription.text = cardDetails.CardTextDetails.CardDescription;
        
        cardCreatureSprite.sprite = cardDetails.CardSprite;
        cardEnergySprite.sprite = cardDetails.CardSprite;

        cardAttack.text = cardDetails.AttackAmount.ToString();
        cardDefense.text = cardDetails.DefAmount.ToString();
        
        if (cardDetails.CardType.Equals(CardType.Energy))
        {
            var energyLogo = CardSpriteStorage.Instance.GetEnergyLogoSprite(cardDetails.EnergyType);
            cardEnergyLogo.sprite = energyLogo;
            
            foreach (var energyCost in energyCosts)
            {
                energyCost.DOFade(0f, 0f);
            }
        }
        else
        {
            statsCounter.SetStats(cardDetails.AttackAmount, cardDetails.DefAmount);
            ProcessRelatedEnergy(cardDetails.EnergyNeeded, cardDetails.EnergyType);
        }
        
        //ProcessRelatedEnergy(cardDetails.EnergyNeeded, cardDetails.EnergyType);
        ProcessCardTypeDiff(cardDetails);
    }

    private void ProcessCardTypeDiff(CardDetails cardDetails)
    {
        switch (cardDetails.CardType)
        {
            case CardType.Creature:
                //turn off energy stuff
                cardEnergySprite.DOFade(0f, 0f);
                cardEnergyLogo.DOFade(0f, 0f);
                
                //turn on creature stuff
                cardCreatureSprite.DOFade(1f, 0f);
                nameBannerSprite.DOFade(1f, 0f);
                cardEnergyContainer.DOFade(1f, 0f);
                attackDefFrame.DOFade(1f, 0f);

                // foreach (var energyCost in energyCosts)
                // {
                //     energyCost.DOFade(1f, 0f);
                // }
                
                break;
            case CardType.Energy:
                //turn off creature stuff
                cardCreatureSprite.DOFade(0f, 0f);
                nameBannerSprite.DOFade(0f, 0f);
                cardEnergyContainer.DOFade(0f, 0f);
                attackDefFrame.DOFade(0f, 0f);

                cardDescription.text = string.Empty;
                cardAttack.text = string.Empty;
                cardDefense.text = string.Empty;
                
                // foreach (var energyCost in energyCosts)
                // {
                //     energyCost.DOFade(0f, 0f);
                // }
                
                //turn on energy stuff
                cardEnergySprite.DOFade(1f, 0f);
                cardEnergyLogo.DOFade(1f, 0f);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ProcessRelatedEnergy(EnergyNeed[] energyNeeds, EnergyType energyType)
    {
        var energyCostInitCounter = 0;
        foreach (var energy in energyCosts)
        {
            energy.DOFade(0f, 0f);
        }
        
        foreach (var energy in energyNeeds)
        {
            for(var i=0; i<energy.EnergyNeededAmount; i++)
            {
                var energySprite = CardSpriteStorage.Instance.GetEnergyIconSprite(energy.EnergyNeededType);
                energyCosts[energyCostInitCounter].sprite = energySprite;
                energyCosts[energyCostInitCounter].DOFade(1f, 0.1f);
                energyCostInitCounter++;
            }
        }

        var energyContainerSprite = CardSpriteStorage.Instance.GetEnergyBanner(energyCostInitCounter);
        cardEnergyContainer.sprite = energyContainerSprite;
        
        var nameBanner = CardSpriteStorage.Instance.GetNameBannerSprite(energyType);
        nameBannerSprite.sprite = nameBanner;
        
        var borderBanner = CardSpriteStorage.Instance.GetBorderBannerSprite(energyType);
        this.borderBanner.sprite = borderBanner;
    }

    public bool UpdateAttackStats(int deduction)
    {
        return statsCounter.UpdateAttack(deduction);
    }

    public bool UpdateDefStats(int deduction)
    {
        return statsCounter.UpdateDef(deduction);
    }
}
