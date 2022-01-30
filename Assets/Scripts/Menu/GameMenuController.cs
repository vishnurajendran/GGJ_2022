using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuType
{
    CUTSCENE,
    IN_BATTLE,
    WIN,
    LOSE
}

public class GameMenuController : MonoBehaviour
{
    private static GameMenuController instance;
    public static GameMenuController Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GameMenuController>();

            return instance;
        }
    }

    public static bool isPaused;
    [SerializeField] Transform parentMenu;
    [SerializeField] CanvasGroup blackBG;

    [SerializeField] GameObject cutsceneMenu;
    [SerializeField] GameObject inBattleMenu;
    [SerializeField] GameObject winMenu;
    [SerializeField] GameObject loseMenu;

    BattleAnimEventComplete OnRestartClicked;
    BattleAnimEventComplete OnNextClicked;

    GameObject prevMenu;

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                if (CutsceneManager.Instance.CutSceneActive && !BattleVisuals.Instance.InBattle)
                {
                    ShowMenu(MenuType.CUTSCENE);
                }
                else if (!CutsceneManager.Instance.CutSceneActive && BattleVisuals.Instance.InBattle)
                {
                    ShowMenu(MenuType.IN_BATTLE);
                }
                else
                {
                    if (BattleVisuals.Instance.Win)
                    {
                        ShowMenu(MenuType.WIN);
                    }
                    else
                    {
                        ShowMenu(MenuType.LOSE);
                    }
                }
            }
            else
            {
                if(prevMenu != null)
                    prevMenu.gameObject.SetActive(false);

                parentMenu.gameObject.SetActive(false);
            }
           
        }
    }

    public void ShowMenu(MenuType type)
    {
        parentMenu.gameObject.SetActive(true);
        isPaused = true;

        if (prevMenu != null)
            prevMenu.gameObject.SetActive(false);

        switch (type)
        {
            case MenuType.CUTSCENE:
                prevMenu = cutsceneMenu;
                cutsceneMenu.SetActive(true);
                break;
            case MenuType.IN_BATTLE:
                prevMenu = inBattleMenu;
                inBattleMenu.SetActive(true);
                break;
            case MenuType.WIN:
                prevMenu = winMenu;
                winMenu.SetActive(true);
                break;
            case MenuType.LOSE:
                prevMenu = loseMenu;
                loseMenu.SetActive(true);
                break;
        }
    }

    public void Skip()
    {
        if (prevMenu != null)
            prevMenu.gameObject.SetActive(false);
        isPaused = false;
        parentMenu.gameObject.SetActive(false);
        CutsceneManager.Instance.Skip(); ;
    }

    public void GoHome()
    {
        if (prevMenu != null)
            prevMenu.gameObject.SetActive(false);
        isPaused = false;
        parentMenu.gameObject.SetActive(false);
        StartCoroutine(Fade(() =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
        }));

    }

    public void OnClose()
    {
        if (prevMenu != null)
            prevMenu.gameObject.SetActive(false);
        parentMenu.gameObject.SetActive(false);
        parentMenu.gameObject.SetActive(false);
        isPaused = false;
    }

    public void OnRestart()
    {
        if (prevMenu != null)
            prevMenu.gameObject.SetActive(false);
        isPaused = false;
        parentMenu.gameObject.SetActive(false);
        blackBG.gameObject.SetActive(true);
        StartCoroutine(Fade(() =>
        {
            blackBG.gameObject.SetActive(false);
            OnRestartClicked?.Invoke();
        }));
    }

    public void OnNext()
    {
        if (prevMenu != null)
            prevMenu.gameObject.SetActive(false);
        isPaused = false;
        parentMenu.gameObject.SetActive(false);
        blackBG.gameObject.SetActive(true);
        StartCoroutine(Fade(() =>
        {
            blackBG.gameObject.SetActive(false);
            OnNextClicked?.Invoke();
        }));
    }

    IEnumerator Fade(System.Action onComplete)
    {
        float timeStep = 0;
        while(timeStep < 1)
        {
            timeStep += Time.deltaTime;
            blackBG.alpha = Mathf.Lerp(0, 1, timeStep);
            yield return new WaitForEndOfFrame();
        }
        onComplete?.Invoke();
    } 
}
