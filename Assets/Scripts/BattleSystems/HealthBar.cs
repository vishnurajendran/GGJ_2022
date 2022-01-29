using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class BattleAnimEventComplete : UnityEvent
{

}
public class HealthBar : MonoBehaviour
{
#pragma warning disable CS0649 // Field 'HealthBar.shatterAnim' is never assigned to, and will always have its default value null
    [SerializeField] GameObject shatterAnim;
#pragma warning restore CS0649 // Field 'HealthBar.shatterAnim' is never assigned to, and will always have its default value null
#pragma warning disable CS0649 // Field 'HealthBar.border' is never assigned to, and will always have its default value null
    [SerializeField] GameObject border;
#pragma warning restore CS0649 // Field 'HealthBar.border' is never assigned to, and will always have its default value null
#pragma warning disable CS0649 // Field 'HealthBar.bg' is never assigned to, and will always have its default value null
    [SerializeField] GameObject bg;
#pragma warning restore CS0649 // Field 'HealthBar.bg' is never assigned to, and will always have its default value null
    
#pragma warning disable CS0649 // Field 'HealthBar.cg' is never assigned to, and will always have its default value null
    [SerializeField] CanvasGroup cg;
#pragma warning restore CS0649 // Field 'HealthBar.cg' is never assigned to, and will always have its default value null
#pragma warning disable CS0649 // Field 'HealthBar.fill' is never assigned to, and will always have its default value null
    [SerializeField] Image fill;
#pragma warning restore CS0649 // Field 'HealthBar.fill' is never assigned to, and will always have its default value null

    Coroutine animRoutine = null;

    public float RemainingHealth
    {
        get
        {
            return fill.fillAmount;
        }
    }

    public void SetHealth(float perc, float dur=0.15f)
    {
        if (animRoutine != null)
            StopCoroutine(animRoutine);

        animRoutine = StartCoroutine(Animate(perc, dur));
    }

    IEnumerator Animate(float perc, float dur)
    {
        float timeStep = 0;
        float currAlpha = cg.alpha;
        while(timeStep <= 1)
        {
            timeStep += Time.deltaTime / dur;
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

        if (fill.fillAmount <= 0)
        {
            shatterAnim.SetActive(true);
            fill.gameObject.SetActive(false);
            border.gameObject.SetActive(false);
            bg.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            yield return new WaitForSeconds(1);
        }

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
