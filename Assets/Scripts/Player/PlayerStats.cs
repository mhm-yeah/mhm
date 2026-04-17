using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base stats")]
    public float baseMoveSpeed = 5f;
    public float sprintSpeed = 8f;
    public float baseMaxHealth = 100f;
    public float baseDamage = 25f;
    public float baseFireRate = 3f;
    public float baseDefense = 0f;

    [Header("Live stats")]
    public float moveSpeed = 5f;
    public float maxHealth = 100f;
    public float damage = 25f;
    public float fireRate = 3f;
    public float defense = 0f;

    [Header("Chance stats")]
    [Range(0f, 1f)]
    public float dodgeChance = 0.2f;

    [Header("Player status")]
    private bool canAttack = true;

    private GameManager gameManager;
    private Weapon[] weapons;
    private Weapon currentWeapon;
    private int weaponInd = 0;


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        currentWeapon = transform.GetComponent<Gun>();
        weapons = transform.GetComponents<Weapon>();
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

    public void ApplyWeaponCooldown()
    {
        canAttack = false;
        Invoke(nameof(ResetWeapon), 1f / fireRate);
    }

    private void ResetWeapon()
    {
        canAttack = true;
    }

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    private void ChangeWeapon()
    {
        weapons[weaponInd].enabled = false;
        weaponInd = (weaponInd + 1) % weapons.Length;
        Debug.Log(weaponInd + ", Equipped: " + weapons[weaponInd]);
        weapons[weaponInd].enabled = true;
        currentWeapon = weapons[weaponInd];
    }
}
