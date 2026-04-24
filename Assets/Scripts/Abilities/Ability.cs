using UnityEngine;

public class Ability : MonoBehaviour
{
    [Header("Base stats")]
    public Utilities.Element element = Utilities.Element.Default;
    public int level = 0;
    public float damage = 20f;
    public float abilitySpeed = 10f;
    public float castTime = 0f;
    public float lifeTime = 10f;
    public float cooldownTime = 5f;

    [Header("Ability scaling")]
    public float perLevelDamageIncrease = 0f;
    public float perLevelDamageMultiplier = 1f;

    [Header("Ability state")]
    public AbilityID ID;
    public bool unlocked = false;
    public bool isOnCooldown = false;
    private float cooldownTimer = 0f;

    void Update()
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
        if (level >= 5) return;

        level++;
        damage += perLevelDamageIncrease;
        damage *= perLevelDamageMultiplier;
    }

    public void StartCooldown()
    {
        isOnCooldown = true;
        cooldownTimer = cooldownTime;
    }

    public float GetCooldownTimer()
    {
        return cooldownTimer;
    }
}
