using UnityEngine;
using UnityEngine.EventSystems;
using Flippards;
using System.Collections;
using UnityEngine.UI;

public class CardsView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Sprite front;
    private Sprite back;
    private Image image;
    private CardHolder cardHolder;
    [SerializeField] private AnimationCurve scaleUpCurve;
    [SerializeField] private AnimationCurve scaleDownCurve;

    public void Initialize(FullCard cardData, CardHolder cardHolder)
    {
        image = GetComponent<Image>();
        front = Resources.Load<Sprite>(cardData.frontCard.imagePath);
        back = Resources.Load<Sprite>(cardData.backCard.imagePath);
        image.sprite = front;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        StartCoroutine(ScaleUp());
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        StartCoroutine(ScaleDown());
    }

    IEnumerator ScaleUp()
    {
        transform.localScale = Vector3.one;
        var timer = 0f;
        var animTime = 0.4f;

        while(timer > animTime)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.one * Mathf.Lerp(1, 1.2f, scaleUpCurve.Evaluate(timer / animTime));
            yield return null;
        }

        transform.localScale = Vector3.one * 1.2f;
    }

    IEnumerator ScaleDown()
    {
        var timer = 0f;
        var animTime = 0.2f;
        transform.localScale = Vector3.one * 1.2f;
        while(timer > animTime)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.one * Mathf.Lerp(1, 1.2f, scaleUpCurve.Evaluate(timer / animTime));
            yield return null;
        }

        transform.localScale = Vector3.one;
    }
}
