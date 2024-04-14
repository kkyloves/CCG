using DG.Tweening;
using Script.Misc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EnemyTargetController : Singleton<EnemyTargetController>
    {
        [SerializeField] private TextMeshProUGUI hpText;
        [SerializeField] private Button targetButton;
        [SerializeField] private Transform targetTransform;
        [SerializeField] private Color damagedColor;
        [SerializeField] private Color originalColor;

        public Button TargetButton => targetButton;
        
        private int totalHp = 30;
        
        private void Awake()
        {
            hpText.text = totalHp.ToString();

            targetButton.onClick.AddListener(OnClickTarget);
            targetButton.gameObject.SetActive(false);
        }

        private void OnClickTarget()
        {
            if (ChangePhaseController.Instance.CurrentPhase.Equals(GamePhase.Combat) &&
                BattlefieldManager.Instance.ActiveBattleCardManager != null &&
                BattlefieldManager.Instance.IsEnemyBattleFieldOpen())
            {
                var attackerBattleCardManager = BattlefieldManager.Instance.ActiveBattleCardManager;
                var attackerBattleCardItem = attackerBattleCardManager.CardItem;
                
                var differenceIndex = attackerBattleCardManager.BattlefieldIndex - 5;
                attackerBattleCardItem.transform.DORotate(new Vector3(0f, 0f, 10f * differenceIndex), 0.1f);
                
                var duration = attackerBattleCardManager.BattlefieldIndex switch
                {
                    <= 0 => 0.4f,
                    <= 3 => 0.3f,
                    _ => 0.2f
                };

                attackerBattleCardItem.transform.DOMove(targetTransform.position, duration).OnComplete(() =>
                {
                    attackerBattleCardItem.transform.DORotate(new Vector3(0f, 0f, 0f), duration);
                    
                    var attackDamage = attackerBattleCardItem.StatsCounter.AttackCount;
                    totalHp -= attackDamage;
                    
                    hpText.DOColor(damagedColor, 0.1f);
                    
                    if (totalHp <= 0)
                    {
                        totalHp = 0;
                        //game over
                    }
                    else
                    {
                        //not yet
                    }
                    
                    hpText.text = totalHp.ToString();

                    attackerBattleCardItem.transform.DOMove(attackerBattleCardManager.transform.position, duration).OnComplete(() =>
                    {

                        hpText.DOColor(originalColor, 0.5f);
                    });
                });
            }
        }
    }
}
