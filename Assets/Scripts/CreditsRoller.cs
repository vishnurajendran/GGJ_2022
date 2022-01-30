using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsRoller : MonoBehaviour
{
    [SerializeField] CanvasGroup overlay;
    
    [SerializeField] List<string> credits;
    [SerializeField] List<GameObject> gameObjects;

    [SerializeField] Transform top;
    [SerializeField] Transform bottom;
    [SerializeField] GameObject creditObj;

    List<Coroutine> routines;
    Coroutine fadeRoutine;
    public void PlayCredits()
    {
        if(routines == null)
            routines = new List<Coroutine>();

        routines.Clear();

        Debug.Log("Fuck");
        this.gameObject.SetActive(true);
        float baseDelay = 1.5f;
        float deltaDelay = 0.5f;

        int i = 0;
        foreach(string credit in credits)
        {
            GameObject obj = Instantiate(creditObj, this.transform);
            gameObjects.Add(obj);
            obj.SetActive(true);
            obj.name = credit;
            obj.GetComponent<TMPro.TMP_Text>().text = credit;
            obj.transform.position = bottom.position;
            routines.Add(StartCoroutine(RollCredit(obj, baseDelay + (routines.Count * deltaDelay))));
        }

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(Fade(true, 1));
    }

    IEnumerator Fade(bool fadeIn, float dur)
    {
        float timeStep = 0;
        float currAlpha = overlay.alpha;
        float toFade = fadeIn ? 1f : 0f;
        while(timeStep <= 1)
        {
            timeStep += Time.deltaTime / dur;
            overlay.alpha = Mathf.Lerp(currAlpha, toFade, timeStep);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
        if(!fadeIn)
            this.gameObject.SetActive(false);
    }

    IEnumerator RollCredit (GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        while (true)
        {
            obj.transform.position += obj.transform.up * 100 * Time.deltaTime;

            if (obj.transform.position.y >= top.position.y)
                obj.transform.position = bottom.position;

            yield return new WaitForEndOfFrame();
        }
    }

    public void CloseCredits()
    {
        //delete all this shit
        while(gameObjects.Count > 0)
        {
            GameObject obj = gameObjects[0];
            Destroy(obj);
            gameObjects.RemoveAt(0);
        }

        gameObjects.Clear();

        for(int i = 0; i < routines.Count; i++)
        {
            if(routines[i] != null)
                StopCoroutine(routines[i]);
        }

        routines.Clear();

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(Fade(false, 1)); ;
    }
}
