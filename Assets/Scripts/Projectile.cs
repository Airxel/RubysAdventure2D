using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Projectile : MonoBehaviour
{
    Rigidbody2D projectileRb;

    private void Awake()
    {
        projectileRb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        
    }

    public void Launch(Vector2 direction, float force)
    {
        projectileRb.AddForce(direction * force);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Projectile collision with " + other.gameObject);
        Destroy(gameObject);
    }
}
