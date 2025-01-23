using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    // Variable utilizada para los sonidos de los collectibles
    public AudioClip collectibleClip;

    // Variable utilizada para los efectos de los collectibles
    public ParticleSystem collectibleEffect;

    // Variable utilizada para la cantidad de vida que aumentan los collectibles
    public int healthAmount = 1;

    /// <summary>
    /// Función que controla la interacción del jugador con los collectibles
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null && controller.health < controller.maxHealth)
        {
            controller.ChangeHealth(healthAmount);

            Instantiate(collectibleEffect, transform.position, Quaternion.identity);
            controller.PlaySound(collectibleClip);

            Destroy(gameObject);
        }
    }
}
