using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ArrowVolley : Ability
{
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private int arrowCount = 3;
    [SerializeField] private float spreadAngle = 30f;
    [SerializeField] private GameObject arrowVolleyObject;

    private Camera mainCam;
    private GameObject projectilesFolder;

    public override void Activate()
    {
        arrowVolleyObject.SetActive(true);
        base.Activate();
        Debug.Log("Arrow Volley unlocked!");
    }

    void Start()
    {
        mainCam = Camera.main;
        projectilesFolder = GameObject.Find("Projectiles");
    }

    public void OnSpellCast(InputValue input)
    {
        if (!enabled) return; // also for da cards

        if (input.isPressed)
        {
            FireVolley();
        }
    }

    void FireVolley()
    {
        if (isOnCooldown) return;
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue(); //fire where mouse cursor is
        Vector3 mousePos = mainCam.ScreenToWorldPoint(mouseScreenPos);
        mousePos.z = 0f;
        Vector2 direction = (mousePos - transform.position).normalized;
        float startAngle = -spreadAngle / 2f;
        float angleStep = spreadAngle / (arrowCount - 1);

        for (int i = 0; i < arrowCount; i++)
        {
            float angle = startAngle + (angleStep * i);
            Vector2 rotatedDir = RotateVector(direction, angle);
            GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity, projectilesFolder.transform);
            Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
            Projectile projectileScript = arrow.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.Init(this);
            }
            if (rb != null)
            {
                rb.linearVelocity = rotatedDir * abilitySpeed;
            }
            //rotate arrow to face 
            float rotZ = Mathf.Atan2(rotatedDir.y, rotatedDir.x) * Mathf.Rad2Deg;
            arrow.transform.rotation = Quaternion.Euler(0f, 0f, rotZ + 90f); //due to sprite rotation being up
        }
        StartCooldown();
    }

    Vector2 RotateVector(Vector2 v, float degrees)
    {
        float rad = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(rad);
        float cos = Mathf.Cos(rad);

        return new Vector2(
            cos * v.x - sin * v.y,
            sin * v.x + cos * v.y
        );
    }

    public override Dictionary<string, object> AbilityInfo()
    {
        return new Dictionary<string, object>
        {
            { "Level", 1 },
            { "Damage", currentDamage },
            { "Cooldown", currentCooldownTime },
            { "Arrow Count", arrowCount }
        };
    }

    public override Dictionary<string, object> LevelUpInfo()
    {
        return new Dictionary<string, object>
        {
            { "Level", $"{level} -> {level + 1}" },
            { "Damage", $"{currentDamage} -> {currentDamage + perLevelDamageIncrease}" },
            { "Cooldown", $"{currentCooldownTime} -> {currentCooldownTime - perLevelCooldownReduction}" },
            { "Arrow Count", arrowCount }
        };
    }
}