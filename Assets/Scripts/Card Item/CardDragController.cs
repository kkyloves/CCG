using DG.Tweening;
using Manager;
using Scriptable;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HandCard_Item
{
    public class CardDragController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private CardDetails cardDetails;
        private CardDetails cardDetailsToDisplay;
        
        private int handIndex;
        public int HandIndex => handIndex;

        private bool isDragging;
        public bool IsDragging => isDragging;

        private bool isDraggingDown;
        private bool isDragCardSetup;

        public void Init(HandCardManager handCardManager)
        {
            cardDetails = handCardManager.CardDetails;
        }

        public void SetIndex(int handIndex)
        {
            this.handIndex = handIndex;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!CardHandDragController.Instance.IsDragging)
            {
                // Debug.Log("Card Drag Controller Up");
                //
                // isDragging = true;
                // cardDetailsToDisplay = cardDetails;
                //
                // CardHandDragController.Instance.MoveHandCardListDown();
                // CardHandManager.Instance.ResetHandCardsPosition();
                //
                // CardHandDragTool.Instance.InitCard(cardDetailsToDisplay.CardSprite);
                // CardHandDragTool.Instance.DragPosition(eventData.position);
                // BattlefieldManager.Instance.ShowPlayerBattleField();
                //
                // GetComponent<Image>().DOFade(0.5f, 0.3f);
            }
        }

        private void SetupDragCard(Vector2 dragPosition)
        {
            isDragCardSetup = true;
            isDragging = true;
            cardDetailsToDisplay = cardDetails;

            CardHandDragController.Instance.MoveHandCardListDown();
            CardHandManager.Instance.ResetHandCardsPosition();

            HandCardDragTool.Instance.InitCard(cardDetailsToDisplay);
            HandCardDragTool.Instance.DragPosition(dragPosition);
            BattlefieldManager.Instance.ShowPlayerBattleField();

            GetComponent<Image>().DOFade(0.5f, 0.3f);
        }

        public void OnDrag(PointerEventData eventData)
        {
            var positiveY = Mathf.Abs(eventData.position.y);
            if (positiveY > 300f)
            {
                if (!isDragCardSetup)
                {
                    SetupDragCard(eventData.position);
                }

                if (!CardHandDragController.Instance.IsDragging)
                {
                    HandCardDragTool.Instance.DragPosition(eventData.position);
                }
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!CardHandDragController.Instance.IsDragging && isDragCardSetup)
            {
                HandCardDragTool.Instance.EndDrag();

                if (cardDetailsToDisplay.CardType == CardType.Creature)
                {
                    var canPlace = PlayerDataManager.Instance.CanPlaceMinionCard(cardDetailsToDisplay.EnergyNeeded);

                    if (canPlace)
                    {
                        CardChosenDisplayController.Instance.DisplayCardChosen(cardDetailsToDisplay, AfterDisplayCallback);
                        BattlefieldManager.Instance.ShowPlayerBattleField();
                    }
                    else
                    {
                        GetComponent<Image>().DOFade(1f, 0.3f);
                    }
                }
                else
                {
                    CardChosenDisplayController.Instance.DisplayCardChosen(cardDetailsToDisplay, AfterDisplayCallback);
                }
                
                isDragging = false;
                isDragCardSetup = false;
                enabled = false;
            }
        }

        private void AfterDisplayCallback()
        {
            if (cardDetailsToDisplay.CardType == CardType.Energy)
            {
                var card = BattlefieldManager.Instance.GetNearestOpenEnergySpotCard(HandCardDragTool.Instance.transform.position);
                card.UpdateBattleCard(cardDetailsToDisplay);

                PlayerDataManager.Instance.AddEnergyValue(cardDetailsToDisplay.EnergyType, 1);
            }
            else
            {
                var card = BattlefieldManager.Instance.GetNearestOpenCreatureSpotCard(HandCardDragTool.Instance.transform.position);
                card.UpdateBattleCard(cardDetailsToDisplay);
                
                PlayerDataManager.Instance.UseEnergy(cardDetailsToDisplay.EnergyNeeded);
            }
            
            BattlefieldManager.Instance.HidePlayerBattleField();
            CardHandManager.Instance.RemoveHandCard(handIndex);
        }
    }
}