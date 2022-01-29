using UnityEngine;
using Flippards;
using System.Collections.Generic;

public class GameState : MonoBehaviour
{
    private DeckManager deckManager;
    private List<FullCard> playerHand;
    private List<FullCard> enemyHand;

    private int playerHealth;
    private int playerMaxHealth;
    private int enemyHealth;
    private int enemyMaxHealth;


    private bool isPlayersTurn = true;
    private CardAttributes lastPlayerCard;

    public int startingHandCount;

    private void Start()
    {
        deckManager = GetComponent<DeckManager>();
        deckManager.GenerateDeck();

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

    private void DoTurn(FullCard cardPlayed, EntityType entityType = EntityType.PLAYER)
    {
        CardAttributes cardToEval = cardPlayed.GetTopCardAttributes();
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

        //TODO : ask venkat!
        lastPlayerCard = cardToEval;
    }

    private void FlipCards(EntityType entity, CardAttributes cardToEval)
    {
        BattleVisualManager.Instance.FlipCardsVisually(entity, cardToEval);
        //TODO : Discuss, Flip only opponents cards? Or both?
        List<FullCard> cardsToFlip = entity == EntityType.PLAYER ? enemyHand : playerHand;
        foreach (var t in cardsToFlip)
        {
            t.isCardFlipped = !t.isCardFlipped;
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