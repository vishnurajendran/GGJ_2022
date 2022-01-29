using System;
using System.Collections.Generic;
using Flippards.Helpers;
using UnityEngine;

namespace Flippards
{
    [Serializable]
    public class Deck
    {
        public List<FullCard> FullCardDeck;
        public int CurrentDeckIdx { get; private set; }

        public Deck(List<CardAttributes> frontCards, List<CardAttributes> backCards)
        {
            frontCards.Shuffle();
            backCards.Shuffle();

            FullCardDeck = new List<FullCard>();

            for (int i = 0; i < frontCards.Count; i++)
            {
                FullCard fullCard = new FullCard(frontCards[i], backCards[i]);
                FullCardDeck.Add(fullCard);
            }
            CurrentDeckIdx = 0;
        }

        public FullCard TakeNewCardFromDeck()
        {
            if (FullCardDeck.Count == 0)
            {
                //Debug.LogError($"Ran out of deck!");
                return null;
            }
            var removedCard = FullCardDeck[0];
            FullCardDeck.RemoveAt(0);
            CurrentDeckIdx++;
            return removedCard;
        }
    }
}