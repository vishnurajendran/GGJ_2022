using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLetter: MonoBehaviour
{
    [SerializeField] float initialDelay;
    public void Init(float initialDelay)
    {
        this.initialDelay = initialDelay;
    }

    private void Start()
    {
        StartCoroutine(Rotate(initialDelay));
    }

    IEnumerator Rotate(float initialDelay)
    {
        yield return new WaitForSeconds(initialDelay);
        while (true)
        {
            float timeStep = 0;

            Vector3 axis = Vector2.right;
            if(Random.Range(0,200) % 2 == 0)
            {
                axis = Vector2.up;
            }
            
            Vector3 currEuler = transform.eulerAngles;
            Vector3 newEuler = currEuler + (axis * 180);

            while(timeStep <= 1)
            {
                timeStep += Time.deltaTime / 0.15f;
                transform.rotation = Quaternion.Lerp(Quaternion.Euler(currEuler), Quaternion.Euler(newEuler), timeStep);
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(Random.Range(2, 5));
            //transform.Rotate(axis * speed * Time.deltaTime);
            //yield return null;
        }
    }
}
