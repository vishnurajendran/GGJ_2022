using IRIS.RPG.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueOption
{
    public string OptionText;
    public System.Action OnOptionSelected;

    public DialogueOption(string OptionText, System.Action OnOptionSelected)
    {
        this.OptionText = OptionText;
        this.OnOptionSelected = OnOptionSelected;
    }
}

public class DialogueWindow : MonoBehaviour
{
    const string DIALOG_DATA_PATH = "DialogueData/";

    [Header("Dialogue References")]
    [SerializeField] Image leftChar;
    [SerializeField] Image rightChar;
    [SerializeField] RectTransform dialogueBody;
    [SerializeField] CanvasGroup panelCG;
    [SerializeField] TMPro.TMP_Text dialogueText;
    [SerializeField] GameObject moreDialogueArrow;

    [Header("Options References")]
    [SerializeField] GameObject optionsBody;
    [SerializeField] CanvasGroup optionCG;
    [SerializeField] Transform optionsParent;
    [SerializeField] GameObject optionSelectionArrow;
    [SerializeField] RectTransform arrowMoveArea;
    [SerializeField] float delayBwCharAnim = 0.15f;
  Coroutine dialogStartRoutine = null;
    Coroutine dialogueAnimation = null;

    DialogParser dialogueParser;
    System.Action onDialogueCompleted = null;

    [SerializeField] bool isOptionsShowing = false;
    [SerializeField] bool blockInputs = true;

    List<DialogueOption> options = null;
    List<GameObject> optionsObjs;
    DailogueEntry currEntry;
    [SerializeField] int selectedOptionIndex = 0;
    [SerializeField] int prevSelectionIndex = -1;
    [SerializeField] bool dialogueAnimating = false;
    GameObject optionsRef;

    Image activeSpeaker;

    public void StartDialogue(string npcName, System.Action onDialogueCompleted = null)
    {
        optionsBody.SetActive(false);
        optionCG.alpha = 0;
        optionCG.alpha = 0;
        blockInputs = true;

        if (dialogueParser == null)
            dialogueParser = new DialogParser();

        this.onDialogueCompleted = onDialogueCompleted;
        
        //start dialog
        TextAsset dialogData = Resources.Load<TextAsset>(DIALOG_DATA_PATH + npcName);
        dialogueParser.LoadDialogFromTextAsset(dialogData);

        if (dialogStartRoutine != null)
            StopCoroutine(dialogStartRoutine);

        dialogueText.text = "";
        dialogStartRoutine = StartCoroutine(ShowDialogueBox());
    }

    IEnumerator ShowDialogueBox()
    {
        AudioManager.Instance.PlayWhooshSFX();
        float timeStep = 0;
        float startAlpha = panelCG.alpha;
        dialogueBody.gameObject.SetActive(true);
        Vector2 openSize = new Vector2(1070, 150);
        Vector2 startSize = dialogueBody.sizeDelta;
        while (timeStep <= 1)
        {
            timeStep += Time.deltaTime / 0.15f;
            panelCG.alpha = Mathf.Lerp(startAlpha, 1, timeStep);
            dialogueBody.sizeDelta = Vector2.Lerp(startSize, openSize, timeStep);
            yield
            return new WaitForEndOfFrame();
        }

        ShowDialogueText(dialogueParser.GetStartDialogue());
    }

    void ShowDialogueText(DailogueEntry entry)
    {
        HidePreviousSpeaker();
        if (!string.IsNullOrEmpty(entry.Speaker))
        {
            ShowSpeaker(entry.Speaker, entry.IsLeft);
        }

        if (optionsObjs != null)
        {
            foreach (GameObject obj in optionsObjs)
            {
                Destroy(obj);
            }
            optionsObjs.Clear();
        }

        isOptionsShowing = false;
        optionsBody.SetActive(false);
        currEntry = entry;

        List<DialogueOption> options = null;

        if (entry.Options != null && entry.Options.Count > 0)
        {
            options = new List<DialogueOption>();
            foreach (DialogueOptionData option in entry.Options)
            {
                options.Add(new DialogueOption(option.OptionText, () => {

                    if (option.JumpToDialogueID == null)
                    {
                        CloseDialogue();
                    }
                    else
                        OnOptionSelected(option.JumpToDialogueID);

                }));
            }
        }

        if (dialogueAnimation != null)
            StopCoroutine(dialogueAnimation);

        if (entry.NextDialogueEntry != null)
        {
            moreDialogueArrow.SetActive(true);
        }
        else
        {
            moreDialogueArrow.SetActive(false);
        }

        this.options = options;
        dialogueAnimation = StartCoroutine(AnimateDialogText(entry.Dialogue));
    }

    void ShowSpeaker(string speaker, bool isLeft)
    {

        if (isLeft)
        {
            activeSpeaker = leftChar;
        }
        else
        {
            activeSpeaker = rightChar;
        }

        activeSpeaker.gameObject.SetActive(true);
        activeSpeaker.sprite = Resources.Load<Sprite>("Characters/" + speaker);
    }

    void HidePreviousSpeaker()
    {
        if(activeSpeaker != null)
            activeSpeaker.gameObject.SetActive(false);
    }

