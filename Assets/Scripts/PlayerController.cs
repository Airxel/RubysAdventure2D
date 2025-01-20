using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputAction MoveAction;

    private Rigidbody2D playerRb;
    private Vector2 playerMovement;
    public float speed = 5.0f;

    public int maxHealth = 5;

    public int health { get { return currentHealth; } }
    private int currentHealth;

    public float timeInvincible = 2.0f;
    private bool isInvincible;
    private float damageCooldown;

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
