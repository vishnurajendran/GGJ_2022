using UnityEngine;
using Flippards;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public class GameState : MonoBehaviour
{
    private DeckManager deckManager;
    [SerializeField] private List<FullCard> playerHand;
    [SerializeField] private List<FullCard> enemyHand;

    [SerializeField] private int playerHealth;
    [SerializeField] private int playerMaxHealth;
    [SerializeField] private int enemyHealth;
    [SerializeField] private int enemyMaxHealth;


    private bool isPlayersTurn = true;
    private CardAttributes lastPlayerCard;

    public int startingHandCount;

    [Button("Generate Deck and distribute initial!")]
    private void InitGame()
    {
        deckManager = GetComponent<DeckManager>();
        deckManager.GenerateDeck();

        playerHand = new List<FullCard>();
        enemyHand = new List<FullCard>();
        for (int i = 0; i < 2 * startingHandCount; i++)
        {
            if (i % 2 == 0)
            {
                playerHand.Add(deckManager.DrawCardFromMasterDeck());
            }
            else
            {
                enemyHand.Add(deckManager.DrawCardFromMasterDeck());
            }
        }

        BattleVisualManager.Instance.onTurnAnimationsCompleted += StartNextTurn;
    }
    

    private void StartNextTurn()
    {
        FullCard cardDrawn = deckManager.DrawCardFromMasterDeck();
        //UI Update with card
        if (isPlayersTurn)
        {
            playerHand.Add(cardDrawn);
        }
        else
        {
            enemyHand.Add(cardDrawn);
        }
    }

    [Button("Do Player Turn!")]
    private void DoPlayerTurn()
    {
        var cardToPlay = playerHand[Random.Range(0, playerHand.Count)];
        playerHand.Remove(cardToPlay);
        DoTurn(cardToPlay, EntityType.PLAYER);

    }

    [Button("Do Enemy Turn!")]
    private void DoEnemyTurn()
    {
        var cardToPlay = enemyHand[Random.Range(0, enemyHand.Count)];
        enemyHand.Remove(cardToPlay);
        DoTurn(cardToPlay, EntityType.ENEMY);
    }


    private void DoTurn(FullCard cardPlayed, EntityType entityType = EntityType.PLAYER)
    {
        CardAttributes cardToEval = cardPlayed.GetTopCardAttributes();
        Debug.Log($"{entityType} turn. Played {cardToEval.name}. Type = {cardToEval.cardType} and value = {cardToEval.value}");

        // if (isPlayer)
        //     cardToEval = isPlayerFlipped ? cardPlayed.backCard : cardPlayed.frontCard;
        // else
        //     cardToEval = isEnemyFlipped ? cardPlayed.backCard : cardPlayed.frontCard;

        if (cardToEval.cardType == CardType.Flip)
        {
            FlipCards(entityType, cardToEval);
        }
        else if (entityType == EntityType.PLAYER)
        {
            if (lastPlayerCard != null)
            {
                if (cardToEval.cardType == CardType.Hit)
                {
                    enemyHealth -= GetModifiedStatValue(cardToEval);
                    enemyHealth = Mathf.Clamp(enemyHealth, 0, enemyMaxHealth);
                    //Battle visuals send health and percentage
                    BattleVisualManager.Instance.DealDamage(EntityType.ENEMY, cardToEval);
                }
                else if (cardToEval.cardType == CardType.Heal)
                {
                    playerHealth += GetModifiedStatValue(cardToEval);
                    playerHealth = Mathf.Clamp(playerHealth, 0, playerHealth);
                    BattleVisualManager.Instance.GainHealth(EntityType.PLAYER, cardToEval);
                }
            }
        }
        else if (entityType == EntityType.ENEMY)
        {
            if (lastPlayerCard != null)
            {
                if (cardToEval.cardType == CardType.Hit)
                {
                    playerHealth -= GetModifiedStatValue(cardToEval);
                    playerHealth = Mathf.Clamp(playerHealth, 0, playerHealth);
                    BattleVisualManager.Instance.DealDamage(EntityType.PLAYER, cardToEval);
                }
                else if (cardToEval.cardType == CardType.Heal)
                {
                    enemyHealth += GetModifiedStatValue(cardToEval);
                    enemyHealth = Mathf.Clamp(enemyHealth, 0, enemyMaxHealth);
                    BattleVisualManager.Instance.GainHealth(EntityType.ENEMY, cardToEval);
                }
            }
        }

        lastPlayerCard = cardToEval;
    }

    private void FlipCards(EntityType entity, CardAttributes cardToEval)
    {
        BattleVisualManager.Instance.FlipCardsVisually(entity, cardToEval);
        List<FullCard> cardsToFlip = entity == EntityType.PLAYER ? enemyHand : playerHand;
        foreach (var t in cardsToFlip)
        {
            t.isCardFlipped = !t.isCardFlipped;
            Debug.Log($"Flipping for {entity} and updated card name = {t.GetTopCardAttributes().name}");
        }
    }

    private int GetModifiedStatValue(CardAttributes playedCard)
    {
        if (playedCard.cardClass == lastPlayerCard.cardClass)
        {
            return playedCard.value;
        }
        else
        {
            if (playedCard.cardClass == CardClass.Liquid && lastPlayerCard.cardClass == CardClass.Weight ||
                playedCard.cardClass == CardClass.Weight && lastPlayerCard.cardClass == CardClass.Paper ||
                playedCard.cardClass == CardClass.Paper && lastPlayerCard.cardClass == CardClass.Liquid)
            {
                if (playedCard.cardType == CardType.Hit)
                {
                    return playedCard.value * 2;
                }
                else if (playedCard.cardType == CardType.Heal)
                {
                    return playedCard.value + 1;
                }
            }
            else if (playedCard.cardClass == CardClass.Weight && lastPlayerCard.cardClass == CardClass.Liquid ||
                     playedCard.cardClass == CardClass.Paper && lastPlayerCard.cardClass == CardClass.Weight ||
                     playedCard.cardClass == CardClass.Liquid && lastPlayerCard.cardClass == CardClass.Paper)
            {
                if (playedCard.cardType == CardType.Hit)
                {
                    return playedCard.value / 2;
                }
                else if (playedCard.cardType == CardType.Heal)
                {
                    return playedCard.value - 1;
                }
            }
        }

        return 0;
    }
}