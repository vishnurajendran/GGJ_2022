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
    public static BattleVisuals Instancea
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
    [SerializeField] EntityType type;
    [SerializeField] bool hit = false;
    [SerializeField] bool begin = false;
    [SerializeField] bool lose = false;
    [SerializeField] bool win = false;
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

    public void SetupEntity(Transform entity, EntityType type)
    {
        if (HealthbarRef == null)
        {
            HealthbarRef = Resources.Load<GameObject>("HealthBar");
        }

        Vector3 uiPos = Camera.main.WorldToScreenPoint(entity.position) +  new Vector3(50,25,0);
        GameObject healthBarObj = Instantiate(HealthbarRef, statsCanvas);
        healthBarObj.transform.position = uiPos;

        if(type == EntityType.PLAYER)
        {
            playerTransform = entity;
            playerHealth = healthBarObj.GetComponent<HealthBar>();
            playerHealth.SetHealth(1);
        }
        else
        {
            enemyTransform = entity;
            enemyHealth = healthBarObj.GetComponent<HealthBar>();
            enemyHealth.SetHealth(1);
        }
    }

    public void ApplyDamage(EntityType entityType ,int damageValue, float healthPerc)
    {
        if(entityType == EntityType.PLAYER)
        {
            playerHealth.SetHealth(healthPerc);
        }
        else
        {
            enemyHealth.SetHealth(healthPerc);
        }

        ShowDamageText(damageValue, entityType);
    }

    void ShowDamageText(int damage, EntityType type)
    {
        if (damageTextRef == null)
            damageTextRef = Resources.Load<GameObject>("DamageText");

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

    public void PlayBegin()
    {
        animator.Play("Begin");
    }

    public void PlayWin()
    {
        animator.Play("Win");
    }

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
