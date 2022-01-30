using System.Collections;
using System.Collections.Generic;
using Flippards;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCardHolder : MonoBehaviour, ICardHolder
{
    [SerializeField] private CardsView cardViewPrefab;
    [SerializeField] private GameState gameState;
    [SerializeField] private Transform lastPlayedCardPos;
    private RadialLayout radialLayout;
    private List<CardsView> cardsInHand = new List<CardsView>();
    [SerializeField] private AnimationCurve lastPlayedCurve;
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

    private void Start()
    {
        radialLayout = GetComponent<RadialLayout>();
    }

    public void AddCard(FullCard cardDrawn)
    {
        CardsView cardsView = Instantiate(cardViewPrefab);
        cardsView.transform.SetParent(transform);
        cardsView.Initialize(cardDrawn, this);
        cardsView.DisablePointerEvents();
        cardsInHand.Add(cardsView);
        radialLayout.CalculateLayoutInputVertical();
    }

    public void PlayCard(int cardIndex)
    {
        CardsView cardToPlay = cardsInHand[cardIndex];
        cardsInHand.RemoveAt(cardIndex);
        StartCoroutine(MovePlayedCardToLastPlayed(cardToPlay));
        cardToPlay.transform.SetParent(transform.parent);
        //Destroy(cardToPlay.gameObject);
    }

    IEnumerator MovePlayedCardToLastPlayed(CardsView cardToPlay)
    {
        var timer = 0f;
        var animTime = 0.25f;
        Vector3 startPos = cardToPlay.transform.position;
        Quaternion randRot = Quaternion.Euler(lastPlayedCardPos.rotation.eulerAngles + new Vector3(0, 0, Random.Range(-30, 30)));
        while (timer < animTime)
        {
            timer += Time.deltaTime;
            cardToPlay.transform.position = Vector3.Lerp(startPos, lastPlayedCardPos.position, lastPlayedCurve.Evaluate(timer / animTime));
            cardToPlay.transform.rotation = Quaternion.Slerp(cardToPlay.transform.rotation, randRot, lastPlayedCurve.Evaluate(timer / animTime));
            yield return null;
        }

        cardToPlay.transform.position = lastPlayedCardPos.position;
        cardToPlay.transform.rotation = randRot;
        cardToPlay.transform.SetAsLastSibling();
        Destroy(cardToPlay);
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

    public void PlayCard(CardsView cardPlayed)
    {

    }
}