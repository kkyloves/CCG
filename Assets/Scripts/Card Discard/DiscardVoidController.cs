using System;
using System.Collections;
using DG.Tweening;
using Manager;
using Script.Misc;
using Scriptable;
using UnityEngine;
using UnityEngine.UI;

public class DiscardVoidController : Singleton<DiscardVoidController>
{
    [SerializeField] private CardItem cardItem;
    private Action callback;

    private bool isDiscarding;
    public bool IsDiscarding => isDiscarding;
    
    private void Awake()
    {
        cardItem.CanvasGroup.DOFade(0f, 0f);
    }
    
    public void InitDiscardItem(CardDetails cardDetails, Action callback)
    {
        isDiscarding = true;
        
        this.callback = callback;
        cardItem.Init(cardDetails);
        cardItem.transform.DOScale(1f, 0f);
        
        cardItem.CanvasGroup.DOFade(1f, 0.5f);
        cardItem.transform.DOScale(1.5f, 0.5f).OnComplete(() =>
        {
            StartCoroutine(Discard(cardDetails));
        });
    }

    private IEnumerator Discard(CardDetails cardDetails)
    {
        yield return new WaitForSeconds(1f);

        if (cardDetails.DoesRequireEnergyType(EnergyType.Void))
        {
            PlayerDataManager.Instance.AddEnergyValue(EnergyType.Void, 1);
        }

        cardItem.transform.DOScale(0f, 0.5f).OnComplete(() =>
        {
            callback?.Invoke();
            isDiscarding = false;
        });
    }
    
    
}