    IEnumerator AnimateDialogText(string dialogue)
    {
        AudioManager.Instance.PlayDialogueInteractSFX();
        yield
        return new WaitForSeconds(0.15f);
        blockInputs = true;
        dialogueText.text = "";
        int id = 0;
        dialogueAnimating = true;
        while (dialogueText.text.Length < dialogue.Length)
        {
            AudioManager.Instance.PlayDialogueInteractSFX();
            dialogueText.text += dialogue[id++];
            yield
            return new WaitForSeconds(delayBwCharAnim);
        }
        dialogueAnimating = false;
        blockInputs = false;
        OnDialogAnimationComplete();
    }

    void OnDialogAnimationComplete()
    {
        if (options != null)
        {
            ShowDialogueOption(options);
            arrowMoveArea.sizeDelta = new Vector2(arrowMoveArea.sizeDelta.x, optionsParent.GetComponent<RectTransform>().sizeDelta.y);
            UpdateSelection(true);
        }

    }

    void ShowDialogueOption(List<DialogueOption> options)
    {
        optionsBody.SetActive(true);
        if (optionsObjs == null)
            optionsObjs = new List<GameObject>();
        //populate the options for a particular Dialog
        if (optionsRef == null)
            optionsRef = Resources.Load<GameObject>("Prefabs/UI/DialogueOption");

        selectedOptionIndex = 0;
        prevSelectionIndex = -1;
        foreach (DialogueOption option in options)
        {
            GameObject obj = Instantiate(optionsRef, optionsParent);
            TMPro.TMP_Text optionText = obj.GetComponent<TMPro.TMP_Text>();
            optionText.text = option.OptionText;
            optionsObjs.Add(obj);
        }
        isOptionsShowing = true;
    }

    void OnOptionSelected(string key)
    {
        ShowDialogueText(dialogueParser.GetDailogueEntryOfKey(key));
    }

    private void Update()
    {
        if (blockInputs)
            return;

        if (isOptionsShowing)
        {
            if (Input.GetButton("Vertical"))
            {
                float vertical = Input.GetAxis("Vertical");
                int delta = 0;

                if (vertical > 0)
                    delta = -1;
                else if (vertical < 0)
                    delta = 1;

                selectedOptionIndex += delta;
                selectedOptionIndex = Mathf.Clamp(selectedOptionIndex, 0, options.Count - 1);
                if (selectedOptionIndex != prevSelectionIndex)
                {
                    UpdateSelection();
                }
            }

            if (Input.GetButtonDown("Submit"))
            {
                options[selectedOptionIndex].OnOptionSelected?.Invoke();
            }
        }
        else
        {
            if (Input.GetButtonDown("Submit"))
            {
                if (currEntry.NextDialogueEntry == null)
                {
                    CloseDialogue();
                }
                else
                    ShowDialogueText(dialogueParser.GetDailogueEntryOfKey(currEntry.NextDialogueEntry));
            }
        }
    }

    void UpdateSelection(bool ignoreSFX = false)
    {
        Debug.Log("Updating Selection");
        prevSelectionIndex = selectedOptionIndex;
        StartCoroutine(UpdateSelectionAfterFrameEnd());

        if (!ignoreSFX)
            AudioManager.Instance.PlayDialogueInteractSFX();
    }

    IEnumerator UpdateSelectionAfterFrameEnd()
    {
        yield
        return new WaitForEndOfFrame();
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.GetComponent<RectTransform>());
        yield
        return new WaitForEndOfFrame();
        Vector3 currPos = optionSelectionArrow.transform.position;
        optionSelectionArrow.transform.position = new Vector3(currPos.x, optionsObjs[selectedOptionIndex].transform.position.y, 0);
        optionCG.alpha = 1;
    }

    DailogueEntry NextDialog(string nextDialogueID)
    {
        // return next dialog
        return null;
    }

    public void CloseDialogue()
    {
        HidePreviousSpeaker();

        if (dialogStartRoutine != null)
            StopCoroutine(dialogStartRoutine);

        isOptionsShowing = false;
        blockInputs = true;

        dialogueText.text = "";

        if (isOptionsShowing)
        {
            if (optionsObjs != null)
            {
                foreach (GameObject obj in optionsObjs)
                {
                    Destroy(obj);
                }
                optionsObjs.Clear();
            }

            optionsBody.gameObject.SetActive(false);
        }

        dialogStartRoutine = StartCoroutine(CloseDialogueBox());
    }

    IEnumerator CloseDialogueBox()
    {
        AudioManager.Instance.PlayWhooshSFX();
        blockInputs = true;
        float timeStep = 0;
        float startAlpha = panelCG.alpha;
        Vector2 closedSize = new Vector2(0, 125);
        Vector2 startSize = dialogueBody.sizeDelta;
        while (timeStep <= 1)
        {
            timeStep += Time.deltaTime / 0.15f;
            panelCG.alpha = Mathf.Lerp(startAlpha, 0, timeStep);
            dialogueBody.sizeDelta = Vector2.Lerp(startSize, closedSize, timeStep);
            yield
            return new WaitForEndOfFrame();
        }
        onDialogueCompleted?.Invoke();
        onDialogueCompleted = null;
        dialogueBody.gameObject.SetActive(false);
    }
}