using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Animator animator;


    void Start()
    {
        AudioManager.Instance.PlayMenuTheme();
    }

    public void OnPlay()
    {
        AudioManager.Instance.PlayWhooshSFX();
        playButton.interactable = false;
        animator.Play("FillScreen");
        Invoke("LoadGame", 2.75f);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    void LoadGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TestBeds_anims_mine");
    }
}
