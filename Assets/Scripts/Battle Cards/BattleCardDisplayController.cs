using DG.Tweening;
using Scriptable;
using UnityEngine;
using UnityEngine.UI;

namespace Battle_Cards
{
    public class BattleCardDisplayController : MonoBehaviour
    {
        [SerializeField] private Image battleCardSlotImage;

        private void Awake()
        {
            battleCardSlotImage.DOFade(0f, 0.3f);
        }
        
        public void ShowBattleCardSlot()
        {
            battleCardSlotImage.DOFade(0.5f, 0.3f);
        }
        
        public void HideBattleCardSlot()
        {
            battleCardSlotImage.DOFade(0f, 0.3f);
        }
    }
}
