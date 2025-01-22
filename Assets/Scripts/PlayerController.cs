using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    Vector2 moveDirection = new Vector2(1, 0);

    // Variables referentes al movimiento del jugador
    public InputAction moveAction;

    private Rigidbody2D playerRb;
    private Vector2 playerMovement;
    public float speed = 5.0f;

    // Variables referentes al sistema de vidas
    public int maxHealth = 5;

    public int health { get { return currentHealth; } }
    private int currentHealth;

    // Variables referentes al tiempo de invulnerabilidad
    public float timeInvincible = 2.0f;
    private bool isInvincible;
    private float damageCooldown;

    // Variables referentes a la zona de regeneración
    public float timeHealing = 5.0f;
    private bool isHealing;
    private float healingCooldown;

    public GameObject projectilePrefab;
    public float projectileForce = 300f;

    public InputAction talkAction;

    private void Start()
    {
        moveAction.Enable();

        talkAction.Enable();

        animator = GetComponent<Animator>();

        playerRb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
    }

    private void Update()
    {
        playerMovement = moveAction.ReadValue<Vector2>();

        // Si el jugador se está moviendo
        if(!Mathf.Approximately(playerMovement.x, 0.0f) || !Mathf.Approximately(playerMovement.y, 0.0f))
        {
            moveDirection.Set(playerMovement.x, playerMovement.y);
            moveDirection.Normalize();
        }

        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", playerMovement.magnitude);

        if (isInvincible)
        {
            damageCooldown -= Time.deltaTime;

            if (damageCooldown < 0)
            {
                isInvincible = false;
            }
        }

        if (isHealing)
        {
            healingCooldown -= Time.deltaTime;

            if (healingCooldown < 0)
            {
                isHealing = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            FindFriend();
        }
    }

    private void FixedUpdate()
    {
        Vector2 playerPosition = (Vector2)playerRb.position + playerMovement * speed * Time.deltaTime;

        playerRb.MovePosition(playerPosition);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
            {
                return;
            }

            isInvincible = true;
            damageCooldown = timeInvincible;

            animator.SetTrigger("Hit");
        }

        if (amount > 0)
        {
            if (isHealing)
            {
                return;
            }

            isHealing = true;
            healingCooldown = timeHealing;
        }

        currentHealth = Mathf.Clamp(currentHealth +  amount, 0, maxHealth);
        
        UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
    }

    private void Launch()
    {
        GameObject newProjectile = Instantiate(projectilePrefab, playerRb.position + Vector2.up, Quaternion.identity);

        Projectile projectile = newProjectile.GetComponent<Projectile>();
        projectile.Launch(moveDirection, projectileForce);

        animator.SetTrigger("Launch");
    }

    private void FindFriend()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerRb.position + Vector2.up, moveDirection, 1.5f, LayerMask.GetMask("NPC"));

        if (hit.collider != null)
        {
            Debug.Log("Raycast has hit the object " + hit.collider.gameObject);
        }
    }
}
