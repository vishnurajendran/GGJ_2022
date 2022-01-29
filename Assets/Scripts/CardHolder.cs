using System.Collections;
using System.Collections.Generic;
using Flippards;
using UnityEngine;

public class CardHolder : MonoBehaviour
{
    [SerializeField] private CardsView cardViewPrefab;
    [SerializeField] private GameState gameState;
    private Queue<FullCard> cardsQueue = new Queue<FullCard>();
    private List<CardsView> cardsInHand = new List<CardsView>();
    private Coroutine cardFanCoroutine;
    private static CardHolder instance;
    public static CardHolder Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<CardHolder>();

            return instance;
        }
    }

    public void AddCard(FullCard cardDrawn)
    {
        CardsView cardsView = Instantiate(cardViewPrefab);
        cardsView.Initialize(cardDrawn, this);
        cardsInHand.Add(cardsView);
        cardsView.transform.SetParent(transform);
    }

    public void PlayCard(CardsView cardPlayed)
    {
        cardsInHand.Remove(cardPlayed);
    }

    public void Flip()
    {
        
    }
}