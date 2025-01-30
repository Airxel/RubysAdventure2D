using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Variables utilizadas para el sonido de los enemigos
    private AudioSource audioSource;
    public AudioClip enemyFixedClip;

    // Variable utilizada para las animaciones de los enemigos
    private Animator animator;

    // Variables utilizadas para el movimiento de los enemigos
    private Rigidbody2D enemyRb;

    public float enemyVMovementTime = 3.0f;
    public float enemyHMovementTime = 1.5f;
    private float enemyMovementTimer;

    public int enemyDirection = 1;
    public float speed = 3.0f;
    public bool vertical = true;

    // Variable utilizada para saber el daño que hacen los enemigos
    public int enemyDamage = 1;

    // Variable para saber si un enemigo está roto o no
    private bool broken = true;

    // Variable utilizada para los efectos de los enemigos
    public ParticleSystem smokeEffect;

    private void Awake()
    {
        EnemiesContainer.instance.AddEnemies();
    }

    /// <summary>
    /// Función para guardar las referencias de los elementos y establecer valores iniciales
    /// </summary>
    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyRb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        enemyMovementTimer = enemyVMovementTime;
    }

    /// <summary>
    /// Función que controla el tiempo para el cambio de dirección de los enemigos
    /// </summary>
    private void Update()
    {
        enemyMovementTimer -= Time.deltaTime;

        if (enemyMovementTimer < 0)
        {
            RandomDirection();
        }
    }

    /// <summary>
    /// Función que controla el movimiento de los enemigos
    /// </summary>
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

    /// <summary>
    /// Función que controla la interacción de los enemigos con el jugador
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            player.ChangeHealth(-enemyDamage);
        }
    }

    /// <summary>
    /// Función que controla el cambio de dirección, según la dirección anterior
    /// </summary>
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

    /// <summary>
    /// Función que controla si un enemigo se arregla
    /// </summary>
    public void Fix()
    {
        EnemiesContainer.instance.RemoveEnemies();

        broken = false;
        enemyRb.simulated = false;

        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
        audioSource.Stop();
        audioSource.PlayOneShot(enemyFixedClip);
    }
}
