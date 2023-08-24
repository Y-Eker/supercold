using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody playerRb;
    [SerializeField] float velocityCap;
    [SerializeField] float playerSpeed;
    private float horizontalInput;
    private float verticalInput;
    void Start()
    {
        
    }

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }
    
    private void MoveCharacter()
    {
        playerRb.AddForce(transform.forward * verticalInput * playerSpeed, ForceMode.VelocityChange);
        playerRb.AddForce(transform.right * horizontalInput * playerSpeed, ForceMode.VelocityChange);
        if (playerRb.velocity.x > velocityCap || playerRb.velocity.x < -velocityCap || playerRb.velocity.z > velocityCap || playerRb.velocity.z < -velocityCap) 
        {
            playerRb.velocity = Vector3.ClampMagnitude(playerRb.velocity, velocityCap);
        }
    }
}
