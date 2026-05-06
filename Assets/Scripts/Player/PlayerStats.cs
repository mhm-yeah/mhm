using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base stats")]
    public float baseMoveSpeed = 5f;
    public float sprintSpeed = 8f;
    public float baseMaxHealth = 100f;
    public float baseDamage = 0f;
    public float baseAttackSpeed = 0f;
    public float baseDefense = 0f;

    [Header("Live stats")]
    public float moveSpeed = 5f;
    public float maxHealth = 100f;
    public float damage = 25f;
    public float attackSpeed = 3f;
    public float defense = 0f;

    [Header("Chance stats")]
    [Range(0f, 1f)]
    public float dodgeChance = 0.2f;

    [Header("Player status")]
    private bool canAttack = true;
    private bool isStunned = false;
    private bool isInvulnerable = false;
    
    private SpriteRenderer sprite;
    private Color originalColor;

    private GameManager gameManager;
    private Weapon[] weapons;
    private Weapon currentWeapon;
    private Utilities.Element currentElement = Utilities.Element.Default;
    private int weaponInd = 0;


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentWeapon = transform.GetComponent<Gun>();
        weapons = transform.GetComponents<Weapon>();
        sprite = transform.GetComponent<SpriteRenderer>();
        originalColor = sprite.color;

        ChangeStats();
    }

    void Update()
    {
        if (gameManager.isGameOver) return;

        if (Input.GetKeyDown(KeyCode.F)) // change to actionmap input later
        {
            ChangeWeapon();
        }
    }

    public bool GetWeaponActionStatus()
    {
        return canAttack;
    }

    public bool GetStunStatus()
    {
        return isStunned;
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public Utilities.Element GetCurrentElement()
    {
        return currentElement;
    }

    public void ApplyWeaponCooldown()
    {
        canAttack = false;
        Invoke(nameof(RemoveWeaponCooldown), 1f / attackSpeed);
    }

    private void RemoveWeaponCooldown()
    {
        canAttack = true;
    }

    private void ChangeWeapon()
    {
        weapons[weaponInd].enabled = false;
        weaponInd = (weaponInd + 1) % weapons.Length;
        Debug.Log(weaponInd + ", Equipped: " + weapons[weaponInd]);
        weapons[weaponInd].enabled = true;
        currentWeapon = weapons[weaponInd];
        ChangeStats();
    }

    private void ChangeStats()
    {
        Weapon weapon = weapons[weaponInd];
        currentElement = weapon.element;
        damage = baseDamage + weapon.damage;
        attackSpeed = baseAttackSpeed + weapon.attackSpeed;
    }

    //public bool HasElementalSynergy(Utilities.Element element)
    //{
    //    //return true;

    //    Weapon weapon = GetCurrentWeapon();
    //    if (weapon == null || weapon.element != element)
    //        return false;

    //    Ability[] abilities = GetComponents<Ability>();
    //    foreach (Ability ability in abilities)
    //    {
    //        if (ability.unlocked && ability.element == element)
    //            return true;
    //    }
    //    return false;
    //}
    public bool HasElementalSynergy(Utilities.Element element)
    {
        int count = 0;

        Weapon weapon = GetCurrentWeapon();
        if (weapon != null && weapon.element == element)
            count++;

        Ability[] abilities = GetComponents<Ability>();
        foreach (Ability ability in abilities)
        {
            if (ability.unlocked && ability.element == element)
                count++;
        }

        return count >= 2;
    }

    public void Stun(float duration)
    {
        if (isStunned == false)
        {
            isStunned = true;
            canAttack = false;
            
            sprite.color = Color.cyan;

            Invoke(nameof(Unstun), duration);
        }
    }

    private void Unstun()
    {
        if (transform != null) // it is possible for the player to die while stunned.
        {
            isStunned = false;
            canAttack = true;
            sprite.color = originalColor;
        }
    }
}
