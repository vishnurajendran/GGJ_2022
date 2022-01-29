using System.Collections;
using System.Collections.Generic;
using Flippards;
using UnityEngine;

public class PlayerCardHolder : MonoBehaviour
{
    [SerializeField] private CardsView cardViewPrefab;
    [SerializeField] private GameState gameState;
    [SerializeField] private Transform drawnCardStartPos;
    [SerializeField] private Transform drawnCardMidPos;
    [SerializeField] private Transform drawnCardLastPos;
    [SerializeField] private AnimationCurve drawnCardFirstAnimCurve;
    [SerializeField] private AnimationCurve drawnCardSecondAnimCurve;
    [SerializeField] private Transform playedCardEndPos;

    private Queue<FullCard> cardsQueue = new Queue<FullCard>();
    private List<CardsView> cardsInHand = new List<CardsView>();
    private Coroutine cardFanCoroutine;
    private static PlayerCardHolder instance;
    public static PlayerCardHolder Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PlayerCardHolder>();

            return instance;
        }
    }

    public void AddCard(FullCard drawnCard)
    {
        cardsQueue.Enqueue(drawnCard);
        if (cardFanCoroutine == null)
            cardFanCoroutine = StartCoroutine(AddCardToHandAnim());
    }
    public void RemoveCard(CardsView playedCard)
    {
        cardsInHand.Remove(playedCard);
    }

    private IEnumerator AddCardToHandAnim()
    {
        while (cardsQueue.Count > 0)
        {
            FullCard drawnCard = cardsQueue.Dequeue();
            CardsView cardView = Instantiate(cardViewPrefab);
            cardView.Initialize(drawnCard);
            cardsInHand.Add(cardView);

            var timer = 0f;
            var animTime = 0.8f;
            while (timer < animTime)
            {
                timer += Time.deltaTime;
                cardView.transform.position = Vector3.Lerp(drawnCardStartPos.position, drawnCardMidPos.position, drawnCardFirstAnimCurve.Evaluate(timer / animTime));
                yield return null;
            }

            timer = 0f;
            animTime = 3f;
            List<Vector3> cardsInHandStartPosList = new List<Vector3>();
            List<Vector3> cardsInHandFinalPosList = new List<Vector3>();

            Vector3 offset = Vector3.zero;
            for (int i = cardsInHand.Count - 1; i >= 0; i--)
            {
                cardsInHandStartPosList.Add(cardsInHand[i].transform.position);
                cardsInHandFinalPosList.Add(drawnCardLastPos.position + offset);
                offset -= 0.1f * Vector3.right;
                offset -= 0.02f * Vector3.forward;
            }

            while (timer < animTime)
            {
                timer += Time.deltaTime;
                cardView.transform.position = Vector3.Lerp(drawnCardMidPos.position, drawnCardLastPos.position, drawnCardSecondAnimCurve.Evaluate(timer / animTime));
                for (int i = 0; i < cardsInHand.Count - 1; i++)
                {
                    cardsInHand[i].transform.position = Vector3.Lerp(cardsInHandStartPosList[i], cardsInHandFinalPosList[i], drawnCardSecondAnimCurve.Evaluate(timer / animTime));
                }

                yield return null;
            }

            cardView.transform.parent = transform;

        }

        cardFanCoroutine = null;
        yield return null;
    }
}