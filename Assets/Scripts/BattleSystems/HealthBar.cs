using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] CanvasGroup cg;
    [SerializeField] Image fill;

    Coroutine animRoutine = null;

    public void SetHealth(float perc)
    {
        if (animRoutine != null)
            StopCoroutine(animRoutine);

        animRoutine = StartCoroutine(Animate(perc));
    }

    IEnumerator Animate(float perc)
    {
        float timeStep = 0;
        float currAlpha = cg.alpha;
        while(timeStep <= 1)
        {
            timeStep += Time.deltaTime / 0.15f;
            cg.alpha = Mathf.Lerp(currAlpha, 1, timeStep);
            yield return new WaitForEndOfFrame();
        }

        timeStep = 0;
        float currFill = fill.fillAmount;
        while (timeStep <= 1)
        {
            timeStep += Time.deltaTime / 0.15f;
            fill.fillAmount = Mathf.Lerp(currFill, perc, timeStep);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(2);
        timeStep = 0;
        currAlpha = cg.alpha;
        while (timeStep <= 1)
        {
            timeStep += Time.deltaTime / 0.15f;
            cg.alpha = Mathf.Lerp(currAlpha, 0, timeStep);
            yield return new WaitForEndOfFrame();
        }
    }
}
