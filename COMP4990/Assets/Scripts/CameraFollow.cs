using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //What we want to follow?
    public Transform target;
    public Vector3 offset;
    void FixedUpdate() {
        Vector3 desiredPosition = target.position + offset;
        
        transform.position = desiredPosition;
        
        transform.LookAt(target);
    }
}
