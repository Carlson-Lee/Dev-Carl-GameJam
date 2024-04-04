using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniGameCameraController : MonoBehaviour
{

    public Transform playerTransform;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void FixedUpdate()
    {
        // Calculate desired position for the camera
        Vector3 desiredPosition = playerTransform.position + offset;
        desiredPosition.y = transform.position.y; // Lock the camera's y-position

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
