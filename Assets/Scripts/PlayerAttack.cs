using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Visual Effect")]
    [SerializeField] private GameObject attackEffect;

    private float lastAttackTime;
    private PlayerDefense playerDefense;

    private void Awake()
    {
        playerDefense = GetComponent<PlayerDefense>();
    }

    public void OnAttack(InputValue value)
    {
        if (!value.isPressed)
            return;

        if (playerDefense != null && playerDefense.IsDefending)
            return;

        if (Time.time < lastAttackTime + attackCooldown)
            return;

        lastAttackTime = Time.time;
        PerformAttack();
    }

    private void PerformAttack()
    {
        if (attackPoint == null)
            return;

        if (attackEffect != null)
        {
            GameObject effect = Instantiate(attackEffect, attackPoint.position, attackPoint.rotation);
            Destroy(effect, 0.3f);
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
          attackPoint.position,
          attackRange,
          enemyLayer
        );

        //foreach (Collider2D enemy in hitEnemies)
        //{
        //    EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

        //    if (enemyHealth != null)
        //    {
        //        enemyHealth.TakeDamage(attackDamage);
        //    }
        //}
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}