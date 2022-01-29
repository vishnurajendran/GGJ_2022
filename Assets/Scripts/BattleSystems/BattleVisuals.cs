using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public enum EntityType{
    PLAYER,
    ENEMY
}

public class BattleVisuals : MonoBehaviour
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


    [Header("Testing")]
    [SerializeField] bool isTesting = false;
    [SerializeField] Transform pl;
    [SerializeField] Transform en;

    HealthBar playerHealth;
    HealthBar enemyHealth;
    Transform playerTransform;
    Transform enemyTransform;
    GameObject damageTextRef;
    GameObject HealthbarRef;

    public BattleAnimEventComplete onBeginComplete;
    public BattleAnimEventComplete onWinComplete;
    public BattleAnimEventComplete onLoseComplete;

    float i=1;

    private void Start()
    {
        if (isTesting)
        {
            onBeginComplete.AddListener(() =>
            {
                SetupEntity(pl, EntityType.PLAYER);
                SetupEntity(en, EntityType.ENEMY);
            });

            PlayBegin();
        }
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
        return;
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
        Vector3 uiPosUp = new Vector3(uiPos.x, uiPos.y + 50, uiPos.z);
        while (timeStep <= 1)
        {
            timeStep += Time.deltaTime/0.25f;
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
        animator.Play("Win");
    }

    [Button("Play Lose")]
    public void PlayLose()
    {
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
}
