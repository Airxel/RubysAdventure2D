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
    private int currentHealth = 1;

    private void Start()
    {
        MoveAction.Enable();

        playerRb = GetComponent<Rigidbody2D>();

        //currentHealth = maxHealth;
    }

    private void Update()
    {
        playerMovement = MoveAction.ReadValue<Vector2>();
        //Debug.Log(playerMovement);
    }

    private void FixedUpdate()
    {
        Vector2 playerPosition = (Vector2)playerRb.position + playerMovement * speed * Time.deltaTime;

        playerRb.MovePosition(playerPosition);
    }

    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth +  amount, 0, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
    }
}
