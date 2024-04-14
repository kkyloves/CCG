using DG.Tweening;
using Scriptable;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Battle_Cards
{
    public class BattleCardManager : MonoBehaviour
    {
        [SerializeField] private Button attackButton;
        [SerializeField] private Button targetButton;

        [SerializeField] private Transform upTarget;
        [SerializeField] private Transform downTarget;
        [SerializeField] private CardItem battleCardItem;

        private BattleCardDisplayController battleCardDisplayController;
        public BattleCardDisplayController BattleCardDisplayController => battleCardDisplayController;

        private bool isSlotted;
        public bool IsSlotted => isSlotted;
        public CardItem CardItem => battleCardItem;
        public Button TargetButton => targetButton;
        public Button AttackButton => attackButton;

        private int battlefieldIndex;
        public int BattlefieldIndex => battlefieldIndex;

        public void UpdateBattleCard(CardDetails cardDetails)
        {
            isSlotted = true;

            battleCardItem.Init(cardDetails);
            battleCardItem.CanvasGroup.DOFade(1f, 0.3f);
        }

        private void Awake()
        {
            attackButton.onClick.AddListener(OnClickAttack);
            targetButton.onClick.AddListener(OnClickTarget);

            if (gameObject.TryGetComponent(out BattleCardDisplayController battleCardDisplayController))
            {
                this.battleCardDisplayController = battleCardDisplayController;
            }

            targetButton.gameObject.SetActive(false);
            attackButton.gameObject.SetActive(false);

            battleCardItem.CanvasGroup.DOFade(0f, 0f);
        }

        public void SetIndex(int index)
        {
            battlefieldIndex = index;
        }

        private void OnClickTarget()
        {
            if (ChangePhaseController.Instance.CurrentPhase.Equals(GamePhase.Combat) &&
                BattlefieldManager.Instance.ActiveBattleCardManager != null)
            {
                var attackerBattleCardManager = BattlefieldManager.Instance.ActiveBattleCardManager;
                var attackerBattleCardItem = attackerBattleCardManager.CardItem;
                
                BattlefieldManager.Instance.RemoveActiveAttackBattleCard();
                BattlefieldManager.Instance.SetTargetButtons(null, false);

                var differenceIndex = attackerBattleCardManager.BattlefieldIndex - battlefieldIndex;
                attackerBattleCardItem.transform.DORotate(new Vector3(0f, 0f, 15f * differenceIndex), 0.1f);

                var duration = Mathf.Abs(differenceIndex) switch
                {
                    <= 0 => 0.2f,
                    <= 3 => 0.3f,
                    _ => 0.4f
                };

                attackerBattleCardItem.transform.DOMove(downTarget.position, duration).OnComplete(() =>
                {
                    transform.DORotate(new Vector3(0f, 0f, -20f), 0.1f);
                    
                    attackerBattleCardItem.transform.DORotate(new Vector3(0f, 0f, 0f), duration);
                    battleCardItem.RedHurtEffect.DOFade(0.6f, 0.1f);

                    attackerBattleCardItem.transform.DOMove(attackerBattleCardManager.transform.position, duration).OnComplete(() =>
                    {
                        var attackDamage = attackerBattleCardItem.StatsCounter.AttackCount;
                        var isDead = battleCardItem.UpdateDefStats(attackDamage);

                        if (isDead)
                        {
                            battleCardItem.CanvasGroup.DOFade(0f, 0.5f);
                            
                            isSlotted = false;
                            battleCardItem = null;
                            targetButton.gameObject.SetActive(false);

                            if (BattlefieldManager.Instance.IsEnemyBattleFieldOpen())
                            {
                                EnemyTargetController.Instance.TargetButton.gameObject.SetActive(true);
                            }
                        }
                        else
                        {
                            transform.DORotate(new Vector3(0f, 0f, 0f), 0.1f);
                            battleCardItem.RedHurtEffect.DOFade(0f, 0.1f);
                        }
                    });
                });
            }
        }

        private void OnClickAttack()
        {
            if (ChangePhaseController.Instance.CurrentPhase.Equals(GamePhase.Combat))
            {
                BattlefieldManager.Instance.SetTargetButtons(this, true);
                battleCardItem.CardGlow.DOFade(1f, 0.5f);
            }
        }
    }
}