using System.Collections;
using DG.Tweening;
using HandCard_Item;
using Script.Misc;
using Scriptable;
using UnityEngine;
using UnityEngine.UI;

public class CardDrawAnimationTool : Singleton<CardDrawAnimationTool>
{
    private CardItem cardItem;
    private Image cardImage;
    
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

    public void InitDrawCard(CardDetails cardDetails, HandCardManager finalHandPosition)
    {
        cardImage.sprite = CardSpriteStorage.Instance.BackCardSprite;
        
        cardItem.CanvasGroup.DOFade(1f, 0f);
        cardImage.DOFade(1f, 0f);

        transform.DOLocalRotate(new Vector3(180f, 0f, 0), 0f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear);
        transform.DOMove(CardDrawManager.Instance.InitialPosition.position, 0f);
        transform.DOScale(new Vector2(1f, 1f), 0f);

        gameObject.SetActive(true);
        StartCoroutine(StartDrawAnimation(cardDetails, finalHandPosition));
    }

    private IEnumerator StartDrawAnimation(CardDetails cardDetails, HandCardManager handHandCardManager)
    {
        yield return new WaitForSeconds(0.2f);
        
        transform.DOScale(new Vector2(2f, 2f), 0.5f);

        StartCoroutine(FlipCard());
        StartCoroutine(ChangeSprite(cardDetails));

        transform.DOLocalMove(CardDrawManager.Instance.CardDrawAnimationLandPosition.localPosition, 0.5f).OnComplete(() =>
        {
            StartCoroutine(MoveToHand(handHandCardManager));
        });
    }

    private IEnumerator FlipCard()
    {
        yield return new WaitForSeconds(0.1f);
        transform.DOLocalRotate(new Vector3(180f, 0f, 0), 0.4f, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear);
    }

    private IEnumerator ChangeSprite(CardDetails cardDetails)
    {
        yield return new WaitForSeconds(0.3f);
        cardItem.Init(cardDetails);
        
        cardItem.CanvasGroup.DOFade(1f, 0f);
        cardImage.DOFade(0f, 0f);
    }
    
    private IEnumerator MoveToHand(HandCardManager finalHandPosition)
    {
        yield return new WaitForSeconds(0.3f);
        transform.DOScale(new Vector2(1f, 1f), 0.5f);
        transform.DOMove(finalHandPosition.HandPosition.position, 0.5f).OnComplete(() =>
        {
            finalHandPosition.gameObject.SetActive(true);
            gameObject.SetActive(false);
        });
    }
}
