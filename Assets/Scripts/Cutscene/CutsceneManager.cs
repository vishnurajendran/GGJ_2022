using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    

    private static CutsceneManager instance;
    public static CutsceneManager Instance
    {
        get { 
            if(instance == null)
                instance = FindObjectOfType<CutsceneManager>();
            return instance; 
        }
    }

    [SerializeField] DialogueWindow dialogueWindow;
    public BattleAnimEventComplete OnCutsceneComplete;

    public void LoadCutsceneFor(string charName)
    {
        dialogueWindow.StartDialogue(charName, () =>
        {
            OnCutsceneComplete?.Invoke();
        });
    }

    private void Start()
    {
       
    }
}
