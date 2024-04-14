using UnityEngine;
using UnityEngine.UI;

namespace HandCard_Item
{
    public class CardNeighborController : MonoBehaviour
    {
        public HandCardManager handCardManagerLeftNeighbor;
        public HandCardManager handCardManagerRightNeighbor;

        private CardRectController cardRectController;

        public void Init(HandCardManager handCardManager)
        {
            cardRectController = handCardManager.CardRectController;
        }

        public void SetLeftCardNeighbor(HandCardManager handCardManager)
        {
            handCardManagerLeftNeighbor = handCardManager;
        }

        public void SetRightCardNeighbor(HandCardManager handCardManager)
        {
            handCardManagerRightNeighbor = handCardManager;
        }

        public void PositionAfterDragLeft()
        {
            cardRectController.ResetMoveToLeft();
            if (handCardManagerLeftNeighbor != null)
            {
                handCardManagerLeftNeighbor.CardNeighborController.PositionAfterDragLeft();
            }
        }

        public void MoveLeftNeighbor()
        {
            if (handCardManagerLeftNeighbor != null)
            {
                handCardManagerLeftNeighbor.CardRectController.MoveToLeft();
                handCardManagerLeftNeighbor.CardNeighborController.MoveLeftNeighbor();
            }
        }

        public void PositionAfterDragRight()
        {
            cardRectController.ResetMoveToRight();
            if (handCardManagerRightNeighbor != null)
            {
                handCardManagerRightNeighbor.CardNeighborController.PositionAfterDragRight();
            }
        }

        public void MoveRightNeighbor()
        {
            if (handCardManagerRightNeighbor != null)
            {
                handCardManagerRightNeighbor.CardRectController.MoveToRight();
                handCardManagerRightNeighbor.CardNeighborController.MoveRightNeighbor();
            }
        }
    }
}
