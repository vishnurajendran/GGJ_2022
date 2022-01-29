using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Flippards
{
    public class DeckManager : MonoBehaviour
    {
#pragma warning disable CS0649 // Field 'DeckManager.cardDB' is never assigned to, and will always have its default value null
        [SerializeField] private CardDB cardDB;
#pragma warning restore CS0649 // Field 'DeckManager.cardDB' is never assigned to, and will always have its default value null

        [SerializeField] private Deck MasterDeck;

        public void GenerateDeck()
        {
            List<CardAttributes> frontCards = new List<CardAttributes>();
            List<CardAttributes> backCards = new List<CardAttributes>();
            for (var i = 0; i < cardDB.frontCards.Count; i++) //FrontCard count and back count card should be same!!!!
            {
                var frontCardTemplate = cardDB.frontCards[i];
                var backCardTemplate = cardDB.backCards[i];
                // Debug.Log($"Starting for {frontCardTemplate.name} and {backCardTemplate.name}");

                for (int j = 0; j < frontCardTemplate.count; j++)
                {
                    // Debug.Log($"Timers = {j}");
                    frontCards.Add(new CardAttributes(frontCardTemplate));
                }

                for (int j = 0; j < backCardTemplate.count; j++)
                {
                    // Debug.Log($"Timers = {j}");
                    backCards.Add(new CardAttributes(backCardTemplate));
                }
            }
            MasterDeck = new Deck(frontCards, backCards);
        }

        public FullCard DrawCardFromMasterDeck()
        {
            return MasterDeck.TakeNewCardFromDeck();
        }
    }
}