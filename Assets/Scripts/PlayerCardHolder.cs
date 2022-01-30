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
        cardsView.transform.SetParent(transform);
        cardsView.Initialize(cardDrawn, this);
        cardsInHand.Add(cardsView);
        radialLayout.CalculateLayoutInputVertical();
    }

    public void PlayCard(CardsView cardPlayed)
    {
        var tra = cardPlayed.transform;
        gameState.CheckAndTakeTurn(cardsInHand.IndexOf(cardPlayed));
        cardsInHand.Remove(cardPlayed);
        cardPlayed.transform.SetParent(transform.parent);
        Destroy(cardPlayed);
        tra.position = lastPlayedCardPos.position;
        StartCoroutine(MovePlayedCardToLastPlayed(tra));
    }

    IEnumerator MovePlayedCardToLastPlayed(Transform cachedTransform)
    {
        var timer = 0f;
        var animTime = 0.25f;
        Vector3 startPos = cachedTransform.position;
        Quaternion randRot = Quaternion.Euler(lastPlayedCardPos.rotation.eulerAngles + new Vector3(0, 0, Random.Range(-30, 30)));
        while (timer < animTime)
        {
            timer += Time.deltaTime;
            cachedTransform.transform.position = Vector3.Lerp(startPos, lastPlayedCardPos.position, lastPlayedCurve.Evaluate(timer / animTime));
            cachedTransform.transform.rotation = Quaternion.Slerp(cachedTransform.rotation, randRot, lastPlayedCurve.Evaluate(timer / animTime));
            yield return null;
        }

        cachedTransform.position = lastPlayedCardPos.position;
        cachedTransform.rotation = randRot;
        cachedTransform.SetAsLastSibling();
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