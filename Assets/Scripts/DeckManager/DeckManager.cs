using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Flippards
{
    public class DeckManager : MonoBehaviour
    {
        [SerializeField] private CardDB cardDB;
        
        [SerializeField] private Deck MasterDeck;

        [Button("Generate Deck")]
        public void GenerateDeck()
        {
            List<CardAttributes> frontCards = new List<CardAttributes>();
            List<CardAttributes> backCards = new List<CardAttributes>();
            for (var i = 0; i < cardDB.frontCards.Count; i++) //FrontCard count and back count card should be same!!!!
            {
                var frontCardTemplate = cardDB.frontCards[i];
                var backCardTemplate = cardDB.backCards[i];

                for (int j = 0; j < frontCardTemplate.count; j++)
                {
                    frontCards.Add(frontCardTemplate); //ref copy please don't break my shit.
                }
                for (int j = 0; j < backCardTemplate.count; j++)
                {
                    backCards.Add(backCardTemplate); //ref copy please don't break my shit.
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