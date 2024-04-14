using Script.Misc;
using Scriptable;
using UnityEngine;

namespace HandCard_Item
{
    public class HandCardDragTool : Singleton<HandCardDragTool>
    {
        private CardItem cardItem;
        
        private void Awake()
        {
            if (gameObject.TryGetComponent(out CardItem cardItem))
            {
                this.cardItem = cardItem;
            }
        }

        public void InitCard(CardDetails cardDetails)
        {
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