using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{
    private PlayerMove playerMove;
    private CharacterController characterController;

    Vector3 Offset = new Vector3(0f, 1.0f, 0f);

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        characterController = GetComponent<CharacterController>();
    }
    private void Update()
    {
        if (GameManager.instance.isGameover) return;

        Check_Ground();
    }
    private void Check_Ground()
    {
        if (characterController.isGrounded) playerMove.IsGround = true;
        else
        {
            RaycastHit hit;
            playerMove.IsGround = Physics.Raycast(transform.position - Offset, Vector3.down, out hit, 0.15f);
        }
    }
}
