using System.Collections;
using System.Collections.Generic;
using Flippards;
using UnityEngine;

public class EnemyCardHolder : MonoBehaviour, ICardHolder
{
    [SerializeField] private CardsView cardViewPrefab;
    [SerializeField] private GameState gameState;
    private List<CardsView> cardsInHand = new List<CardsView>();
    private static EnemyCardHolder instance;
    public static EnemyCardHolder Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<EnemyCardHolder>();

            return instance;
        }
    }

    public void AddCard(FullCard cardDrawn)
    {
        CardsView cardsView = Instantiate(cardViewPrefab);
        cardsView.Initialize(cardDrawn, this);
        cardsView.DisablePointerEvents();
        cardsInHand.Add(cardsView);
        cardsView.transform.SetParent(transform);
    }

    public void PlayCard(CardsView cardPlayed)
    {
        gameState.CheckAndTakeTurn(cardsInHand.IndexOf(cardPlayed));
        cardsInHand.Remove(cardPlayed);
        Destroy(cardPlayed.gameObject);
    }

    public void FlipCards()
    {
        StartCoroutine(Flip());
    }

    IEnumerator Flip()
    {
        for (int i = 0; i < cardsInHand.Count; i++)
        {
            yield return StartCoroutine(cardsInHand[i].Flip());
        }
    }
}