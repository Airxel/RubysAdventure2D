using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D enemyRb;
    public float speed = 3.0f;

    public bool vertical = true;

    public float enemyMovementTime = 3.0f;
    private float enemyMovementTimer;
    private int enemyDirection = 1;

    private void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();

        enemyMovementTimer = enemyMovementTime;
    }

    private void Update()
    {
        enemyMovementTimer -= Time.deltaTime;

        if (enemyMovementTimer < 0)
        {
            enemyDirection = -enemyDirection;

            enemyMovementTimer = enemyMovementTime;
        }
    }

    private void FixedUpdate()
    {
        Vector2 enemyPosition = enemyRb.position;

        if (vertical)
        {
            enemyPosition.y = enemyPosition.y + speed * enemyDirection * Time.deltaTime;
        }
        else
        {
            enemyPosition.x = enemyPosition.x + speed * enemyDirection * Time.deltaTime;
        }

        enemyRb.MovePosition(enemyPosition);
    }
}
