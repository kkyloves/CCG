using System.Collections.Generic;
using System.Linq;
using HandCard_Item;
using DG.Tweening;
using Script.Misc;
using Scriptable;
using UnityEngine;

namespace Manager
{
    public class CardHandManager : Singleton<CardHandManager>
    {
        [SerializeField] private List<CardItem> listOfHandCardsPool = new();
        public List<CardItem> ListOfHandsCardsPool => listOfHandCardsPool;
        
        private HandCardManager selectedHandCard;
        
        //instantiate and store hand cards pool
        public void InitHandCardsNeighbor(List<CardDetails> cardDetailsList)
        {
            //listOfHandCardsPool = handCardParent.GetComponentsInChildren<HandCardManager>().ToList(); 
            for (var i = 0; i < listOfHandCardsPool.Count; i++)
            {
                if (i > 0)
                {
                    var handCardManager = listOfHandCardsPool[i].HandCardManager;
                    var leftCardManager = listOfHandCardsPool[i - 1].HandCardManager;

                    handCardManager.CardNeighborController.SetLeftCardNeighbor(leftCardManager);
                    leftCardManager.CardNeighborController.SetRightCardNeighbor(handCardManager);
                }

                listOfHandCardsPool[i].HandCardManager.InitHandCard(cardDetailsList[i], i);
            }
        }

        public void InitHandCards(List<CardDetails> cardDetailsList)
        {
            foreach (var hand in listOfHandCardsPool)
            {
                hand.gameObject.SetActive(false);
            }
            
            for (var i = 0; i < cardDetailsList.Count; i++)
            {
                listOfHandCardsPool[i].HandCardManager.InitHandCard(cardDetailsList[i], i);
            }
        }

        public void SetHandCardClicked(HandCardManager handCardManager)
        {
            selectedHandCard = handCardManager;
        }

        public void ResetSelectedHandCardsPosition()
        {
            if (selectedHandCard != null)
            {
                selectedHandCard.CardButtonController.SetCardButtonEnabled(true);
            }
        }

        public void ResetHandCardsPosition()
        {
            foreach (var handCard in listOfHandCardsPool)
            {
                handCard.HandCardManager.CardRectController.Reset();
            }
        }

        public void SetHandCardsEnabled(bool enable)
        {
            foreach (var handCard in listOfHandCardsPool)
            {
                handCard.HandCardManager.CardButtonController.SetCardButtonEnabled(enable);
                handCard.HandCardManager.CardDragController.enabled = enable || handCard.HandCardManager.CardDragController.IsDragging;
            }
        }
        
        public void RemoveHandCard(int index)
        {
            listOfHandCardsPool[index].GetComponent<CanvasGroup>().DOFade(1f, 0.3f);
            for (var i = index; i < listOfHandCardsPool.Count; i++)
            {
                if (listOfHandCardsPool[i].HandCardManager.CardNeighborController.handCardManagerRightNeighbor == null ||
                    !listOfHandCardsPool[i].HandCardManager.CardNeighborController.handCardManagerRightNeighbor.gameObject.activeInHierarchy)
                {
                    listOfHandCardsPool[i].gameObject.SetActive(false);
                }
                else
                {
                    var rightNeighbor = listOfHandCardsPool[i].HandCardManager.CardNeighborController.handCardManagerRightNeighbor;
                    var rightNeighborCardDetails = rightNeighbor.CardDetails;

                    listOfHandCardsPool[i].HandCardManager.InitHandCard(rightNeighborCardDetails, i);
                }
            }
        }

        public HandCardManager GetOpenSlotHandCard()
        {
            foreach (var handCard in listOfHandCardsPool)
            {
                if (!handCard.gameObject.activeInHierarchy)
                {
                    return handCard.HandCardManager;
                }
            }

            return null;
        }
    }
}