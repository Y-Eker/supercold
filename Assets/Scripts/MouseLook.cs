using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] float sensitivity;
    public float mouseX;
    public float mouseY;
    private float xRotation;
    private float yRotation;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxisRaw("Mouse X") * Time.unscaledDeltaTime * sensitivity;
        mouseY = Input.GetAxisRaw("Mouse Y") * Time.unscaledDeltaTime * sensitivity;

        yRotation += mouseX;
        xRotation -= mouseY;

        playerTransform.rotation = Quaternion.Euler(playerTransform.rotation.x, yRotation, playerTransform.rotation.z);
        xRotation = Mathf.Clamp(xRotation, -85, 80);
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}
