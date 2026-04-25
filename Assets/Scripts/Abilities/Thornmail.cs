using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Thornmail : Ability
{
    [Header("Thornmail Settings")]
    [SerializeField] private GameObject thornmailPrefab;
    [SerializeField] private GameObject ThornmailObject;
    [SerializeField] private float duration = 3f;   

    private bool isActive = false;
    void Awake()
    {
        enabled = false;
    }
    public void Activate()
    {
        ThornmailObject.SetActive(true);
        unlocked = true;
        enabled = true;

        Debug.Log("Thornmail unlocked!");
    }
    public void OnThornmail(InputValue value)
    {
        if (!unlocked || isActive) return;

        if (value.isPressed && !isOnCooldown)
        {
            StartCoroutine(ActivateThornmail());
        }
    }

    private IEnumerator ActivateThornmail()
    {
        isActive = true;
        if (thornmailPrefab != null)
        {
            thornmailPrefab.SetActive(true);
        }

        Debug.Log("Thornmail Active!");
        yield return new WaitForSeconds(duration);
        if (thornmailPrefab != null)
        {
            thornmailPrefab.SetActive(false);
        }
        isActive = false;
        StartCooldown();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isActive && other.CompareTag("Enemy"))
        {
            EnemyHealth enemy = other.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage); 
            }
        }
    }
}