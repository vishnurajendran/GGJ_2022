using Flippards;
using UnityEngine;

public partial class GameState
{
    public FullCard GetAIMove()
    {
        var randomValue = Random.Range(0, 100);

        if (randomValue <= 30)
        {
            //Completely random
            return GetRandomCard();
        }

        if (randomValue <= 60)
        {
            //Most powerful card
            return GetMostPowerfulCard();
        }

        if (randomValue <= 80)
        {
            //Most Poweruped powerful card!
            return GetFancyMostPowerfulCard();
        }

        if (randomValue <= 90)
        {
            //Heal because why not, else random
            return GetFirstHealCard();
        }

        //play flip, else random
        return GetFirstFlipCard();
    }


    private FullCard GetRandomCard()
    {
        return enemyHand[Random.Range(0, enemyHand.Count)];
    }

    private FullCard GetMostPowerfulCard()
    {
        int maxVal = 0;
        FullCard cardToReturn = null;
        foreach (var t in enemyHand)
        {
            if (t.GetTopCardAttributes().value < maxVal)
                continue;
            maxVal = t.GetTopCardAttributes().value;
            cardToReturn = t;
        }

        return cardToReturn ?? GetRandomCard();
    }

    private FullCard GetFancyMostPowerfulCard()
    {
        int maxVal = 0;
        FullCard cardToReturn = null;
        foreach (var t in enemyHand)
        {
            if (GetModifiedStatValue(t.GetTopCardAttributes()) < maxVal)
                continue;
            maxVal = t.GetTopCardAttributes().value;
            cardToReturn = t;
        }

        return cardToReturn ?? GetRandomCard();
    }

    private FullCard GetFirstHealCard()
    {
        foreach (var t in enemyHand)
        {
            if (t.GetTopCardAttributes().cardType == CardType.Heal)
                return t;
        }

        return GetRandomCard();
    }

    private FullCard GetFirstFlipCard()
    {
        foreach (var t in enemyHand)
        {
            if (t.GetTopCardAttributes().cardType == CardType.Flip)
                return t;
        }

        return GetRandomCard();
    }
}