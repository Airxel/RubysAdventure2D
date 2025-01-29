using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Variables utilizadas para el sonido del juego
    private AudioSource audioSource;
    public AudioClip playerHitClip, playerShootClip;

    // Variable utilizada para las animaciones del jugador
    private Animator animator;
    
    // Variables utilizadas para el movimiento del jugador
    public InputAction moveAction;
    private Rigidbody2D playerRb;
    private Vector2 playerMovement;
    private Vector2 moveDirection = new Vector2(1, 0);
    public float speed = 5.0f;

    public InputAction sprintAction;
    public float sprint = 2.0f;

    // Variables utilizadas para el sistema de vidas del jugador
    public int health { get { return currentHealth; } }
    private int currentHealth;
    public int maxHealth = 5;

    // Variables utilizadas para el tiempo de invulnerabilidad del jugador
    public float timeInvincible = 2.0f;
    private bool isInvincible;
    private float damageCooldown;

    // Variables utilizadas para la zona de regeneración del juego
    public float timeHealing = 5.0f;
    private bool isHealing;
    private float healingCooldown;

    // Variables utilizadas para el disparo del jugador
    public InputAction shootAction;
    public GameObject projectilePrefab;
    public float projectileForce = 300f;

    // Variables utilizadas para el diálogo e interacciones del jugador
    public InputAction talkAction;
    public int npcTalked = 0;

    public BoxCollider2D fog1Collider, fog2Collider;

    // Variable utilizada para los efectos del juego
    public ParticleSystem playerHitEffect;

    /// <summary>
    /// Función para activar los InputActions, guardar referencias de los elementos y establecer la vida del jugador
    /// </summary>
    private void Start()
    {
        moveAction.Enable();
        shootAction.Enable();
        talkAction.Enable();
        sprintAction.Enable();

        animator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        currentHealth = maxHealth;
    }

    /// <summary>
    /// Función que controla la dirección del jugador, el período de invencibilidad y curación, los InputActions y las animaciones
    /// </summary>
    private void Update()
    {
        playerMovement = moveAction.ReadValue<Vector2>();

        // Si el jugador se está moviendo
        if (!Mathf.Approximately(playerMovement.x, 0.0f) || !Mathf.Approximately(playerMovement.y, 0.0f))
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

        if (shootAction.triggered)
        {
            Launch();
        }

        if (talkAction.triggered)
        {
            FindFriend();
        }
    }

    /// <summary>
    /// Función que controla el movimiento del jugador
    /// </summary>
    private void FixedUpdate()
    {
        float newSpeed = speed;

        if (sprintAction.IsPressed())
        {
            newSpeed *= sprint;
        }

        Vector2 playerPosition = (Vector2)playerRb.position + playerMovement * newSpeed * Time.deltaTime;

        playerRb.MovePosition(playerPosition);
    }

    /// <summary>
    /// Función que controla la salud del jugador y lo que ocurre según su estado
    /// </summary>
    /// <param name="amount"></param>
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
            Instantiate(playerHitEffect, transform.position, Quaternion.identity);
            PlaySound(playerHitClip); 
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
        
        if (currentHealth <= 0)
        {
            SceneManager.LoadScene("MainScene");
        }

        // Hay que pasar uno de los valores a float, para que no de errores la operación
        UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
    }

    /// <summary>
    /// Función que controla el disparo del proyectil del jugador
    /// </summary>
    private void Launch()
    {
        GameObject newProjectile = Instantiate(projectilePrefab, playerRb.position + Vector2.up, Quaternion.identity);

        Projectile projectile = newProjectile.GetComponent<Projectile>();
        projectile.Launch(moveDirection, projectileForce);

        animator.SetTrigger("Launch");
        PlaySound(playerShootClip);
    }

    /// <summary>
    /// Función que controla la interacción del jugador con otros elementos, usando un raycast a corta distancia
    /// </summary>
    private void FindFriend()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerRb.position + Vector2.up, moveDirection, 1.5f, LayerMask.GetMask("NPC"));

        if (hit.collider != null)
        {
            NonPlayerCharacter npc = hit.collider.GetComponent<NonPlayerCharacter>();
            
            if (npc != null)
            {
                UIHandler.instance.DisplayDialogue(npc.dialogueText, "NPC");
                UIHandler.instance.DisplayTime(npc.displayTime);

                if (!npc.talkedTo)
                {
                    npc.talkedTo = true;

                    npcTalked += 1;

                    // Se deja pasar a la siguiente zona solo cuando se hable con los NPCs necesarios
                    if (npcTalked >= 2)
                    {
                        fog1Collider.enabled = false;
                        fog2Collider.enabled = false;
                    }
                }
            }  
        }

        RaycastHit2D hit2 = Physics2D.Raycast(playerRb.position + Vector2.up, moveDirection, 1.5f, LayerMask.GetMask("NPC2"));

        if (hit2.collider != null)
        {
            NonPlayerCharacter npc = hit2.collider.GetComponent<NonPlayerCharacter>();

            if (npc != null)
            {
                UIHandler.instance.DisplayDialogue(npc.dialogueText, "NPC2");
                UIHandler.instance.DisplayTime(npc.displayTime);

                if (!npc.talkedTo)
                {
                    npc.talkedTo = true;

                    npcTalked += 1;

                    // Se deja pasar a la siguiente zona solo cuando se hable con los NPCs necesarios
                    if (npcTalked >= 2)
                    {
                        fog1Collider.enabled = false;
                        fog2Collider.enabled = false;
                    }
                }
            }
        }

        RaycastHit2D hit3 = Physics2D.Raycast(playerRb.position + Vector2.up, moveDirection, 1.5f, LayerMask.GetMask("Environment"));

        if (hit3.collider != null)
        {
            NonPlayerCharacter fog = hit3.collider.GetComponent<NonPlayerCharacter>();

            if (fog != null)
            {
                UIHandler.instance.DisplayDialogue(fog.dialogueText, "Environment");
                UIHandler.instance.DisplayTime(fog.displayTime);
            }
        }
    }

    /// <summary>
    /// Función que controla algunos efectos de sonido, que suenan una única vez, cada vez
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySound (AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
