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

    [System.Serializable]
    public class DialogueStruct
    {
        public string Background;
        public string BattleMusic = "Extreme Energy - Electric Guitar Music - Free Copyright";
        public string CutsceneMusic = "Jack The Lumberer - Alexander Nakarada";
        public Dictionary<string, DailogueEntry> Dialogues;
    }

    public class DialogueOptionData
    {
        public string OptionText;
        public string JumpToDialogueID;
    }

    public class DialogParser
    {
        DialogueStruct dStruct;

        Dictionary<string, DailogueEntry> dialogs;

        public void LoadDialogFromTextAsset(TextAsset textAsset)
        {
            dStruct =  Newtonsoft.Json.JsonConvert.DeserializeObject<DialogueStruct>(textAsset.text);
            dialogs = dStruct.Dialogues;
            CutsceneManager.Instance.SetImage(Resources.Load<Sprite>("Floors/" + dStruct.Background));
        }

        public string CutsceneTheme
        {
            get
            {
                if (dStruct != null)
                    return dStruct.CutsceneMusic;

                return string.Empty;
            }
        }

        public string BattleTheme
        {
            get
            {
                if (dStruct != null)
                    return dStruct.BattleMusic;

                return string.Empty;
            }
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
