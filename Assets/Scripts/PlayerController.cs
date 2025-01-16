using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputAction MoveAction;

    private void Start()
    {
        MoveAction.Enable();
    }

    private void Update()
    {
        Vector2 playerMovement = MoveAction.ReadValue<Vector2>();
        //Debug.Log(playerMovement);

        Vector2 playerPosition = (Vector2)transform.position + playerMovement * 5.0f * Time.deltaTime;

        this.transform.position = playerPosition;
    }
}
