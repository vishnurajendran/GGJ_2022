using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IRIS.RPG.DialogueSystem
{
    public class DailogueEntry
    {
        public string Dialogue;
        public string Speaker="";
        public bool IsLeft=false;
        public List<DialogueOptionData> Options;
        public string NextDialogueEntry;
    }

    public class DialogueOptionData
    {
        public string OptionText;
        public string JumpToDialogueID;
    }

    public class DialogParser
    {
        Dictionary<string, DailogueEntry> dialogs;

        public void LoadDialogFromTextAsset(TextAsset textAsset)
        {
            dialogs = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, DailogueEntry>>>(textAsset.text)["Dialogues"];
        }

        public DailogueEntry GetStartDialogue()
        {
            if (dialogs.ContainsKey("Start"))
                return dialogs["Start"];

            return null;
        }

        public DailogueEntry GetDailogueEntryOfKey(string dialogueKey)
        {
            if (dialogs.ContainsKey(dialogueKey))
                return dialogs[dialogueKey];

            return null;
        }

        public void Cleanup()
        {
            if (dialogs != null)
                dialogs.Clear();
        }
    }
}
