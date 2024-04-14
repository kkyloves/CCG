using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace HandCard_Item
{
    public class CardRectController : MonoBehaviour
    {
        public Vector2 originalLocalPosition;

        private void Awake()
        {
            originalLocalPosition = transform.localPosition;
        }

        public void AdjustClickSized()
        {
            transform.DOScale(new Vector2(2f, 2f), 0.3f);
        }

        public void ResetMoveToLeft()
        {
            transform.DOLocalMoveX(transform.localPosition.x + 260f, 0.3f);
            originalLocalPosition = new Vector2(originalLocalPosition.x + 260f, originalLocalPosition.y);
        }

        public void MoveToLeft()
        {
            transform.DOLocalMoveX(transform.localPosition.x - 130f, 0.3f);
        }

        public void ResetMoveToRight()
        {
            transform.DOLocalMoveX(transform.localPosition.x - 260f, 0.3f);
            originalLocalPosition = new Vector2(originalLocalPosition.x - 260f, originalLocalPosition.y);
        }

        public void MoveToRight()
        {
            transform.DOLocalMoveX(transform.localPosition.x + 130f, 0.3f);
        }

        public void AdjustAnchorPositionToMid()
        {
            return;
            var panelRectTransform = GetComponent<RectTransform>();
            panelRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            panelRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            panelRectTransform.pivot = new Vector2(0.5f, 0.5f);
        }

        public void Reset()
        {
            transform.DOScale(new Vector2(1f, 1f), 0.3f);
            transform.localPosition = new Vector2(originalLocalPosition.x, originalLocalPosition.y);
        }
    }
}