using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flippards;

public class CardsView : MonoBehaviour
{
    public SpriteRenderer front;
    public SpriteRenderer back;
    public Animation anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animation>();
        Flip();
    }

    public void Initialize(FullCard cardData)
    {
        front = this.transform.GetChild(0).GetComponent<SpriteRenderer>();
        back = this.transform.GetChild(1).GetComponent<SpriteRenderer>();

        front.sprite = Resources.Load<Sprite>("Cards/" + cardData.frontCard.frontImage);
        back.sprite = Resources.Load<Sprite>("Cards/" + cardData.backCard.backImage);
    }

    // Update is called once per frame
    void Update()
    {

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
