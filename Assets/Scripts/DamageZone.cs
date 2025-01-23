using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    // Variable utilizada para la cantidad de vida que disminuyen las zonas de da�o
    public int healthDecrease = -1;

    /// <summary>
    /// Funci�n que controla la interacci�n del jugador con las zonas de da�o
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null)
        {
            controller.ChangeHealth(healthDecrease);
        }
    }
}
