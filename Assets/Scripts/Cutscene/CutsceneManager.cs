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

#pragma warning disable CS0649 // Field 'CutsceneManager.dialogueWindow' is never assigned to, and will always have its default value null
    [SerializeField] DialogueWindow dialogueWindow;
#pragma warning restore CS0649 // Field 'CutsceneManager.dialogueWindow' is never assigned to, and will always have its default value null
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
