using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class AudioManager : MonoBehaviour
    {
        const string BATTLE_THEME = "Audio/BG/05b Battle Theme";
        const string MENU_THEME = "Audio/BG/Menu";
        const string CUTSCENE_THEME = "Audio/BG/11b Downtime";
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
                    instance.Init();
                }

                return instance;
            }
        }

        [SerializeField] UnityEngine.Audio.AudioMixer mixer;

        AudioSource bgSource;
        AudioSource sfxSource;
        AudioSource voSource;

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

        public void PlayCutSceneTheme()
        {
            bgSource.clip = Resources.Load<AudioClip>(CUTSCENE_THEME);
            bgSource.loop = true;
            bgSource.Play();
        }

        public void PlayBattleTheme()
        {
            bgSource.clip = Resources.Load<AudioClip>(BATTLE_THEME);
            bgSource.loop = true;
            bgSource.Play();
        }

    public void PlayMenuTheme()
    {
        bgSource.clip = Resources.Load<AudioClip>(MENU_THEME);
        bgSource.loop = true;
        bgSource.Play();
    }
}