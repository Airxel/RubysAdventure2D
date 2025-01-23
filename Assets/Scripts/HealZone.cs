using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealZone : MonoBehaviour
{
    // Variable utilizada para la cantidad de vida que aumentan las zonas de regeneraci�n
    public int healthIncrease = 1;

    /// <summary>
    /// Funci�n que controla la interacci�n del jugador con las zonas de regeneraci�n
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
