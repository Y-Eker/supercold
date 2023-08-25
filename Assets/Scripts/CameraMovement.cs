using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform cameraPosTransform;
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = cameraPosTransform.position;
    }
}
