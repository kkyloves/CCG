using Battle_Cards;
using DG.Tweening;
using Manager;
using Scriptable;
using UI;
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

        private BattleCardManager battleCardToDrop;

        public HandCardManager handCardManager;

        public void Init(HandCardManager handCardManager)
        {
            this.handCardManager = handCardManager;
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

            GetComponent<CanvasGroup>().DOFade(0.5f, 0.3f);
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
            if (CardHandManager.Instance.IsDraggingCards)
            {
                BattlefieldManager.Instance.HidePlayerBattleField();
                GetComponent<CanvasGroup>().DOFade(1f, 0.3f);

                CardHandManager.Instance.SetIsDraggingCard(false);
                return;
            }

            
            if (!CardHandDragController.Instance.IsDragging && isDragCardSetup)
            {
                if (handCardManager.IsDisabled == false)
                {
                    var renderCamera = CanvasCameraController.Instance.CanvasCamera.worldCamera;
                    var screenPos = Input.mousePosition;
                    screenPos.z = 100f;
                    var canvasPos = renderCamera.ScreenToWorldPoint(screenPos);


                    if (cardDetailsToDisplay.CardType == CardType.Creature)
                    {
                        var canPlace = PlayerDataManager.Instance.CanPlaceCreatureCard(cardDetailsToDisplay.EnergyNeeded);


                        if (canPlace)
                        {
                            CardHandManager.Instance.SetIsDraggingCard(true);

                            battleCardToDrop = BattlefieldManager.Instance.GetNearestOpenCreatureSpotCard(canvasPos);

                            CardChosenDisplayController.Instance.DisplayCardChosen(cardDetailsToDisplay, battleCardToDrop.transform, AfterDisplayCallback);
                            BattlefieldManager.Instance.ShowPlayerBattleField();
                        }
                        else
                        {
                            BattlefieldManager.Instance.HidePlayerBattleField();
                            GetComponent<CanvasGroup>().DOFade(1f, 0.3f);
                        }
                    }
                    else
                    {
                        CardHandManager.Instance.SetIsDraggingCard(true);

                        battleCardToDrop = BattlefieldManager.Instance.GetNearestOpenEnergySpotCard(canvasPos);
                        CardChosenDisplayController.Instance.DisplayCardChosen(cardDetailsToDisplay, battleCardToDrop.transform, AfterDisplayCallback);
                    }
                }
                else
                {
                    BattlefieldManager.Instance.HidePlayerBattleField();
                }
                
                HandCardDragTool.Instance.EndDrag();
            
                isDragging = false;
                isDragCardSetup = false;
                enabled = false;
            }
        }

        private void AfterDisplayCallback()
        {
            var isEnergyCard = false;
            battleCardToDrop.UpdateBattleCard(cardDetailsToDisplay);

            if (cardDetailsToDisplay.CardType == CardType.Energy)
            {
                isEnergyCard = true;
                PlayerDataManager.Instance.AddEnergyValue(cardDetailsToDisplay.EnergyType, 1);
            }
            else
            {
                PlayerDataManager.Instance.UseEnergy(cardDetailsToDisplay.EnergyNeeded);
            }
            
            CardHandManager.Instance.SetIsDraggingCard(false);
            HandCardDragTool.Instance.EndDrag();
            
            isDragging = false;
            isDragCardSetup = false;
            enabled = false;
            
            BattlefieldManager.Instance.HidePlayerBattleField();
            CardHandManager.Instance.RemoveHandCard(handIndex, isEnergyCard);
        }
    }
}