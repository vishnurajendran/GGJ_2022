using UnityEngine;
using UnityEngine.EventSystems;
using Flippards;
using System.Collections;
using UnityEngine.UI;

public class CardsView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Sprite front;
    private Sprite back;
    private Image image;
    private ICardHolder cardHolder;
    [SerializeField] private AnimationCurve scaleUpCurve;
    [SerializeField] private AnimationCurve scaleDownCurve;
    [SerializeField] private AnimationCurve flipCurve;
    public Sprite cardFace1;
    public Sprite cardFace2;
    private bool isScalingUp;
    private bool isScalingDown;

    private bool isFlipped = false;
    private Transform originalParent;
    private int originalSiblingIndex;

    public void Initialize(FullCard cardData, ICardHolder cardHolder, bool isFlipped = false)
    {
        image = GetComponent<Image>();
        front = Resources.Load<Sprite>(cardData.frontCard.imagePath);
        back = Resources.Load<Sprite>(cardData.backCard.imagePath);
        transform.localScale = Vector3.one;
        image.sprite = front;
        this.isFlipped = isFlipped;
        this.cardHolder = cardHolder;
        originalParent = transform.parent;
    }

    public void DisablePointerEvents()
    {
        image.raycastTarget = false;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        transform.localScale = Vector3.one * 1.5f;
        originalSiblingIndex = transform.GetSiblingIndex();
        transform.SetParent(originalParent.parent);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        transform.localScale = Vector3.one;
        transform.SetParent(originalParent);
        transform.SetSiblingIndex(originalSiblingIndex);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.SetParent(originalParent);
        transform.SetSiblingIndex(originalSiblingIndex);
        cardHolder.PlayCard(this);
    }

    IEnumerator ScaleUp()
    {
        var startScale = transform.localScale.x;
        var finalScale = 1.5f;
        var startPos = transform.localPosition;
        var finalPos = transform.localPosition + transform.up * 2;
        var timer = 0f;
        var animTime = 0.3f;

        while (timer < animTime)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.one * Mathf.Lerp(startScale, finalScale, scaleUpCurve.Evaluate(timer / animTime));
            //transform.position = Vector3.Lerp(startPos, finalPos, scaleUpCurve.Evaluate(timer / animTime));
            yield return null;
        }

        transform.localScale = Vector3.one * finalScale;
        //transform.localPosition = finalPos;
    }

    IEnumerator ScaleDown()
    {
        var startScale = transform.localScale.x;
        var finalScale = 1f;
        var startPos = transform.localPosition;
        var finalPos = transform.localPosition - transform.up * 2;
        var timer = 0f;
        var animTime = 0.1f;

        while (timer < animTime)
        {
            timer += Time.deltaTime;
            transform.localScale = Vector3.one * Mathf.Lerp(startScale, finalScale, scaleDownCurve.Evaluate(timer / animTime));
            //transform.position = Vector3.Lerp(startPos, finalPos, scaleDownCurve.Evaluate(timer / animTime));
            yield return null;
        }

        transform.localScale = Vector3.one * finalScale;
        //transform.localPosition = finalPos;
    }

    public IEnumerator Flip()
    {
        isFlipped = !isFlipped;
        var timer = 0f;
        var animTime = 0.1f;
        var from = transform.localRotation;
        var to = Quaternion.Euler(transform.localRotation.eulerAngles + new Vector3(0, 90, 0));
        while (timer < animTime)
        {
            timer += Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(from, to, flipCurve.Evaluate(timer / animTime));
            yield return null;
        }

        image.sprite = isFlipped ? back : front;
        cardFace1 = image.sprite;
        cardFace2 = isFlipped ? front : back;

        timer = 0f;
        animTime = 0.1f;
        from = transform.localRotation;
        to = Quaternion.Euler(transform.localRotation.eulerAngles + new Vector3(0, -90, 0));
        while (timer < animTime)
        {
            timer += Time.deltaTime;
            transform.localRotation = Quaternion.Lerp(from, to, flipCurve.Evaluate(timer / animTime));
            yield return null;
        }
    }
}
