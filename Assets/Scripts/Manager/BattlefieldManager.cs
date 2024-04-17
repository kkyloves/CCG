using Battle_Cards;
using DG.Tweening;
using Script.Misc;
using Scriptable;
using UnityEngine;

public class BattlefieldManager : Singleton<BattlefieldManager>
{
    [SerializeField] private CardDetails[] tempEnemyCardDetails;
    
    [SerializeField] private BattleCardManager[] enemyBattleCards;
    [SerializeField] private BattleCardManager[] enemyEnergyCards;

    [SerializeField] private BattleCardManager[] playerBattleCards;
    [SerializeField] private BattleCardManager[] playerEnergyCards;

    private BattleCardManager activeBattleCardManager;
    public BattleCardManager ActiveBattleCardManager => activeBattleCardManager;

    public BattleCardManager[] EnemyBattleCards => enemyBattleCards;
    
    private void Start()
    {
        for (var i = 0; i < tempEnemyCardDetails.Length; i++)
        {
            enemyBattleCards[i].SetIndex(i);
            if (tempEnemyCardDetails[i] != null)
            {
                enemyBattleCards[i].UpdateBattleCard(tempEnemyCardDetails[i]);
            }
        }
        
        for (var i = 0; i < playerEnergyCards.Length; i++)
        {
            playerEnergyCards[i].SetIndex(i);
        }
        
        for (var i = 0; i < enemyEnergyCards.Length; i++)
        {
            enemyEnergyCards[i].SetIndex(i);
        }
        
        for (var i = 0; i < playerBattleCards.Length; i++)
        {
            playerBattleCards[i].SetIndex(i);
        }
        
        for (var i = 0; i < enemyBattleCards.Length; i++)
        {
            enemyBattleCards[i].SetIndex(i);
        }
    }

    public void ShowPlayerBattleField()
    {
        foreach (var card in playerBattleCards)
        {
            card.BattleCardDisplayController.ShowBattleCardSlot();
        }

        foreach (var card in playerEnergyCards)
        {
            card.BattleCardDisplayController.ShowBattleCardSlot();
        }
    }

    public void HidePlayerBattleField()
    {
        foreach (var card in playerBattleCards)
        {
            card.BattleCardDisplayController.HideBattleCardSlot();
        }

        foreach (var card in playerEnergyCards)
        {
            card.BattleCardDisplayController.HideBattleCardSlot();
        }
    }

    public BattleCardManager GetNearestOpenEnergySpotCard(Vector3 position)
    {
        var nearestCard = GetOpenSpotEnergyCard();
        position.z = nearestCard.transform.position.z;

        var nearestDistance = Vector2.Distance(nearestCard.transform.position, position);
        
        if (position.y < 700f)
        {
            //foreach (var card in playerEnergyCards)
            foreach (var card in playerEnergyCards)
            {
                if (!card.IsSlotted)
                {
                    var distance = Vector3.Distance(card.transform.position, position);

                    if (distance < nearestDistance)
                    {
                        nearestCard = card;
                        nearestDistance = distance;
                    }
                }
            }
        }

        return nearestCard;
    }
    
    public BattleCardManager GetNearestOpenCreatureSpotCard(Vector2 position)
    {
        var nearestCard = GetOpenSpotCreatureCard();
        var nearestDistance = Vector2.Distance(nearestCard.transform.position, position);

        if (position.y < 700f)
        {
            foreach (var card in playerBattleCards)
            {
                if (!card.IsSlotted)
                {
                    var distance = Vector2.Distance(card.transform.position, position);
                    if (distance < nearestDistance)
                    {
                        nearestCard = card;
                        nearestDistance = distance;
                    }
                }
            }
        }

        return nearestCard;
    }

    private BattleCardManager GetOpenSpotEnergyCard()
    {
        foreach (var card in playerEnergyCards)
        {
            if (!card.IsSlotted)
            {
                return card;
            }
        }

        Debug.Log("No Extra Slot Available");
        return null;
    }
    
    private BattleCardManager GetOpenSpotCreatureCard()
    {
        foreach (var card in playerBattleCards)
        {
            if (!card.IsSlotted)
            {
                return card;
            }
        }

        Debug.Log("No Extra Slot Available");
        return null;
    }

    public BattleCardManager GetPlayerFreeBattleCard()
    {
        foreach (var card in playerBattleCards)
        {
            if (!card.IsSlotted)
            {
                return card;
            }
        }

        Debug.Log("No Extra Slot Available");
        return null;
    }

    public BattleCardManager GetEnemyCounterpart(int index)
    {
        return enemyBattleCards[index].IsSlotted ? enemyBattleCards[index] : null;
    }

    public void SetTargetButtons(BattleCardManager battleCardManager, bool enabled)
    {
        if (enabled)
        {
            if (activeBattleCardManager != null)
            {
                activeBattleCardManager.CardItem.CardGlow.DOFade(0f, 0.5f);
            }
            
            activeBattleCardManager = battleCardManager;
            activeBattleCardManager.CardItem.CardGlow.DOFade(1f, 0.5f);
        }

        foreach (var card in enemyBattleCards)
        {
            if (card.IsSlotted)
            {
                card.TargetButton.gameObject.SetActive(enabled);
            }
        }
        
    }
    
    public void TurnOnAttackButtons()
    {
        foreach (var card in playerBattleCards)
        {
            if (card.IsSlotted)
            {
                card.AttackButton.gameObject.SetActive(true);
            }
        }
    }
    
    public void ResetBattleCards()
    {
        foreach (var card in playerBattleCards)
        {
            if (card.IsSlotted)
            {
                card.CardItem.DisableEffect.DOFade(0f, 0.5f);
            }
        }
    }

    public void RemoveActiveAttackBattleCard()
    {
        activeBattleCardManager.CardItem.DisableEffect.DOFade(0.7f, 0.5f);
        activeBattleCardManager.CardItem.CardGlow.DOFade(0f, 0.5f);

        activeBattleCardManager.AttackButton.gameObject.SetActive(false);
        activeBattleCardManager = null;
    }

    public bool IsEnemyBattleFieldOpen()
    {
        foreach (var battleCard in enemyBattleCards)
        {
            if (battleCard.IsSlotted)
            {
                return false;
            }
        }

        return true;
    }
}