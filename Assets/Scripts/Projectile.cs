using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D projectileRb;

    /// <summary>
    /// Funci�n para guardar referencias de los elementos
    /// </summary>
    private void Awake()
    {
        projectileRb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Funci�n que controla la distancia a la que un proyectil deber�a ser destru�do
    /// </summary>
    private void Update()
    {
        if (transform.position.magnitude > 100.0f)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Funci�n que controla la velocidad y direcci�n en la que se dispara el proyectil del jugador
    /// </summary>
    /// <param name="direction"></param>
    /// <param name="force"></param>
    public void Launch(Vector2 direction, float force)
    {
        projectileRb.AddForce(direction * force);
    }

    /// <summary>
    /// Funci�n que controla la interacci�n del proyectil con otros elementos
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyController enemy = other.GetComponent<EnemyController>();

        if (enemy != null)
        {
            enemy.Fix();
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// Funci�n que controla la destrucci�n del proyectil cuando colisiona con algo que no necesita interacci�n
    /// </summary>
    /// <param name="other"></param>
    private void OnCollisionEnter2D(Collision2D other)
    {
        Destroy(gameObject);
    }
}
