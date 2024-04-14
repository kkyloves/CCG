using System;
using System.Collections;
using DG.Tweening;
using HandCard_Item;
using Scriptable;
using UnityEngine;
using UnityEngine.UI;

public class InitialDrawManager : MonoBehaviour
{
    private CardItem cardItem;
    private Image cardImage;

    public HandCardManager handCardManager;

    private void Awake()
    {
        if (gameObject.TryGetComponent(out CardItem cardItem))
        {
            this.cardItem = cardItem;
        }

        cardImage = new GameObject
        {
            transform =
            {
                position = new Vector3(transform.position.x, transform.position.y, transform.position.z) //set it's position 
            }
        }.AddComponent<Image>();

        cardImage.transform.parent = transform.GetChild(0).transform;
        
        var objRectTransform = cardImage.GetComponent<RectTransform>();
        objRectTransform.sizeDelta = cardItem.BackgroundTransform.GetComponent<RectTransform>().sizeDelta;
        objRectTransform.localScale = new Vector3(1f, 1f, 1f);
        objRectTransform.DOLocalRotate(new Vector3(0f, 180f, 0), 0f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear);
    }

    public void InitDrawCard(HandCardManager handCardManager)
    {
        this.handCardManager = handCardManager;
        cardImage.sprite = CardSpriteStorage.Instance.BackCardSprite;
        
        cardImage.DOFade(1f, 0f);
        transform.DOLocalRotate(new Vector3(0f, 180f, 0), 0f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear);

        gameObject.SetActive(true);
        StartCoroutine(StartDrawAnimation(handCardManager.CardDetails));
    }

    private IEnumerator StartDrawAnimation(CardDetails cardDetails)
    {
        yield return new WaitForSeconds(0.2f);
        
        StartCoroutine(FlipCard());
        StartCoroutine(ChangeSprite(cardDetails));
    }

    private IEnumerator FlipCard()
    {
        yield return new WaitForSeconds(0.1f);
        cardImage.DOFade(0f, 0.3f);
        transform.DOLocalRotate(new Vector3(0f, 180f, 0), 0.4f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear);
    }

    private IEnumerator ChangeSprite(CardDetails cardDetails)
    {
        yield return new WaitForSeconds(0.3f);
        cardItem.Init(cardDetails);
    }

    public void MoveToHand()
    {
        StartCoroutine(MoveToHandCoroutine());
    }
    
    private IEnumerator MoveToHandCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        transform.DOScale(new Vector2(1f, 1f), 0.5f);
        transform.DOMove(handCardManager.HandPosition.position, 0.5f).OnComplete(() =>
        {
            handCardManager.gameObject.SetActive(true);
            gameObject.SetActive(false);
            
            InitialDrawCardsController.Instance.gameObject.SetActive(false);
        });
    }
}
