using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    public AudioClip collectibleClip;

    public ParticleSystem collectibleEffect;

    public int healthAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null && controller.health < controller.maxHealth)
        {
            controller.ChangeHealth(healthAmount);

            controller.PlaySound(collectibleClip);

            Instantiate(collectibleEffect, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
