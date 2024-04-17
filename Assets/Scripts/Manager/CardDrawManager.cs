using System.Collections;
using System.Collections.Generic;
using HandCard_Item;
using Manager;
using Scriptable;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardDrawManager : Script.Misc.Singleton<CardDrawManager>
{
    private const int HAND_CARD_COUNT = 7;
    
    [SerializeField] private List<CardDetails> cardDeck;
    [SerializeField] private Transform initialPosition;
    [SerializeField] private Transform cardDrawAnimationLandPosition;
    [SerializeField] private Transform cardDisplayPosition;
    [SerializeField] private Transform cardDisplayIdlePosition;

    public Transform InitialPosition => initialPosition;
    public Transform CardDrawAnimationLandPosition => cardDrawAnimationLandPosition;
    public Transform CardDisplayPosition => cardDisplayPosition;
    public Transform CardDisplayIdlePosition => cardDisplayIdlePosition;

    public bool isDrawingCards;
    public bool IsDrawingCards => isDrawingCards;
    
    private CardDetails GetRandomCardInDeck()
    {
        if (cardDeck.Count > 0)
        {
            var randomCardIndex = Random.Range(0, cardDeck.Count);
            var randomCard = cardDeck[randomCardIndex];
            cardDeck.RemoveAt(randomCardIndex);
            
            return randomCard;
        }

        return null;
    }
    
    public void FirstDrawCard()
    {
        StartCoroutine(DrawCardDelay(1f));
    }
    
    private IEnumerator DrawCardDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DrawCard();
    }

    public void DrawCard()
    {
        CardHandDragController.Instance.MoveHandCardListDown();
        CardHandManager.Instance.ResetHandCardsPosition();

        var card = GetRandomCardInDeck();

        if (card != null)
        {
            var cardHandSlot = CardHandManager.Instance.GetOpenSlotHandCard();
            if (cardHandSlot != null)
            {
                var index = cardHandSlot.GetIndex();
                cardHandSlot.InitHandCard(card, index, false);
                CardDrawAnimationTool.Instance.InitDrawCard(card, cardHandSlot);
            }
        }
    }
    
    public void SetDrawingCard(bool isDrawingCards)
    {
        this.isDrawingCards = isDrawingCards;
    }

    public List<CardDetails> DrawInitHandCards()
    {
        List<CardDetails> drawHandCards = new();

        for (var i = 0; i < HAND_CARD_COUNT; i++)
        {
            var card = GetRandomCardInDeck();
            drawHandCards.Add(card);
        }

        return drawHandCards;
    }

    public void ReDrawInitialCards(List<CardDetails> cardDetailsList)
    {
        foreach (var card in cardDetailsList)
        {
            cardDeck.Add(card);
        }
    }
}
