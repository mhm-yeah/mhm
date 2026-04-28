using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ChargingEnemyMovement : MonoBehaviour
{    enum State
    {
        Chasing,
        Charging,
        Dashing
    }

    State currentState = State.Chasing;

    Transform player;
    EnemyStats enemyStats;
    GameManager manager;
    SpriteRenderer spriteRenderer;

    [Header("Charge Settings")]
    public float chargeDistance = 3f;
    public float chargeTime = 1f;

    [Header("Dash Settings")]
    public float dashSpeed = 10f;
    public float dashDuration = 2f;

    float stateTimer;
    Vector3 dashDirection;
    
    void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        enemyStats = GetComponent<EnemyStats>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(manager.isGameOver || enemyStats.isStunned) return;

        switch (currentState)
        {
            case State.Chasing:
                IsChasing();
                break;
            case State.Dashing:
                IsDashing();
                break;
            case State.Charging:
                IsCharging();
                break;
        }
    }

    private void IsChasing()
    {
        Vector3 direction = (player.position - transform.position);
        float distance = direction.magnitude;
        if(distance < chargeDistance)
        {
            currentState = State.Charging;
            stateTimer = chargeTime;
            dashDirection = direction.normalized;
            return;
        }
        Move(direction.normalized, enemyStats.moveSpeed);
    }

    private void IsCharging()
    {
        stateTimer -= Time.deltaTime;
        if(stateTimer <= 0f)
        {
            currentState = State.Dashing;
            stateTimer = dashDuration;
        }
    }

    private void IsDashing()
    {
        stateTimer -= Time.deltaTime;
        Move(dashDirection, dashSpeed);
        if (stateTimer <= 0f)
        {
            currentState = State.Chasing;
        }
    }

    private void Move(Vector3 direction, float speed)
    {
        transform.position += direction * speed * Time.deltaTime;
        spriteRenderer.flipX = direction.x < 0;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(enemyStats.damage, gameObject);
            if (currentState == State.Dashing)
            {
                currentState = State.Chasing;
            }
        }
    }
}
