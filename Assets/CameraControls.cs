using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public float rotationSpeed = 90f;
    public Transform rotationTransform;
    
    void LateUpdate()
    {
        rotationTransform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime * -Input.GetAxisRaw("Horizontal"), 0));
    }
}
