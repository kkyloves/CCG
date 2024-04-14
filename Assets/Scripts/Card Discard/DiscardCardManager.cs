using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Manager;
using Script.Misc;
using Scriptable;
using UnityEngine;
using UnityEngine.UI;

public class DiscardCardManager : Singleton<DiscardCardManager>
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CanvasGroup[] uiToHide;

    [SerializeField] private CardItem[] discardTempCards;
    [SerializeField] private Transform voidTransform;
    
    [SerializeField] private Transform discardParent;
    [SerializeField] private Transform discardTempParent;

    private void Awake()
    {
        canvasGroup.DOFade(0f, 0f);
        
        discardTempParent.gameObject.SetActive(false);
        discardParent.gameObject.SetActive(false);
    }

    public void InitDiscardCards()
    {
        foreach (Transform child in discardParent)
        {
            child.gameObject.SetActive(false);
        }

        discardTempParent.gameObject.SetActive(true);
        discardParent.gameObject.SetActive(true);
        StartCoroutine(StartProcessDisplayDiscard());
    }

    private IEnumerator StartProcessDisplayDiscard()
    {
        yield return new WaitForSeconds(0.1f);

        canvasGroup.DOFade(1f, 0f);
        voidTransform.DOScale(1.5f, 0.5f);

        foreach (var ui in uiToHide)
        {
            ui.DOFade(0, 0.5f);
        }

        var listOfHand = CardHandManager.Instance.ListOfHandsCardsPool;
        for (var i = 0; i < listOfHand.Count; i++)
        {
            if (listOfHand[i].gameObject.activeInHierarchy)
            {
                var cardItemManager = listOfHand[i];
                var discardItem = discardParent.GetChild(i).GetComponent<CardItem>();
                
                discardItem.gameObject.SetActive(true);
                discardItem.DiscardCardItem.InitDiscardCard(listOfHand[i].HandCardManager.CardDetails, cardItemManager.HandCardManager, discardTempCards[i]);
            }
        }
    }

    public void RemoveDiscardCard(DiscardCardItem card)
    {
        card.transform.SetAsLastSibling();
    }

    public void MoveToHand()
    {
        var activeCards = GetAvailableDiscardCards();
        CardHandManager.Instance.InitHandCards(activeCards);

        foreach (var ui in uiToHide)
        {
            ui.DOFade(1f, 0.5f);
        }

        voidTransform.DOScale(1f, 0.5f);

        for (var i = 0; i < activeCards.Count; i++)
        {
            var card = discardParent.GetChild(i).GetComponent<CardItem>();
            if (card.isActiveAndEnabled)
            {
                card.DiscardCardItem.MoveToHand(CardHandManager.Instance.ListOfHandsCardsPool[i].HandCardManager);
            }
        }

        StartCoroutine(DrawCards());
    }

    private IEnumerator DrawCards()
    {
        yield return new WaitForSeconds(1f);

        CardDrawManager.Instance.DrawCard();
        PlayerDataManager.Instance.ResetEnergy();
        
        discardTempParent.gameObject.SetActive(false);
        discardParent.gameObject.SetActive(false);
    }

    private List<CardDetails> GetAvailableDiscardCards()
    {
        var discardCards = new List<CardDetails>();

        foreach (Transform child in discardParent)
        {
            if (child.gameObject.activeInHierarchy)
            {
                discardCards.Add(child.GetComponent<DiscardCardItem>().CardDetails);
            }
        }

        return discardCards;
    }
}