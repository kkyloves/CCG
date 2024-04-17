using DG.Tweening;
using Scriptable;
using UI;
using Unity.VisualScripting;
using UnityEngine;

namespace HandCard_Item
{
    public class HandCardDragTool : Script.Misc.Singleton<HandCardDragTool>
    {
        private CardItem cardItem;
        public CardItem CardItem => cardItem;
        
        private void Awake()
        {
            if (gameObject.TryGetComponent(out CardItem cardItem))
            {
                this.cardItem = cardItem;
            }
        }

        public void InitCard(CardDetails cardDetails)
        {
            transform.DOScale(new Vector3(1f, 1f, 1f), 0f);
            cardItem.Init(cardDetails);
            gameObject.SetActive(true);
        }

        public void DragPosition(Vector2 position)
        {
            transform.position = position;
        }

        public void EndDrag()
        {
            gameObject.SetActive(false);
        }
    }
}