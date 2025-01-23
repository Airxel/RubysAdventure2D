using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealZone : MonoBehaviour
{
    // Variable utilizada para la cantidad de vida que aumentan las zonas de regeneración
    public int healthIncrease = 1;

    /// <summary>
    /// Función que controla la interacción del jugador con las zonas de regeneración
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null)
        {
            controller.ChangeHealth(healthIncrease);
        }
    }
}
