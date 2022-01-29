using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flippards;

public class CardsView : MonoBehaviour
{
    private SpriteRenderer front;
    private SpriteRenderer back;
    public Animation anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animation>();
    }

    public void Initialize(FullCard cardData)
    {
        front = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        back = this.transform.GetChild(1).GetComponent<SpriteRenderer>();

        front.sprite = Resources.Load<Sprite>(cardData.frontCard.imagePath);
        back.sprite = Resources.Load<Sprite>(cardData.backCard.imagePath);
    }


    public void Flip()
    {
        anim.Play("CardFlip");
    }

    public void UnFlip()
    {
        anim.Play("CardUnflip");
    }
}
