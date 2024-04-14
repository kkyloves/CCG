using System.Collections.Generic;
using DG.Tweening;
using Manager;
using Script.Misc;
using Scriptable;
using UnityEngine;
using UnityEngine.UI;

public class InitialDrawCardsController : Singleton<InitialDrawCardsController>
{
    [SerializeField] private CanvasGroup[] uiToDisplay;
    [SerializeField] private CardItem[] initDrawCards;
    [SerializeField] private Button drawAgainButton;

    private List<CardDetails> drawCards = new();

    private void Awake()
    {
        drawAgainButton.onClick.AddListener(DrawAgain);

        foreach (var ui in uiToDisplay)
        {
            ui.DOFade(0f, 0f);
        }
    }

    private void Start()
    {
        InitDrawCards();
    }

    private void DrawAgain()
    {
        CardDrawManager.Instance.ReDrawInitialCards(drawCards);
        InitDrawCards();
    }

    private void InitDrawCards()
    {
        drawCards = CardDrawManager.Instance.DrawInitHandCards();
        CardHandManager.Instance.InitHandCardsNeighbor(drawCards);
        
        var listOfHand = CardHandManager.Instance.ListOfHandsCardsPool;
        for (var i = 0; i < drawCards.Count; i++)
        {
            var cardItemManager = listOfHand[i];
            initDrawCards[i].InitialDrawManager.InitDrawCard(cardItemManager.HandCardManager);
        }
    }

    public void MoveToHand()
    {
        drawAgainButton.gameObject.SetActive(false);
        foreach (var card in initDrawCards)
        {
            card.InitialDrawManager.MoveToHand();
        }
        
        foreach (var ui in uiToDisplay)
        {
            ui.DOFade(1f, 0.5f);
        }
    }

}
