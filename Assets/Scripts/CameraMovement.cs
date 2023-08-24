using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] float sensitivity;
    private float horizontalRotation;
    private float verticalRotation;
    private float xRotation;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        xRotation = 0f;
    }

    void Update()
    {
        horizontalRotation = Input.GetAxis("Mouse X");
        verticalRotation = Input.GetAxis("Mouse Y");
        playerTransform.Rotate(Vector3.up, horizontalRotation * sensitivity * Time.deltaTime);
        xRotation -= verticalRotation * sensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 80f);
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }
}
