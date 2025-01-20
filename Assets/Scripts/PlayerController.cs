using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Variables referentes al movimiento del jugador
    public InputAction MoveAction;

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

    private void Start()
    {
        MoveAction.Enable();

        playerRb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
    }

    private void Update()
    {
        playerMovement = MoveAction.ReadValue<Vector2>();

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
}
