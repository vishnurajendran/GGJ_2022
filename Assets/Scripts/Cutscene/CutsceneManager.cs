using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{

    private static CutsceneManager instance;
    public static CutsceneManager Instance
    {
        get {
            if (instance == null)
            {
                instance = FindObjectOfType<CutsceneManager>();
            }
            return instance; 
        }
    }

    [SerializeField] DialogueWindow dialogueWindow;
    [SerializeField] Image cutSceneBG;
    [SerializeField] bool isTesting = false;
    
    public BattleAnimEventComplete OnCutsceneComplete;
    
    public void SetImage(Sprite img)
    {
        cutSceneBG.gameObject.SetActive(false);
        if (img == null)
            return;

        cutSceneBG.gameObject.SetActive(true);
        cutSceneBG.sprite = img;
    }

    public void LoadCutsceneFor(string charName)
    {
        OnCutsceneComplete.RemoveListener(OnCutsceneClose);
        OnCutsceneComplete.AddListener(OnCutsceneClose);
        dialogueWindow.StartDialogue(charName, () =>
        {
            OnCutsceneComplete?.Invoke();
        });
    }

    void OnCutsceneClose()
    {
        cutSceneBG.gameObject.SetActive(false);
    }

    private void Start()
    {
        if(isTesting)
            LoadCutsceneFor("Test");
    }
}
