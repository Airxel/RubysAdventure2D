using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip playerHitClip, playerShootClip;

    private Animator animator;
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

    public InputAction shootAction;

    public InputAction talkAction;

    public ParticleSystem playerHitEffect;

    public int npcTalked = 0;

    public BoxCollider2D fog1Collider, fog2Collider;

    private void Start()
    {
        moveAction.Enable();
        shootAction.Enable();
        talkAction.Enable();

        animator = GetComponent<Animator>();

        playerRb = GetComponent<Rigidbody2D>();

        audioSource = GetComponent<AudioSource>();

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
            audioSource.Play();
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
            PlaySound(playerHitClip);

            Instantiate(playerHitEffect, transform.position, Quaternion.identity);
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

        UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
    }

    private void Launch()
    {
        GameObject newProjectile = Instantiate(projectilePrefab, playerRb.position + Vector2.up, Quaternion.identity);

        Projectile projectile = newProjectile.GetComponent<Projectile>();
        projectile.Launch(moveDirection, projectileForce);

        animator.SetTrigger("Launch");
        PlaySound(playerShootClip);
    }

    private void FindFriend()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerRb.position + Vector2.up, moveDirection, 1.5f, LayerMask.GetMask("NPC"));

        if (hit.collider != null)
        {
            NonPlayerCharacter npc = hit.collider.GetComponent<NonPlayerCharacter>();
            
            if (npc != null)
            {
                UIHandler.instance.DisplayDialogue(npc.dialogueText);

                if (!npc.talkedTo)
                {
                    npc.talkedTo = true;

                    npcTalked += 1;

                    if (npcTalked >= 2)
                    {
                        fog1Collider.enabled = false;
                        fog2Collider.enabled = false;
                    }
                }
            }  
        }

        RaycastHit2D hit2 = Physics2D.Raycast(playerRb.position + Vector2.up, moveDirection, 1.5f, LayerMask.GetMask("Environment"));

        if (hit2.collider != null)
        {
            NonPlayerCharacter fog = hit2.collider.GetComponent<NonPlayerCharacter>();

            if (fog != null)
            {
                UIHandler.instance.DisplayDialogue(fog.dialogueText);
            }
        }
    }

    public void PlaySound (AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
