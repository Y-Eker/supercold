using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody playerRb;
    [SerializeField] float velocityCap;
    [SerializeField] float playerSpeed;
    [SerializeField] float decelerationSpeed;
    private float decelerationSpeedZ;
    private float decelerationSpeedX;
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
        if (horizontalInput == 0 && playerRb.velocity.x != 0)
        {
            /*
            if (playerRb.velocity.x < 0)
            {
                playerRb.AddForce(transform.right * decelerationSpeed, ForceMode.Force);
            }
            if (playerRb.velocity.x > 0)
            {
                playerRb.AddForce(transform.right * -decelerationSpeed, ForceMode.Force);
            }
            */
            playerRb.velocity = new Vector3(Mathf.SmoothDamp(playerRb.velocity.x, 0, ref decelerationSpeedX, decelerationSpeed),playerRb.velocity.y, playerRb.velocity.z);
        }
        if (verticalInput == 0 && playerRb.velocity.z != 0)
        {
            /*if (playerRb.velocity.z < 0)
            {
                playerRb.AddForce(transform.forward * decelerationSpeed, ForceMode.Force);
            }
            if (playerRb.velocity.z > 0)
            {
                playerRb.AddForce(transform.forward * -decelerationSpeed, ForceMode.Force);
            }*/

            playerRb.velocity = new Vector3(playerRb.velocity.x, playerRb.velocity.y, Mathf.SmoothDamp(playerRb.velocity.z, 0, ref decelerationSpeedZ, decelerationSpeed));
        }
    }

}
