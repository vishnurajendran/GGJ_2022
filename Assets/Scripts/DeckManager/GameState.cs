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

    private bool isPlayerFlipped;
    private bool isEnemyFlipped;

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

    private void DoTurn(FullCard cardPlayed, bool isPlayer = true)
    {
        CardAttributes cardToEval = isPlayer ? isPlayerFlipped ? cardPlayed.backCard : cardPlayed.frontCard : isEnemyFlipped ? cardPlayed.backCard : cardPlayed.frontCard;
        if (isPlayer)
        {
            if (cardToEval.cardType == CardType.Flip)
            {
                isEnemyFlipped = !isEnemyFlipped;
                //Update flip UI
            }
            else
            {
                if (lastPlayerCard != null)
                {
                    if (cardToEval.cardType == CardType.Hit)
                    {
                        enemyHealth -= GetModifiedStatValue(cardToEval);
                        enemyHealth = Mathf.Clamp(enemyHealth, 0, enemyMaxHealth);
                        //Battle visuals send health and percentage
                    }
                    else if (cardToEval.cardType == CardType.Heal)
                    {
                        playerHealth += GetModifiedStatValue(cardToEval);
                        playerHealth = Mathf.Clamp(playerHealth, 0, playerHealth);
                    }
                }
            }
        }
        else
        {
            if (lastPlayerCard != null)
            {
                if (cardToEval.cardType == CardType.Flip)
                {
                    isPlayerFlipped = !isPlayerFlipped;
                    //Update flip UI
                }
                else
                {
                    if (lastPlayerCard != null)
                    {
                        if (cardToEval.cardType == CardType.Hit)
                        {
                            playerHealth -= GetModifiedStatValue(cardToEval);
                            playerHealth = Mathf.Clamp(playerHealth, 0, playerHealth);
                        }
                        else if (cardToEval.cardType == CardType.Heal)
                        {
                            enemyHealth += GetModifiedStatValue(cardToEval);
                            enemyHealth = Mathf.Clamp(enemyHealth, 0, enemyMaxHealth);
                        }
                    }
                }
            }
        }

        lastPlayerCard = cardToEval;
    }

    private int GetModifiedStatValue(CardAttributes playedCard)
    {
        if (playedCard.cardClass == lastPlayerCard.cardClass)
        {
            return playedCard.value;
        }
        else
        {
            if (playedCard.cardClass == CardClass.Liquid && lastPlayerCard.cardClass == CardClass.Weight || playedCard.cardClass == CardClass.Weight && lastPlayerCard.cardClass == CardClass.Paper || playedCard.cardClass == CardClass.Paper && lastPlayerCard.cardClass == CardClass.Liquid)
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
            else if (playedCard.cardClass == CardClass.Weight && lastPlayerCard.cardClass == CardClass.Liquid || playedCard.cardClass == CardClass.Paper && lastPlayerCard.cardClass == CardClass.Weight || playedCard.cardClass == CardClass.Liquid && lastPlayerCard.cardClass == CardClass.Paper)
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