using System;
using System.Collections;
using UnityEngine;

namespace Flippards
{
    
    //Class for managing throwing animations and shit as well as calling Vishnu ka health UI and stuff on player
    public class BattleVisualManager : MonoBehaviour
    {
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

        private Transform activeThrowObject;
        private const float ThrowTime = 1f;

        public Action onTurnAnimationsCompleted;

        public void FlipCardsVisually(EntityType player, CardAttributes cardChosen)
        {
            
        }

        public void DealDamage(EntityType targetEntity, CardAttributes cardChosen)
        {
            Debug.Log($"Dealing damage to {targetEntity} with {cardChosen.name}");
            // TODO : Add parameters based on UI needs

            StartCoroutine(LerpShit(targetEntity, cardChosen));
            //call this in  onComplete of LerpShit!!!
            BattleVisuals.Instance.ApplyDamage(targetEntity, cardChosen.value, targetEntity == EntityType.PLAYER ? gameState.PlayerHealthRatio : gameState.EnemyHealthRatio);
        }

        public void GainHealth(EntityType targetEntity, CardAttributes cardChosen)
        {
            Debug.Log($"Adding health to {targetEntity} with {cardChosen.name}");
            // TODO : Add parameters based on UI needs
            Debug.Log($"On {targetEntity} Subtracting value = {cardChosen.value}, P = {gameState.PlayerHealthRatio}, E =  {gameState.EnemyHealthRatio}");
            BattleVisuals.Instance.ApplyDamage(targetEntity, cardChosen.value, targetEntity == EntityType.PLAYER ? gameState.PlayerHealthRatio : gameState.EnemyHealthRatio);
        }

        private IEnumerator LerpShit(EntityType type, CardAttributes cardChosen)
        {
            activeThrowObject = new GameObject(cardChosen.name).transform;

            Vector3 startPos = playerPlaceHolder.position;
            Vector3 endpos = npcPlaceHolder.position;
            if (type == EntityType.ENEMY)
                (startPos, endpos) = (endpos, startPos);

            float currTimer = 0f;
            while (currTimer < ThrowTime)
            {
                currTimer += Time.deltaTime;
                yield return null;
                activeThrowObject.position = Vector3.Lerp(startPos, endpos, currTimer / ThrowTime);
            }

            onTurnAnimationsCompleted?.Invoke();
        }
    }
}