using System.Collections;
using DG.Tweening;
using HandCard_Item;
using Scriptable;
using UnityEngine;
using UnityEngine.UI;

public class DiscardCardItem : MonoBehaviour
{
    private Button discardButton;

    private CardDetails cardDetails;
    public CardDetails CardDetails => cardDetails;

    private CardItem cardItem;
    public CardItem discardTempCard;
    
    private void Awake()
    {
        if (gameObject.TryGetComponent(out CardItem cardItem))
        {
            this.cardItem = cardItem;
        }
        
        discardButton = gameObject.AddComponent<Button>();
        var image = gameObject.AddComponent<Image>();
        discardButton.image = image;
        image.DOFade(0f, 0f);
        
        discardButton.onClick.AddListener(DiscardToVoid);
    }

    public void InitDiscardCard(CardDetails handCardDetails, HandCardManager handHandCard, CardItem discardTempCard)
    {
        this.discardTempCard = discardTempCard;
        
        cardDetails = handHandCard.CardDetails;
        handHandCard.gameObject.SetActive(false);
        
        // discardTempCard.sprite = handCardDetails;
        // discardCardImage.sprite = handCardDetails;
        
        this.discardTempCard.Init(handCardDetails);
        cardItem.Init(handCardDetails);

        discardTempCard.transform.DOScale(new Vector2(1f, 1f), 0f);
        discardTempCard.transform.DOMove(handHandCard.HandPosition.position, 0f);

        cardItem.CanvasGroup.DOFade(0f, 0f);
        discardTempCard.CanvasGroup.DOFade(1f, 0f);
        
        discardTempCard.gameObject.SetActive(true);
        discardTempCard.transform.DOScale(new Vector2(1.5f, 1.5f), 0.3f);

        discardTempCard.transform.DOMove(transform.position, 0.3f).OnComplete(() =>
        {
            discardTempCard.gameObject.SetActive(false);
            cardItem.CanvasGroup.DOFade(1f, 0f);
        });
    }

    private void DiscardToVoid()
    {
        if (!DiscardVoidController.Instance.IsDiscarding)
        {
            DiscardCardManager.Instance.RemoveDiscardCard(this);
            DiscardVoidController.Instance.InitDiscardItem(cardItem.CardDetails, () =>
                {
                    cardItem.CanvasGroup.DOFade(0f, 0.3f);
                });

            gameObject.SetActive(false);
        }
    }
    
    public void MoveToHand(HandCardManager handCardManager)
    {
        StartCoroutine(MoveToHandCoroutine(handCardManager));
    }
    
    private IEnumerator MoveToHandCoroutine(HandCardManager handCardManager)
    {
        yield return new WaitForSeconds(0.3f);
        discardTempCard.gameObject.SetActive(true);

        discardTempCard.CanvasGroup.DOFade(1f, 0f);
        cardItem.CanvasGroup.DOFade(0f, 0f);

        discardTempCard.transform.DOMove(transform.position, 0f);
        discardTempCard.transform.DOScale(new Vector2(1f, 1f), 0.3f);
        
        discardTempCard.transform.DOMove(handCardManager.HandPosition.position, 0.3f).OnComplete(() =>
        {
            discardTempCard.CanvasGroup.DOFade(0f, 0f);
            handCardManager.gameObject.SetActive(true);
            gameObject.SetActive(false);
        });
    }
    
}
