using System;
using System.Collections;
using DG.Tweening;
using Script.Misc;
using Scriptable;
using UI;
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

        public void  DisplayCardChosen(CardDetails cardDetails, Transform boardTargetPosition, Action callback)
        {
            afterDisplayCallback = callback;
            cardItem.Init(cardDetails);

            //reset position and fade status
            var renderCamera = CanvasCameraController.Instance.CanvasCamera.worldCamera;
            var screenPos = HandCardDragTool.Instance.transform.position;
            screenPos.z = 30f;
            var canvasPos = renderCamera.ScreenToWorldPoint(screenPos);

            transform.DOMove(canvasPos, 0f);
            transform.DOScale(new Vector3(1f, 1f, 1f), 0f);
            cardItem.CanvasGroup.DOFade(1f, 0f);
            transform.DORotate(new Vector3(1f, 1f, 1f), 0f);

            //play animations
            transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.3f);
            transform.DOMove(CardDrawManager.Instance.CardDisplayPosition.position, 0.3f).OnComplete(() =>
            {
                StartCoroutine(DisplayBattleCard(boardTargetPosition));
            });
        }

        private IEnumerator DisplayBattleCard(Transform boardTargetPosition)
        {
            yield return new WaitForSeconds(0.5f);

            transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.2f);
            transform.DORotate(new Vector3(10f, 0f, 0f), 0.2f);

            transform.DOMove(boardTargetPosition.position, 0.2f).OnComplete(() =>
            {
                cardItem.CanvasGroup.DOFade(0f, 0f).OnComplete(() =>
                {
                    transform.DOMove(CardDrawManager.Instance.CardDisplayIdlePosition.position, 0f);
                });

                transform.DOScale(new Vector3(4f, 4f, 4f), 0f);
                afterDisplayCallback?.Invoke();
            });
        }
    }
}