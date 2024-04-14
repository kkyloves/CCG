using DG.Tweening;
using Manager;
using Script.Misc;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HandCard_Item
{
    public class CardHandDragController : Singleton<CardHandDragController>, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Vector2 currentPosition;
        private bool currentDragPositionIsUp;

        private bool isDown = true;
        private bool isDragging = false;
        public bool IsDragging => isDragging;
        
        private void Awake()
        {
            currentPosition = transform.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            CardHandManager.Instance.ResetHandCardsPosition();

            if (Input.mousePosition.y > currentPosition.y && !currentDragPositionIsUp)
            {
                MoveHandCardListUp();
            }
            else if (Input.mousePosition.y < currentPosition.y && currentDragPositionIsUp)
            {
                MoveHandCardListDown();
            }

            isDragging = false;
            currentPosition = transform.position;
        }

        public void MoveHandCardListDown()
        {
            if (!isDown)
            {
                isDown = true;

                currentDragPositionIsUp = false;
                transform.DOLocalMoveY(transform.localPosition.y - 200f, 0.2f);

                CardHandManager.Instance.SetHandCardsEnabled(false);
            }
        }

        private void MoveHandCardListUp()
        {
            if (isDown)
            {
                isDown = false;

                currentDragPositionIsUp = true;
                transform.DOLocalMoveY(transform.localPosition.y + 200f, 0.2f);

                CardHandManager.Instance.SetHandCardsEnabled(true);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
        }
    }
}