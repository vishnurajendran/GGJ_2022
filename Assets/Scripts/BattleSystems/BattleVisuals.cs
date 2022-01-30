using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public enum EntityType{
    PLAYER,
    ENEMY
}

public enum HitFXType
{
    NONE,
    LIGHT_HIT,
    HEAVY_HIT,
    LIQUID_LIGHT,
    LIQUID_HEAVY,
    PAPER_LIGHT,
    PAPER_HEAVY,
    HEAL,
    FLIP
}

[System.Serializable]
public class FXData
{
    public GameObject prefab;
    public AudioClip sfx;
}

public class BattleVisuals : SerializedMonoBehaviour
{
    private static BattleVisuals instance;
    public static BattleVisuals Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<BattleVisuals>();

            return instance;
        }
    }

    [SerializeField] Animator animator;
    [SerializeField] Transform statsCanvas;
    [SerializeField] GameObject playerHighlight;
    [SerializeField] GameObject enemyHighlight;
    [SerializeField] GameObject TargetSelectionHelper;

    [SerializeField] GameObject playerHighlightTrigger;
    [SerializeField] GameObject enemyHighlightTrigger;
    [SerializeField] Transform pl;
    [SerializeField] Transform enParent;
    [SerializeField] GameObject enObj;

    [Header("Testing")]
    [SerializeField] bool isTesting = false;
    [SerializeField] EntityType target;
    [SerializeField] HitFXType hitFXType;

    HealthBar playerHealth;
    HealthBar enemyHealth;
    Transform playerTransform;
    Transform enemyTransform;
    GameObject damageTextRef;
    GameObject HealthbarRef;

    [OdinSerialize] Dictionary<HitFXType, FXData> prefabsRef; 


    public BattleAnimEventComplete onBeginComplete;
    public BattleAnimEventComplete onWinComplete;
    public BattleAnimEventComplete onLoseComplete;

    float i=1;

    public bool Win = false;
    public bool InBattle = false;

    public Transform NpcHolder;

    Animator playerAnimator;
    Animator enemyAnimator;

    private void Start()
    {
        onBeginComplete.AddListener(() =>
        {
            Win = false;
            InBattle = true;
        });

        onLoseComplete.AddListener(() => { GameMenuController.Instance.ShowMenu(MenuType.LOSE); });
        onWinComplete.AddListener(() => { GameMenuController.Instance.ShowMenu(MenuType.WIN); });
    }

    public void SetupEntity(Transform entity, EntityType type)
    {

        if (HealthbarRef == null)
        {
            HealthbarRef = Resources.Load<GameObject>("HealthBar");
        }

        Vector3 uiPos = Camera.main.WorldToScreenPoint(entity.position);
        GameObject healthBarObj = Instantiate(HealthbarRef, statsCanvas);
        healthBarObj.transform.position = uiPos;

        if(type == EntityType.PLAYER)
        {
            if(playerHealth != null)
                Destroy(playerHealth.gameObject);

            playerTransform = entity;
            playerHealth = healthBarObj.GetComponent<HealthBar>();
            playerHealth.SetHealth(1,1);
        }
        else
        {
            if (enemyHealth != null)
                Destroy(enemyHealth.gameObject);

            enemyTransform = entity;
            enemyHealth = healthBarObj.GetComponent<HealthBar>();
            enemyHealth.SetHealth(1,1);
        }
    }

    [Button("Hit Enemy")]
    void TestHitEnemy()
    {
        ApplyDamage(EntityType.ENEMY, 10,enemyHealth.RemainingHealth - 0.1f);
    }

    [Button("Hit Player")]
    void TestHitPlayer()
    {
        ApplyDamage(EntityType.PLAYER, 10, playerHealth.RemainingHealth - 0.1f);
    }

    public void ApplyDamage(EntityType entityType ,int damageValue, float healthPerc, bool isGain=false)
    {
        healthPerc = Mathf.Clamp01(healthPerc);
        if(entityType == EntityType.PLAYER)
        {
            if (playerHealth.RemainingHealth <= 0)
                return;

            playerHealth.SetHealth(healthPerc);
        }
        else
        {
            if (enemyHealth.RemainingHealth <= 0)
                return;

            enemyHealth.SetHealth(healthPerc);
        }

        ShowDamageText(damageValue, entityType, isGain);
    }

    void ShowDamageText(int damage, EntityType type, bool isGain)
    {
        if (isGain)
            damageTextRef = Resources.Load<GameObject>("DamageText_Green");
        else
            damageTextRef = Resources.Load<GameObject>("DamageText_Red");

        Transform entityTransform = type == EntityType.PLAYER ? playerTransform : enemyTransform;
        Vector3 uiPos = Camera.main.WorldToScreenPoint(enemyTransform.position);
        GameObject obj = Instantiate(damageTextRef, statsCanvas);
        obj.GetComponent<TMPro.TMP_Text>().text = damage.ToString();
        obj.transform.position = uiPos;
        StartCoroutine(MoveDamageTextUp(obj.GetComponent<RectTransform>(), entityTransform));
    }

    IEnumerator MoveDamageTextUp(Transform uiDmgText, Transform entityTransform)
    {
        float timeStep = 0;
        Vector3 uiPos = Camera.main.WorldToScreenPoint(entityTransform.position);
        Vector3 uiPosUp = new Vector3(uiPos.x, uiPos.y + 75, uiPos.z);
        while (timeStep <= 1)
        {
            timeStep += Time.deltaTime/0.5f;
            uiDmgText.position = Vector3.Lerp(uiPos, uiPosUp, timeStep);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
        Destroy(uiDmgText.gameObject);
    }

    [Button("Play Begin")]
    public void PlayBegin()
    {
        animator.Play("Begin");
    }

    [Button("Play Win")]
    public void PlayWin()
    {
        Win = true;
        InBattle = false;
        animator.Play("Win");
    }

    [Button("Play Lose")]
    public void PlayLose()
    {
        Win = false;
        InBattle = false;
        animator.Play("Lose");
    }

    public void InvokeBeginComplete()
    {
        onBeginComplete?.Invoke();
    }

    public void InvokeWinComplete()
    {
        onWinComplete?.Invoke();
    }

    public void InvokeLoseComplete()
    {
        onLoseComplete?.Invoke();
    }

    public void AddHitFX(HitFXType type, Transform transform)
    {
        if (transform == null || type == HitFXType.NONE)
            return;

        Instantiate(prefabsRef[type].prefab, transform.position, prefabsRef[type].prefab.transform.rotation);
        AudioManager.Instance.PlayClip(prefabsRef[type].sfx);
    }

    public void ShowSelector(EntityType type)
    {
        if(type == EntityType.PLAYER)
        {
            enemyHighlight.SetActive(false);
            playerHighlight.SetActive(true);
        }
        else
        {
            enemyHighlight.SetActive(true);
            playerHighlight.SetActive(false);
        }
    }

    public void HideSelectors()
    {
        enemyHighlight.SetActive(false);
        playerHighlight.SetActive(false);
    }

    public void HideSelectorTriggers() { 
        TargetSelectionHelper.SetActive(false);
        enemyHighlightTrigger.SetActive(false);
        playerHighlightTrigger.SetActive(false);
    }

    public void ShowSelectorTriggers() {
        TargetSelectionHelper.SetActive(true);
        enemyHighlightTrigger.SetActive(true);
        playerHighlightTrigger.SetActive(true);
    }

    [Button("SimulateHit")]
    void HitSimulate()
    {
        AddHitFX(hitFXType, target == EntityType.PLAYER?pl:enParent);
    }

    public void LoadEnemy(string enemyName)
    {
        GameObject obj = Resources.Load<GameObject>("Characters/Character_"+enemyName);
        enObj = Instantiate(obj, enParent);
        NpcHolder = enObj.transform;
    }

    public void Attack(EntityType type)
    {
        if (EntityType.PLAYER == type)
        {
            if(playerAnimator == null)
            {
                playerAnimator = pl.GetComponentInChildren<Animator>();
            }

            Debug.Log("Player ATTACK!!");
            playerAnimator.SetTrigger("Attack");
        }
        else
        {
            if (enemyAnimator == null)
            {
                enemyAnimator = enObj.GetComponentInChildren<Animator>();
            }

            Debug.Log("Enemy ATTACK!!");
            enemyAnimator.SetTrigger("Attack");
        }
    }
}
