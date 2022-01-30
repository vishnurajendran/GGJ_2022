using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
    {
        const string BATTLE_THEME = "Extreme Energy - Electric Guitar Music - Free Copyright";
        const string MENU_THEME = "Jack The Lumberer - Alexander Nakarada";
        const string CUTSCENE_THEME = "Jack The Lumberer - Alexander Nakarada";
        const string UI_SLOT_ENTER_SFX = "Audio/SFX/UI Tight 12";
        const string UI_WEAPON_EQUIP_SFX = "Audio/SFX/Weapon UI 09";
        const string PICKUP_SFX = "Audio/SFX/LRPG_Positive_Notification";
        const string HIT_SFX = "Audio/SFX/hit";
        const string Dialogue_Select_SFX = "Audio/SFX/UI Tight 12";

        private static AudioManager instance = null;
        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<AudioManager>();
                    if(instance == null)
                    {
                        GameObject obj = Instantiate(Resources.Load<GameObject>("AudioManager"));
                        instance = obj.GetComponent<AudioManager>();
                    }
                    instance.Init();
                }

                return instance;
            }
        }

        [SerializeField] UnityEngine.Audio.AudioMixer mixer;
        [SerializeField] Transform musicTitleParent;
        [SerializeField] GameObject musicTitleRef;

        AudioSource bgSource;
        AudioSource sfxSource;
        AudioSource voSource;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
    }

    private void Init()
        {
            if (bgSource != null)
                Destroy(bgSource.gameObject);

            if (voSource != null)
                Destroy(voSource.gameObject);

            if (sfxSource != null)
                Destroy(sfxSource.gameObject);

            GameObject bgSourceObj = new GameObject("BG Source");
            GameObject sfxSourceObj = new GameObject("SFX Source");
            GameObject voSourceObj = new GameObject("VO Source");

            bgSource = bgSourceObj.AddComponent<AudioSource>();
            sfxSource = sfxSourceObj.AddComponent<AudioSource>();
            voSource = voSourceObj.AddComponent<AudioSource>();

            voSource.outputAudioMixerGroup = mixer.FindMatchingGroups("VO")[0];
            bgSource.outputAudioMixerGroup = mixer.FindMatchingGroups("BG")[0];
            sfxSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];

            bgSourceObj.transform.parent = this.transform;
            sfxSourceObj.transform.parent = this.transform;
            voSourceObj.transform.parent = this.transform;

        DontDestroyOnLoad(this.gameObject);
        }

        public void PlayClip(AudioClip clip)
        {
            sfxSource.PlayOneShot(clip);
        }

        public void PlayMouseEnterItemSlotSFX()
        {
            AudioClip sfxClip = Resources.Load<AudioClip>(UI_SLOT_ENTER_SFX);
            sfxSource.PlayOneShot(sfxClip);
        }

        public void PlayInventoryOpenSFX()
        {
            int rand = Random.Range(1, 16);
            string fileName = "Bag " + (rand < 10 ? "0" + rand : rand.ToString());
            AudioClip sfxClip = Resources.Load<AudioClip>("Audio/SFX/" + fileName);
            sfxSource.PlayOneShot(sfxClip);
        }

        public void PlayWeaponEquipSFX()
        {
            AudioClip sfxClip = Resources.Load<AudioClip>(UI_WEAPON_EQUIP_SFX);
            sfxSource.PlayOneShot(sfxClip);
        }

        public void PlayWhooshSFX()
        {
            int rand = Random.Range(1, 10);
            string fileName = "Whoosh " + (rand < 10 ? "0" + rand : rand.ToString());
            AudioClip sfxClip = Resources.Load<AudioClip>("Audio/SFX/" + fileName);
            sfxSource.PlayOneShot(sfxClip);
        }

        public void PlayPickupSFX()
        {
            AudioClip sfxClip = Resources.Load<AudioClip>(PICKUP_SFX);
            sfxSource.PlayOneShot(sfxClip);
        }


        float interval = 0.03f;
        float nextPlayTime = 0;
        public void PlayDialogueInteractSFX()
        {
            if (Time.time >= nextPlayTime)
            {
                nextPlayTime = Time.time + interval;
                AudioClip sfxClip = Resources.Load<AudioClip>(Dialogue_Select_SFX);
                sfxSource.PlayOneShot(sfxClip);
            }
        }

        public void PlayHitSFX()
        {
            AudioClip sfxClip = Resources.Load<AudioClip>(HIT_SFX);
            GameObject deletableSFXObj = new GameObject("Hit sfx", typeof(AudioSource));
            AudioSource source = deletableSFXObj.GetComponent<AudioSource>();

            source.volume = sfxSource.volume;
            source.PlayOneShot(sfxClip);
            Destroy(deletableSFXObj, sfxClip.length);
        }

        public void StopBG()
        {
            bgSource.Stop();
        }

        public void PlayTheme(string clipName)
        {
            if (string.IsNullOrEmpty(clipName))
            {
                Debug.Log("Clip Empty");
                return;
            }

            AudioClip clip = Resources.Load<AudioClip>("Audio/BG/" + clipName);
            
            Debug.Log(clip.name + " " + bgSource.clip?.name);
            
            if (bgSource.clip == clip)
                return;
            
            bgSource.clip = clip;
            bgSource.loop = true;
            bgSource.Play();
            ShowTitle();
    }

        public void PlayCutSceneTheme()
        {
            PlayTheme(CUTSCENE_THEME);
        }

        public void PlayBattleTheme()
        {
            PlayTheme(BATTLE_THEME);
        }

        public void PlayMenuTheme()
        {
            PlayTheme(MENU_THEME);
        }

    void ShowTitle()
    {
        if (bgSource.clip == null)
            return;

        GameObject go = Instantiate(musicTitleRef, musicTitleParent);
        go.SetActive(true);
        go.GetComponentInChildren<TMPro.TMP_Text>().text = bgSource.clip.name;
        StartCoroutine(ShowMusicTitle(go.GetComponent<CanvasGroup>()));
    }

    IEnumerator ShowMusicTitle(CanvasGroup cg)
    {
        float timeStep = 0;
        cg.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();
        cg.gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(cg.GetComponent<RectTransform>());
        
        while (timeStep <= 1)
        {
            timeStep += Time.deltaTime;
            cg.alpha = Mathf.Lerp(0, 1, timeStep);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(3);

        timeStep = 0;
        while (timeStep <= 1)
        {
            timeStep += Time.deltaTime;
            cg.alpha = Mathf.Lerp(1, 0, timeStep);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();
        Destroy(cg.gameObject);
    }
}