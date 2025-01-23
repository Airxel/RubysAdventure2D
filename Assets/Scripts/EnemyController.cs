using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private AudioSource audioSource;

    private Animator animator;

    private Rigidbody2D enemyRb;
    public float speed = 3.0f;

    public bool vertical = true;

    public float enemyVMovementTime = 3.0f;
    public float enemyHMovementTime = 1.5f;
    private float enemyMovementTimer;
    public int enemyDirection = 1;

    public int enemyDamage = 1;

    private bool broken = true;

    private void Start()
    {
        animator = GetComponent<Animator>();

        enemyRb = GetComponent<Rigidbody2D>();

        audioSource = GetComponent<AudioSource>();

        enemyMovementTimer = enemyVMovementTime;
    }

    private void Update()
    {
        enemyMovementTimer -= Time.deltaTime;

        if (enemyMovementTimer < 0)
        {
            RandomDirection();
        }
    }

    private void FixedUpdate()
    {
        if (!broken)
        {
            return;
        }

        Vector2 enemyPosition = enemyRb.position;

        if (vertical)
        {
            enemyPosition.y = enemyPosition.y + speed * enemyDirection * Time.deltaTime;

            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", enemyDirection);
        }
        else
        {
            enemyPosition.x = enemyPosition.x + speed * enemyDirection * Time.deltaTime;

            animator.SetFloat("Move X", enemyDirection);
            animator.SetFloat("Move Y", 0);
        }

        enemyRb.MovePosition(enemyPosition);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            player.ChangeHealth(-enemyDamage);
        }
    }

    private void RandomDirection()
    {
        if (vertical)
        {
            vertical = !vertical;

            enemyMovementTimer = enemyHMovementTime;
        }
        else if (!vertical && enemyDirection > 0)
        {
            vertical = !vertical;

            enemyDirection = -enemyDirection;

            enemyMovementTimer = enemyVMovementTime;
        }
        else if (vertical && enemyDirection < 0)
        {
            vertical = !vertical;

            enemyMovementTimer = enemyHMovementTime;
        }
        else if (!vertical)
        {
            vertical = !vertical;

            enemyDirection = -enemyDirection;

            enemyMovementTimer = enemyVMovementTime;
        }
    }

    public void Fix()
    {
        broken = false;
        enemyRb.simulated = false;

        audioSource.Stop();
        animator.SetTrigger("Fixed");
    }
}
