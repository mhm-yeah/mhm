using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    [Header("Base stats")]
    public Utilities.Element element = Utilities.Element.Default;
    public int level = 0;
    public float damage = 20f;
    public float abilitySpeed = 10f;
    public float castTime = 0f;
    public float lifeTime = 10f;
    public float cooldownTime = 5f;
    [HideInInspector] public float currentDamage = 0f;
    [HideInInspector] public float currentCooldownTime = 0f;

    [Header("Ability scaling")]
    public int levelCap = 5;
    public float perLevelDamageIncrease = 0f;
    public float perLevelDamageMultiplier = 1f;
    public float perLevelCooldownReduction = 0f;

    [Header("Ability state")]
    public AbilityID ID;
    public bool unlocked = false;
    public bool isOnCooldown = false;
    public bool isMaxLevel = false;
    private float cooldownTimer = 0f;

    protected virtual void Awake()
    {
        ResetAbility();
    }

    protected virtual void Update()
    {
        if (isOnCooldown)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
                cooldownTimer = 0f;
            }
        }
    }
    public virtual void Activate()
    {
        if (!unlocked)
        {
            enabled = true;
            unlocked = true;
            level = 1;
            OnUnlock();
        }
        else
        {
            LevelUp();
        }
    }
    protected virtual void OnUnlock() { }

    public void LevelUp()
    {
        level++;
        currentDamage += perLevelDamageIncrease;
        currentDamage *= perLevelDamageMultiplier;
        currentCooldownTime -= perLevelCooldownReduction;

        isMaxLevel = level >= levelCap;
    }

    public void ResetAbility()
    {
        enabled = false;
        unlocked = false;
        currentDamage = damage;
        currentCooldownTime = cooldownTime;
    }

    public void StartCooldown()
    {
        isOnCooldown = true;
        cooldownTimer = currentCooldownTime;
        Debug.Log($"Started cooldown for {ID}. Cooldown time: {cooldownTimer}");
    }

    public float GetCooldownTimer()
    {
        return cooldownTimer;
    }

    public abstract Dictionary<string, object> AbilityInfo();
    public abstract Dictionary<string, object> LevelUpInfo();
}
