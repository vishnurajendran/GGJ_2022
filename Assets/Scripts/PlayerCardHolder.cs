using System.Collections;
using System.Collections.Generic;
using Flippards;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCardHolder : MonoBehaviour, ICardHolder
{
    [SerializeField] private CardsView cardViewPrefab;
    [SerializeField] private GameState gameState;
    [SerializeField] private Transform lastPlayedCardPos;
    private RadialLayout radialLayout;
    private List<CardsView> cardsInHand = new List<CardsView>();
    [SerializeField] private AnimationCurve lastPlayedCurve;
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

    private void Start()
    {
        radialLayout = GetComponent<RadialLayout>();
    }

    public void AddCard(FullCard cardDrawn)
    {
        CardsView cardsView = Instantiate(cardViewPrefab);
        cardsView.Initialize(cardDrawn, this);
        cardsInHand.Add(cardsView);
        cardsView.transform.SetParent(transform);
        radialLayout.CalculateLayoutInputVertical();
    }

    public void PlayCard(CardsView cardPlayed)
    {
        gameState.CheckAndTakeTurn(cardsInHand.IndexOf(cardPlayed));
        cardsInHand.Remove(cardPlayed);
        cardPlayed.transform.SetParent(transform.parent);
        StartCoroutine(MovePlayedCardToLastPlayed(cardPlayed));
        //Destroy(cardPlayed.gameObject);
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
        cardToPlay.transform.rotation = lastPlayedCardPos.rotation;
        cardToPlay.transform.SetAsLastSibling();
        Destroy(cardToPlay);
    }

    public void FlipCards()
    {
        StartCoroutine(Flip());
    }

    IEnumerator Flip()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < cardsInHand.Count; i++)
        {
            StartCoroutine(cardsInHand[i].Flip());
            yield return new WaitForSeconds(0.15f);
        }
    }
}