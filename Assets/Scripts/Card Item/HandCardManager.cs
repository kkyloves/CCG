using DG.Tweening;
using Manager;
using Scriptable;
using UnityEngine;
using UnityEngine.UI;

namespace HandCard_Item
{
    public class HandCardManager : MonoBehaviour
    {
        public Transform handPosition;
        private CardItem cardItem;
        private CardRectController cardRectController;
        private CardButtonController cardButtonController;
        private CardNeighborController cardNeighborController;
        private CardDragController cardDragController;
        private CardDetails cardDetails;

        public CardButtonController CardButtonController => cardButtonController;
        public CardRectController CardRectController => cardRectController;
        public CardNeighborController CardNeighborController => cardNeighborController;
        public CardDragController CardDragController => cardDragController;
        public CardDetails CardDetails => cardDetails;
        public Transform HandPosition => handPosition;

        private bool isDisabled;
        public bool IsDisabled => isDisabled;

        private void Awake()
        {
            if (gameObject.TryGetComponent(out CardItem cardItem))
            {
                this.cardItem = cardItem;
            }

            cardRectController = gameObject.AddComponent<CardRectController>();

            cardButtonController = gameObject.AddComponent<CardButtonController>();
            //add button
            var button = gameObject.AddComponent<Button>();
            var image = gameObject.AddComponent<Image>();
            button.image = image;
            image.DOFade(0f, 0f);

            cardNeighborController = gameObject.AddComponent<CardNeighborController>();
            cardDragController = gameObject.AddComponent<CardDragController>();

            var newHandPosition = new GameObject();
            newHandPosition.transform.DOMove(new Vector2(transform.position.x, transform.position.y + 140f), 0f);
            handPosition = newHandPosition.transform;
            handPosition.transform.parent = transform;

            InitComponents();
        }

        private void InitComponents()
        {
            cardButtonController.Init(this);
            cardNeighborController.Init(this);
            cardDragController.Init(this);

            cardButtonController.SetCardButtonEnabled(false);
            cardDragController.enabled = false;
            gameObject.SetActive(false);
        }

        public void InitHandCard(CardDetails cardDetails, int index, bool disabled)
        {
            this.cardDetails = cardDetails;
            cardItem.Init(cardDetails);

            cardDragController.SetIndex(index);
            cardDragController.Init(this);

            isDisabled = disabled;

            if (disabled)
            {
                cardItem.DisableEffect.DOFade(0.7f, 0f);
            }
            else
            {
                cardItem.DisableEffect.DOFade(0f, 0f);
            }
        }

        public int GetIndex()
        {
            return cardDragController.HandIndex;
        }

        public void SetIsDisable(bool isDisabled)
        {
            this.isDisabled = isDisabled;
        }
    }
}