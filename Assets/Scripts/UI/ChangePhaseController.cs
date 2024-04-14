using System;
using DG.Tweening;
using Manager;
using Script.Misc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public enum GamePhase
    {
        InitialDraw,
        Draw,
        Planning,
        Combat,
        Discard
    }

    public class ChangePhaseController : Singleton<ChangePhaseController>
    {
        [SerializeField] private Button goToNextPhase;
        [SerializeField] private TextMeshProUGUI combatPhase;
        [SerializeField] private Image[] highlightedImages;

        private GamePhase currentPhase = GamePhase.InitialDraw;
        public GamePhase CurrentPhase => currentPhase;
        
        private void Awake()
        {
            goToNextPhase.onClick.AddListener(OnClickNextPhase);

            foreach (var image in highlightedImages)
            {
                image.DOFade(0f, 0f);
            }

            highlightedImages[0].DOFade(1f, 0f);
        }

        private void OnClickNextPhase()
        {
            switch (currentPhase)
            {
                case GamePhase.InitialDraw:
                    InitialDrawCardsController.Instance.MoveToHand();
                    currentPhase = GamePhase.Planning;
                    highlightedImages[0].DOFade(0f, 0.5f);
                    highlightedImages[1].DOFade(1f, 0.5f);

                    combatPhase.text = "GO TO COMBAT PHASE";
                    break;

                case GamePhase.Draw:
                    // currentPhase = GamePhase.Planning;
                    // highlightedImages[1].DOFade(0f, 0.5f);
                    // highlightedImages[2].DOFade(1f, 0.5f);
                    break;

                case GamePhase.Planning:
                    BattlefieldManager.Instance.TurnOnAttackButtons();
                    
                    currentPhase = GamePhase.Combat;
                    highlightedImages[1].DOFade(0f, 0.5f);
                    highlightedImages[2].DOFade(1f, 0.5f);
                    
                    combatPhase.text = "GO TO DISCARD PHASE";
                    break;

                case GamePhase.Combat:
                    DiscardCardManager.Instance.InitDiscardCards();
                    
                    currentPhase = GamePhase.Discard;
                    highlightedImages[2].DOFade(0f, 0.5f);
                    highlightedImages[3].DOFade(1f, 0.5f);
                    
                    combatPhase.text = "END TURN";
                    break;

                case GamePhase.Discard:
                    // CardDrawManager.Instance.DrawCard();
                    // PlayerDataManager.Instance.ResetEnergy();
                    
                    DiscardCardManager.Instance.MoveToHand();
                    BattlefieldManager.Instance.ResetBattleCards();
                    
                    currentPhase = GamePhase.Planning;
                    
                    highlightedImages[3].DOFade(0f, 0.5f);
                    highlightedImages[1].DOFade(1f, 0.5f);
                    
                    combatPhase.text = "GO TO COMBAT PHASE";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}