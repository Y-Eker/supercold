using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] Rigidbody playerRb;
    [SerializeField] float speed;
    [SerializeField] float speedLimit;
    [SerializeField] float groundDrag;
    [Header("Ground Check")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float playerHeight;
    [SerializeField] float raycastLengthAddition;
    private Vector3 moveDirection;
    private Vector3 currentVelocity;
    private float horizontalInput;
    private float verticalInput;
    private bool onGround;
    
    void Start()
    {
        currentVelocity = new Vector3(0, 0, 0);
    }

    void Update()
    {
        // Gets Movement Input
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    void MovePlayer()
    {
        // Sets The Move Direction
        moveDirection = (transform.forward * verticalInput + transform.right * horizontalInput).normalized;

        // Adds Force To The Direction
        playerRb.AddForce(moveDirection * speed, ForceMode.Force);

    }
    
    void GroundCheck()
    {
        onGround = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + raycastLengthAddition, groundLayer);
    }

    private void FixedUpdate()
    {
        GroundCheck();
        if (onGround)
        {
            playerRb.drag = groundDrag;
        }
        else
        {
            playerRb.drag = 0;
        }
        MovePlayer();
        SpeedLimit();
    }
    
    void SpeedLimit()
    {
        currentVelocity.x = playerRb.velocity.x; currentVelocity.z = playerRb.velocity.z;
        if (currentVelocity.magnitude > speedLimit)
        {
            Vector3 LimitedVel = currentVelocity.normalized * speedLimit;
            playerRb.velocity = new Vector3(LimitedVel.x, playerRb.velocity.y, LimitedVel.z);
        }
    }

}
