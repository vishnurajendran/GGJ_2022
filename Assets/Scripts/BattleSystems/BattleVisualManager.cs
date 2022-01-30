using System;
using System.Collections;
using System.Collections.Generic;
using Flippards.Helpers;
using UnityEngine;

namespace Flippards
{
    //Class for managing throwing animations and shit as well as calling Vishnu ka health UI and stuff on player
    public class BattleVisualManager : MonoBehaviour
    {
        [SerializeField] private GameObject throwThingPrefab;

        public GameState gameState;
        private static BattleVisualManager instance;

        public static BattleVisualManager Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<BattleVisualManager>();

                return instance;
            }
        }

        [SerializeField] private Transform playerPlaceHolder;
        [SerializeField] private Transform npcPlaceHolder;


        public Action onTurnAnimationsCompleted;
        public Action<EntityType> onFlipTargetChosen;

        public void InitVisuals()
        {
            BattleVisuals.Instance.onBeginComplete.AddListener(() =>
            {
                BattleVisuals.Instance.SetupEntity(npcPlaceHolder.GetChild(0), EntityType.ENEMY);
                BattleVisuals.Instance.SetupEntity(playerPlaceHolder.GetChild(0), EntityType.PLAYER);
            });
            BattleVisuals.Instance.PlayBegin();
        }

        public void FlipCardsVisually(EntityType entity)
        {
            //List<FullCard> cardsToFlip = entity == EntityType.PLAYER ? gameState.enemyHand : gameState.playerHand;
            // foreach (var fullCard in cardsToFlip)
            // {
            //     fullCard.view.Flip();
            // }
            BattleVisuals.Instance.AddHitFX(HitFXType.FLIP, entity == EntityType.PLAYER ? playerPlaceHolder : npcPlaceHolder);
            string s = $"Flipped {entity}s Cards.";
            Debug.Log($"{s.GetRichText("yellow")}");
            onTurnAnimationsCompleted?.Invoke();

        }

        public void DealDamage(EntityType targetEntity, CardAttributes cardChosen, int value)
        {

            ThrownObject activeThrowObject = Instantiate(throwThingPrefab).GetComponent<ThrownObject>();
            activeThrowObject.gameObject.name = $"Throwed {cardChosen.name}";

            Vector3 startPos = playerPlaceHolder.position;
            Vector3 endPos = npcPlaceHolder.position;
            if (targetEntity == EntityType.PLAYER)
                (startPos, endPos) = (endPos, startPos);

            activeThrowObject.Init(gameState.autoTurn, startPos, endPos, OnThrowObjectReached);

            void OnThrowObjectReached()
            {
                BattleVisuals.Instance.AddHitFX(GetFXTypeFromClass(cardChosen), targetEntity == EntityType.PLAYER ? playerPlaceHolder : npcPlaceHolder);

                Debug.Log($"Dealing {"damage".GetRichText("red")} to {targetEntity} {value.ToString().GetRichText("red")}");
                onTurnAnimationsCompleted?.Invoke();
                BattleVisuals.Instance.ApplyDamage(targetEntity, value, targetEntity == EntityType.PLAYER ? gameState.PlayerHealthRatio : gameState.EnemyHealthRatio);
            }
        }

        private HitFXType GetFXTypeFromClass(CardAttributes card)
        {
            bool bhaariWala = card.value < gameState.GetModifiedStatValue(card);
            switch (card.cardClass)
            {
                case CardClass.Weight:
                    return bhaariWala ? HitFXType.HEAVY_HIT : HitFXType.LIGHT_HIT;
                case CardClass.Liquid:
                    return bhaariWala ? HitFXType.LIQUID_HEAVY : HitFXType.LIQUID_LIGHT;
                case CardClass.Paper:
                    return bhaariWala ? HitFXType.PAPER_HEAVY : HitFXType.PAPER_LIGHT;
            }
            return HitFXType.NONE;
        }
        public void GainHealth(EntityType targetEntity, CardAttributes cardChosen, int value)
        {
            // TODO : Add parameters based on UI needs
            Debug.Log($"Dealing {"Health".GetRichText("green")} to {targetEntity} {value.ToString().GetRichText("green")}");
            BattleVisuals.Instance.AddHitFX(HitFXType.HEAL, targetEntity == EntityType.PLAYER ? playerPlaceHolder : npcPlaceHolder);
            BattleVisuals.Instance.ApplyDamage(targetEntity, value, targetEntity == EntityType.PLAYER ? gameState.PlayerHealthRatio : gameState.EnemyHealthRatio, true);
            onTurnAnimationsCompleted?.Invoke();
        }

        public void ShowVictory()
        {
            BattleVisuals.Instance.PlayWin();
        }

        public void ShowDefeat()
        {
            BattleVisuals.Instance.PlayLose();
        }
    }
}