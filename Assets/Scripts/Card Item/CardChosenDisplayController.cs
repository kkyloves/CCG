using System;
using System.Collections;
using DG.Tweening;
using Script.Misc;
using Scriptable;
using UnityEngine;

namespace HandCard_Item
{
    public class CardChosenDisplayController : Singleton<CardChosenDisplayController>
    {
        private CardItem cardItem;
        private Action afterDisplayCallback;

        private void Awake()
        {
            if (gameObject.TryGetComponent(out CardItem cardItem))
            {
                this.cardItem = cardItem;
            }

            cardItem.CanvasGroup.DOFade(0f, 0f);
            transform.DOMove(CardDrawManager.Instance.CardDisplayIdlePosition.position, 0f);
        }

        public void DisplayCardChosen(CardDetails cardDetails, Action callback)
        {
            transform.DOMove(CardDrawManager.Instance.CardDisplayPosition.position, 0f);

            afterDisplayCallback = callback;
            cardItem.Init(cardDetails);
            
            cardItem.CanvasGroup.DOFade(1f, 0.5f).OnComplete(() =>
            {
                StartCoroutine(DisplayBattleCard(cardDetails));
            });
        }

        private IEnumerator DisplayBattleCard(CardDetails cardChosenDisplay)
        {
            yield return new WaitForSeconds(1f);
            cardItem.CanvasGroup.DOFade(0f, 0.5f).OnComplete(() =>
            {
                transform.DOMove(CardDrawManager.Instance.CardDisplayIdlePosition.position, 0f);
            });
            
            afterDisplayCallback?.Invoke();
        }
    }
}