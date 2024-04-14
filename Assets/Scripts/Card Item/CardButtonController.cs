using System.Collections;
using Manager;
using UnityEngine;
using UnityEngine.UI;

namespace HandCard_Item
{
    public class CardButtonController : MonoBehaviour
    {
        private Button cardButton;
        private HandCardManager handCardManager;
        private CardRectController cardRectController;
        private CardNeighborController cardNeighborController;
        
        public void SetCardButtonEnabled(bool enabled)
        {
            cardButton.image.enabled = enabled;
        }

        public void Init(HandCardManager handCardManager)
        {
            this.handCardManager = handCardManager;
            cardRectController = handCardManager.CardRectController;
            cardNeighborController = handCardManager.CardNeighborController;

            cardButton = gameObject.GetComponent<Button>();
            cardButton.onClick.AddListener(OnClickCardButton);
        }

        private void OnClickCardButton()
        {
            CardHandManager.Instance.ResetSelectedHandCardsPosition();
            CardHandManager.Instance.SetHandCardClicked(handCardManager);
            OnClickCardButtonProcess();
        }

        private void OnClickCardButtonProcess()
        {
            CardHandManager.Instance.ResetHandCardsPosition();

            SetCardButtonEnabled(false);
            cardRectController.AdjustClickSized();
            cardNeighborController.MoveLeftNeighbor();
            cardNeighborController.MoveRightNeighbor();
        }
    }
}